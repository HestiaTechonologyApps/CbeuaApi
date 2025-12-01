using Microsoft.AspNetCore.Http;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities.Common;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IAttachmentService
    {

        Task<List<Attachment>> GetAllAsync();
        Task<Attachment?> GetByIdAsync(int id);
        Task<Attachment> CreateAsync(Attachment attachment);
        Task<bool> UpdateAsync(Attachment attachment);
        Task<bool> DeleteAsync(int id);

        Task<(Stream? FileStream, string? FileName, string? ContentType, string? ErrorMessage)> DownloadAttachmentAsync(int attachmentId);
        Task<CustomApiResponse> UploadFileAsync(IFormFile file,string uploadPath, string tableName, int recordId, string description);
        Task<CustomApiResponse> GetAttachmentsAsync(string tableName, int recordId);
        Task<CustomApiResponse> DeleteAttachmentAsync(int attachmentId, string deletedBy);

    }
}
