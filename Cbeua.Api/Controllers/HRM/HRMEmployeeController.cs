using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Interfaces.IServices.HRMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cbeua.Api.Controllers.HRMS
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class HRMEmployeeController : ControllerBase
    {
        private readonly IHRMEmployeeService _service;

        public HRMEmployeeController(IHRMEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var data = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = data;
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
            try
            {
                var data = await _service.GetByIdAsync(id);
                if (data == null)
                {
                    response.IsSucess = false;
                    response.Error = "Not found";
                    response.StatusCode = 404;
                }
                else
                {
                    response.IsSucess = true;
                    response.Value = data;
                    response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }

        [HttpPost]
        public async Task<CustomApiResponse> Create([FromBody] HRMEmployeeCreateUpdateDTO entitydto)
        {
            return await _service.CreateAsync(entitydto);
        }

        [HttpPut("{id}")]
        public async Task<CustomApiResponse> Update(int id, [FromBody] HRMEmployeeCreateUpdateDTO entitydto)
        {
            try
            {
                if (id != entitydto.Id)
                    return ApiResponseFactory.Fail("Id mismatch", System.Net.HttpStatusCode.BadRequest);
                return await _service.UpdateAsync(entitydto);
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.Exception(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<CustomApiResponse> Delete(int id)
        {
            try
            {
                return await _service.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.Exception(ex);
            }
        }
        [HttpPost("upload-profile-pic")]
        [Consumes("multipart/form-data")]
        public async Task<CustomApiResponse> UploadProfilePic([FromForm] EmployeeProfilePicUploadDto dto)
        {
            var employeeId = dto.Id;  // Changed from AppUserId
            var profilePic = dto.ProfilePic;

            if (profilePic == null || profilePic.Length == 0)
                return new CustomApiResponse { IsSucess = false, Error = "No file uploaded", StatusCode = 400 };

            // Check file size (max 2MB)
            const long maxFileSize = 2 * 1024 * 1024;
            if (profilePic.Length > maxFileSize)
                return new CustomApiResponse { IsSucess = false, Error = "File size exceeds 2MB", StatusCode = 400 };

            // Check file type (allow only images and gifs)
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedContentTypes.Contains(profilePic.ContentType.ToLower()))
                return new CustomApiResponse { IsSucess = false, Error = "Only image files (jpg, png, gif, webp) are allowed", StatusCode = 400 };

            // Get member to check for old profile pic
            var employee = await _service.GetByIdAsync(employeeId);  // Changed from appUserId
            if (employee == null)
                return new CustomApiResponse { IsSucess = false, Error = "Employee not found", StatusCode = 404 };

            // Prepare file path
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profilepics");
            Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(profilePic.FileName);
            var fileName = $"{employeeId}_{Guid.NewGuid()}{fileExtension}";  // Changed from appUserId
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Delete old profile pic if exists and is not empty
            if (!string.IsNullOrEmpty(employee.ProfileImagePath))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", employee.ProfileImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(oldFilePath))
                {
                    try { System.IO.File.Delete(oldFilePath); } catch { /* ignore file delete errors */ }
                }
            }

            // Save new file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePic.CopyToAsync(stream);
            }

            // Save relative path to DB
            var relativePath = $"/profilepics/{fileName}";
            var result = await _service.UpdateProfilePicAsync(employeeId, relativePath);  // Changed from appUserId

            return result;
        }

        [HttpPost("getall-paginated")]
        public async Task<CustomApiResponse> GetPaged([FromBody] HRMEmployeePaginationParams parameters)
        {
            var response = new CustomApiResponse();
            try
            {
                var pagedResult = await _service.GetPagedAsync(parameters);
                response.IsSucess = true;
                response.Value = pagedResult;
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
    }
}