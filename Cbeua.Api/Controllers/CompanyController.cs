using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cbeua.Domain.DTO; // For CustomApiResponse
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using CbeuaAPI.Controllers;

namespace Cbeua.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class CompanyController : Api_BaseController
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpGet("admin-getall-company")]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var companys = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = companys;
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

        /// <summary>
        /// Give the list of company and their ID for look up purpose
        /// </summary>
        /// <returns></returns>
        [HttpGet("admin-lookUp-company")]
        public async Task<CustomApiResponse> GetCompanyLookUp()
        {
            var response = new CustomApiResponse();
            try
            {
                var companys = await _service.GetAllAsync();

                var lookup = new List<LookUpDTO>();
                foreach (var company in companys)
                {
                    lookup.Add(new LookUpDTO { Id = company.CompanyId, Text = company.ComapanyName });
                }
                response.IsSucess = true;
                response.Value = lookup;
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
            var company = await _service.GetByIdAsync(id);
            if (company == null)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = company;
                response.StatusCode = 200;
            }
            return response;
        }

        [HttpPost]
        public async Task<CustomApiResponse> Create([FromBody] Company company)
        {
            var response = new CustomApiResponse();
            try
            {
                var created = await _service.CreateAsync(company);
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
        public async Task<CustomApiResponse> Update(int id, [FromBody] Company company)
        {
            var response = new CustomApiResponse();
            if (id != company.CompanyId)
            {
                response.IsSucess = false;
                response.Error = "Id mismatch";
                response.StatusCode = 400;
                return response;
            }
            var updated = await _service.UpdateAsync(company);
            if (!updated)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = company;
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

        /// <summary>
        /// Uploads the Company Logo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("upload-company-logo")]
        [Consumes("multipart/form-data")]
        public async Task<CustomApiResponse> UploadCompanyLogo([FromForm] CompanyLogoUploadDto dto)
        {
            var companyId = dto.CompanyId;
            var companyLogo = dto.CompanyLogo;

            if (companyLogo == null || companyLogo.Length == 0)
                return new CustomApiResponse { IsSucess = false, Error = "No file uploaded", StatusCode = 400 };

            // Check file size (max 2MB)
            const long maxFileSize = 2 * 1024 * 1024;
            if (companyLogo.Length > maxFileSize)
                return new CustomApiResponse { IsSucess = false, Error = "File size exceeds 2MB", StatusCode = 400 };

            // Check file type (allow only images)
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedContentTypes.Contains(companyLogo.ContentType.ToLower()))
                return new CustomApiResponse { IsSucess = false, Error = "Only image files (jpg, png, gif, webp) are allowed", StatusCode = 400 };

            // Get company to check for old logo
            var company = await _service.GetByIdAsync(companyId);
            if (company == null)
                return new CustomApiResponse { IsSucess = false, Error = "Company not found", StatusCode = 404 };

            // Prepare file path
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "companylogos");
            Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(companyLogo.FileName);
            var fileName = $"{companyId}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Delete old company logo if exists and is not empty
            if (!string.IsNullOrEmpty(company.CompanyLogo))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", company.CompanyLogo.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(oldFilePath))
                {
                    try { System.IO.File.Delete(oldFilePath); } catch { /* ignore file delete errors */ }
                }
            }

            // Save new file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await companyLogo.CopyToAsync(stream);
            }

            // Save relative path to DB
            var relativePath = $"/companylogos/{fileName}";
            var result = await _service.UpdateCompanyLogoAsync(companyId, relativePath);

            return result;
        }
    }
}