using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MonthlyContributionController : ControllerBase
    {
        private readonly IMonthlyContributionService _service;

        public MonthlyContributionController(IMonthlyContributionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var contributions = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = contributions;
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
        public async Task<CustomApiResponse> GetById(long id)
        {
            var response = new CustomApiResponse();
            var contribution = await _service.GetByIdAsync(id);
            if (contribution == null)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = contribution;
                response.StatusCode = 200;
            }
            return response;
        }

        [HttpPost]
        public async Task<CustomApiResponse> Create([FromBody] MonthlyContribution monthlyContribution)
        {
            var response = new CustomApiResponse();
            try
            {
                var created = await _service.CreateAsync(monthlyContribution);
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
        public async Task<CustomApiResponse> Update(long id, [FromBody] MonthlyContribution monthlyContribution)
        {
            var response = new CustomApiResponse();
            if (id != monthlyContribution.MonthlyContributionId)
            {
                response.IsSucess = false;
                response.Error = "Id mismatch";
                response.StatusCode = 400;
                return response;
            }
            var updated = await _service.UpdateAsync(monthlyContribution);
            if (!updated)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = monthlyContribution;
                response.StatusCode = 200;
            }
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<CustomApiResponse> Delete(long id)
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

        /// <summary>
        /// Uploads the Monthly Contribution File
        /// </summary>
        [HttpPost("upload-file")]
        [Consumes("multipart/form-data")]
        public async Task<CustomApiResponse> UploadFile([FromForm] MonthlyContributionFileUploadDto dto)
        {
            var monthCode = dto.MonthCode;
            var yearOf = dto.YearOf;
            var file = dto.ContributionFile;

            if (file == null || file.Length == 0)
                return new CustomApiResponse { IsSucess = false, Error = "No file uploaded", StatusCode = 400 };

            // Check file size (max 10MB for contribution files)
            const long maxFileSize = 10 * 1024 * 1024;
            if (file.Length > maxFileSize)
                return new CustomApiResponse { IsSucess = false, Error = "File size exceeds 10MB", StatusCode = 400 };

            // Check file type (allow text files, csv, excel)
            var allowedContentTypes = new[] { "text/plain", "text/csv", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
            if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
                return new CustomApiResponse { IsSucess = false, Error = "Only text, CSV, and Excel files are allowed", StatusCode = 400 };

            // Prepare file path
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "contributionfiles");
            Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"Contribution_{yearOf}_{monthCode}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save new file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Get file info
            var fileInfo = new FileInfo(filePath);
            var fileSize = fileInfo.Length;

            // Save to DB
            var result = await _service.UploadContributionFileAsync(
                monthCode,
                yearOf,
                fileName,
                filePath,
                "Contribution",
                fileExtension,
                fileSize
            );

            return result;
        }
    }
}