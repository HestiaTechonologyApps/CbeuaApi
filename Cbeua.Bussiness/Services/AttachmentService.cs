using Microsoft.AspNetCore.Http;
using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.Domain.Entities.Common;

public class AttachmentService : IAttachmentService
{
    private readonly IAttachmentRepository _repo;
    private readonly ICurrentUserService _currentUser;
    public AttachmentService(IAttachmentRepository repo, ICurrentUserService currentUser)
    {
        _repo = repo;
      
        _currentUser = currentUser;
    }

    public async Task<List<Attachment>> GetAllAsync() => (List<Attachment>)await _repo.GetAllAsync();

    public async Task<Attachment?> GetByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<Attachment> CreateAsync(Attachment category)
    {
        await _repo.AddAsync(category);
        await _repo.SaveChangesAsync();
        return category;
    }

    // Root that every stored attachment FilePath must live under (matches AttachmentController.Upload).
    private static string UploadsRoot =>
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    public async Task<bool> UpdateAsync(Attachment attachment)
    {
        var existing = await _repo.GetByIdAsync(attachment.AttachmentId);
        if (existing == null || existing.IsDeleted) return false;

        // The edit screen may point the record at a newly uploaded file, but only one inside the uploads folder.
        if (!string.IsNullOrWhiteSpace(attachment.FilePath) && attachment.FilePath != existing.FilePath)
        {
            if (!FileSafetyHelper.IsUnderRoot(UploadsRoot, attachment.FilePath))
                throw new BusinessRuleException("Invalid attachment file path.");
            existing.FilePath = attachment.FilePath;
            existing.AttachmentPath = attachment.FilePath;
            existing.FileName = attachment.FileName;
            existing.FileSize = attachment.FileSize;
            existing.FileType = attachment.FileType;
            existing.AttachmentType = attachment.AttachmentType;
        }

        existing.Description = attachment.Description;
        if (FileSafetyHelper.IsSafeSegment(attachment.TableName))
            existing.TableName = attachment.TableName;
        existing.RecordID = attachment.RecordID;

        _repo.Update(existing);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return false;
        _repo.Delete(category);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<(Stream? FileStream, string? FileName, string? ContentType, string? ErrorMessage)> DownloadAttachmentAsync(int attachmentId)
    {
        var attachment = await _repo.GetByIdAsync(attachmentId);
        if (attachment == null || attachment.IsDeleted)
            return (null, null, null, "Attachment not found");

        var safePath = ResolveStoredUploadPath(attachment.FilePath);
        if (safePath == null)
            return (null, null, null, "Attachment not found");

        if (!File.Exists(safePath))
            return (null, null, null, "File not found on server");

        var fileStream = new FileStream(safePath, FileMode.Open, FileAccess.Read);
        var contentType = GetContentType(attachment.FileName);
        return (fileStream, attachment.FileName, contentType, null);
    }

    /// <summary>
    /// Returns the stored path if it lies inside the uploads folder. Older rows may hold an absolute path from
    /// a previous deploy folder, so the part after "wwwroot\uploads\" is re-rooted under the current folder.
    /// Anything else (e.g. a tampered path to appsettings.json) returns null.
    /// </summary>
    private static string? ResolveStoredUploadPath(string? storedPath)
    {
        if (string.IsNullOrWhiteSpace(storedPath)) return null;
        if (FileSafetyHelper.IsUnderRoot(UploadsRoot, storedPath)) return storedPath;

        var normalized = storedPath.Replace('/', '\\');
        const string marker = "\\wwwroot\\uploads\\";
        var idx = normalized.LastIndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return null;

        var candidate = Path.Combine(UploadsRoot, normalized[(idx + marker.Length)..].Replace('\\', Path.DirectorySeparatorChar));
        return FileSafetyHelper.IsUnderRoot(UploadsRoot, candidate) ? candidate : null;
    }

    private string GetContentType(string fileName)
    {
        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
            contentType = "application/octet-stream";
        return contentType;
    }

    public async Task<CustomApiResponse> UploadFileAsync(IFormFile file, string uploadPath, string tableName, int recordId, string description)
    {
        if (file == null || file.Length == 0)
            return ApiResponseFactory.Fail("File is null or empty", System.Net.HttpStatusCode.BadRequest);

        if (!FileSafetyHelper.IsSafeSegment(tableName))
            return ApiResponseFactory.Fail("Invalid table name", System.Net.HttpStatusCode.BadRequest);

        if (!FileSafetyHelper.IsUnderRoot(UploadsRoot, uploadPath))
            return ApiResponseFactory.Fail("Invalid upload path", System.Net.HttpStatusCode.BadRequest);

        var originalName = FileSafetyHelper.SanitizeFileName(file.FileName);
        if (FileSafetyHelper.IsBlockedExtension(originalName))
            return ApiResponseFactory.Fail("This file type is not allowed", System.Net.HttpStatusCode.BadRequest);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var fileName = $"{Guid.NewGuid()}_{originalName}";
        var filePath = Path.Combine(uploadPath, fileName);
        if (!FileSafetyHelper.IsUnderRoot(UploadsRoot, filePath))
            return ApiResponseFactory.Fail("Invalid file name", System.Net.HttpStatusCode.BadRequest);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var fileSizeInKb = Math.Round((double)file.Length / 1024, 2);

        var attachment = new Attachment
        {
            TableName = tableName,
            RecordID = recordId,
            Description = description,
            FileName = originalName,
            FileType = Path.GetExtension(originalName),
            AttachmentType = Path.GetExtension(originalName),
            FileSize = fileSizeInKb.ToString(),
            FilePath = filePath,
            AttachmentPath = filePath,
            UploaddedOn = DateTime.UtcNow,
            UploadedBy = _currentUser?.UserId?.ToString() ?? "System"
        };

        await _repo.AddAsync(attachment);
        await _repo.SaveChangesAsync();
        return ApiResponseFactory.Success(attachment, "File uploaded successfully", System.Net.HttpStatusCode.OK);
    }

    public async Task<CustomApiResponse> GetAttachmentsAsync(string tableName, int recordId)
    {
        var attachments = await _repo.FindAsync(a => a.TableName == tableName && a.RecordID == recordId && !a.IsDeleted);
        return ApiResponseFactory.Success(attachments, string.Empty, System.Net.HttpStatusCode.OK);
    }

    public async Task<CustomApiResponse> DeleteAttachmentAsync(int attachmentId, string deletedBy)
    {
        var attachment = await _repo.GetByIdAsync(attachmentId);
        if (attachment == null)
            return ApiResponseFactory.Fail("Attachment not found", System.Net.HttpStatusCode.NotFound);

        attachment.IsDeleted = true;
        attachment.DeletedOn = DateTime.UtcNow;
        attachment.DeletedBy = deletedBy;

        _repo.Update(attachment);
        await _repo.SaveChangesAsync();
        return ApiResponseFactory.Success(null, "Attachment deleted successfully", System.Net.HttpStatusCode.OK);
    }
}