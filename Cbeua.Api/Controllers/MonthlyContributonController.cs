using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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

        // GET: api/MonthlyContribution
        [HttpGet]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                response.IsSucess = true;
                response.Value = await _service.GetAllAsync();
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

        // GET: api/MonthlyContribution/5
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

        // PUT: api/MonthlyContribution/5
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
                response.Error = "Not found or already deleted";
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

        // DELETE: api/MonthlyContribution/5
        [HttpDelete("{id}")]
        public async Task<CustomApiResponse> Delete(long id)
        {
            var response = new CustomApiResponse();
            try
            {
                var result = await _service.DeleteWithContributionDataAsync(id);
                if (!result.IsSucess)
                {
                    response.IsSucess = false;
                    response.Error = result.Error;
                    response.StatusCode = result.StatusCode;
                }
                else
                {
                    response.IsSucess = true;
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

        /// <summary>
        /// Single-shot endpoint: upload file, parse it, save master + details — mirrors the old POST behaviour.
        /// </summary>
        [HttpPost("upload-and-save")]
        [Consumes("multipart/form-data")]
        public async Task<CustomApiResponse> UploadAndSave([FromForm] MonthlyContributionFileUploadDto dto)
        {
            if (dto.ContributionFile == null || dto.ContributionFile.Length == 0)
                return new CustomApiResponse { IsSucess = false, Error = "No file uploaded", StatusCode = 400 };

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "contributionfiles");
            Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(dto.ContributionFile.FileName);
            var fileName = $"Contribution_{dto.YearOf}_{dto.MonthCode}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await dto.ContributionFile.CopyToAsync(stream);

                var fileSize = new FileInfo(filePath).Length;

                return await _service.UploadAndSaveAsync(
                    dto.MonthCode,
                    dto.YearOf,
                    fileName,
                    filePath,
                    "Contribution",
                    fileExtension,
                    fileSize
                );
            }
            catch (Exception ex)
            {
                // Clean up orphaned file on failure
                try { System.IO.File.Delete(filePath); } catch { }

                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = ex.Message,
                    StatusCode = 500
                };
            }
        }
       
        [HttpGet("{id}/details")]
        public async Task<CustomApiResponse> GetContributionDetails(
            long id,
            [FromQuery] ContributionDetailPaginationParams p)
        {
            try
            {
                var result = await _service.GetPagedContributionDetailsAsync(id, p);
                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = result
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = ex.Message,
                    StatusCode = 500
                };
            }
        }
        [HttpGet("ContributionMasters")]
        public async Task<IActionResult> GetContributionAll()
        {
            try
            {
                var result = await _service.GetAllContributionMastersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CustomApiResponse
                {
                    IsSucess = false,
                    Error = ex.Message,
                    StatusCode = 500
                });
            }
        }

     
   
        [HttpGet("{id}/report")]
        public async Task<IActionResult> GetReport(
     long id,
     [FromQuery] string type,
     [FromQuery] int pageNumber,
     [FromQuery] int pageSize)
        {
            var result = await _service.GetContributionReportAsync(id, type, pageNumber, pageSize);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

     
    }
}