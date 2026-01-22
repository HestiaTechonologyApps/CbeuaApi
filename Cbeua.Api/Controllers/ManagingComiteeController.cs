using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManagingComiteeController : ControllerBase
    {
        private readonly IManagingComiteeService _service;

        public ManagingComiteeController(IManagingComiteeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var managingComitees = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = managingComitees;
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }

        [HttpGet("{id}")]
        public async Task<CustomApiResponse> GetById(int id)
        {
            var response = new CustomApiResponse();
            var managingComitee = await _service.GetByIdAsync(id);
            if (managingComitee == null)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = managingComitee;
                response.StatusCode = 200;
            }
            return response;
        }

        [HttpPost]
        public async Task<CustomApiResponse> Create([FromBody] ManagingComitee managingComitee)
        {
            var response = new CustomApiResponse();
            try
            {
                var created = await _service.CreateAsync(managingComitee);
                response.IsSucess = true;
                response.Value = created;
                response.StatusCode = 201;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }

        [HttpPut("{id}")]
        public async Task<CustomApiResponse> Update(int id, [FromBody] ManagingComitee managingComitee)
        {
            var response = new CustomApiResponse();
            if (id != managingComitee.ManagingComiteeId)
            {
                response.IsSucess = false;
                response.Error = "Id mismatch";
                response.StatusCode = 400;
                return response;
            }
            var updated = await _service.UpdateAsync(managingComitee);
            if (!updated)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = managingComitee;
                response.StatusCode = 200;
            }
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<CustomApiResponse> Delete(int id)
        {
            var response = new CustomApiResponse();
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = null;
                response.StatusCode = 204;
            }
            return response;
        }

        // ADD THIS NEW ENDPOINT
        /// <summary>
        /// Uploads the Image for Managing Committee member
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("upload-image")]
        [Consumes("multipart/form-data")]
        public async Task<CustomApiResponse> UploadImage([FromForm] ManagingComiteeDTO dto)
        {
            var managingComiteeId = dto.ManagingComiteeId;
            var image = dto.Image;

            if (image == null || image.Length == 0)
                return new CustomApiResponse { IsSucess = false, Error = "No file uploaded", StatusCode = 400 };

            // Check file size (max 2MB)
            const long maxFileSize = 2 * 1024 * 1024;
            if (image.Length > maxFileSize)
                return new CustomApiResponse { IsSucess = false, Error = "File size exceeds 2MB", StatusCode = 400 };

            // Check file type (allow only images)
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedContentTypes.Contains(image.ContentType.ToLower()))
                return new CustomApiResponse { IsSucess = false, Error = "Only image files (jpg, png, gif, webp) are allowed", StatusCode = 400 };

            // Get managing comitee to check for old image
            var managingComitee = await _service.GetByIdAsync(managingComiteeId);
            if (managingComitee == null)
                return new CustomApiResponse { IsSucess = false, Error = "Managing Committee member not found", StatusCode = 404 };

            // Prepare file path
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "committeeimages");
            Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(image.FileName);
            var fileName = $"{managingComiteeId}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Delete old image if exists and is not empty
            if (!string.IsNullOrEmpty(managingComitee.imageLocation))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", managingComitee.imageLocation.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(oldFilePath))
                {
                    try { System.IO.File.Delete(oldFilePath); } catch { /* ignore file delete errors */ }
                }
            }

            // Save new file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Save relative path to DB
            var relativePath = $"/committeeimages/{fileName}";
            var result = await _service.UpdateImageAsync(managingComiteeId, relativePath);

            return result;
        }
    }
}