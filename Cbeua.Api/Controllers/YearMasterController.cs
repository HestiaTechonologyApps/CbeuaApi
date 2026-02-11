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
    public class YearMasterController : ControllerBase
    {
        private readonly IYearMasterService _service;

        public YearMasterController(IYearMasterService service)
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
                var yearMasters = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = yearMasters;
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
                var yearMaster = await _service.GetByIdAsync(id);
                if (yearMaster == null)
                {
                    response.IsSucess = false;
                    response.Error = "Year not found or has been deleted";
                    response.StatusCode = 404;
                }
                else
                {
                    response.IsSucess = true;
                    response.Value = yearMaster;
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
        public async Task<CustomApiResponse> Create([FromBody] YearMaster yearMaster)
        {
            var response = new CustomApiResponse();
            try
            {
                var created = await _service.CreateAsync(yearMaster);
                response.IsSucess = true;
                response.Value = created;
                response.StatusCode = 201;
            }
            catch (InvalidOperationException ex) // ✅ CATCH DUPLICATE/VALIDATION ERRORS
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 409; // Conflict
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
        public async Task<CustomApiResponse> Update(int id, [FromBody] YearMaster yearMaster)
        {
            var response = new CustomApiResponse();
            try
            {
                if (id != yearMaster.YearOf)
                {
                    response.IsSucess = false;
                    response.Error = "Id mismatch";
                    response.StatusCode = 400;
                    return response;
                }

                var updated = await _service.UpdateAsync(yearMaster);
                if (!updated)
                {
                    response.IsSucess = false;
                    response.Error = "Year not found or has been deleted";
                    response.StatusCode = 404;
                }
                else
                {
                    response.IsSucess = true;
                    response.Value = yearMaster;
                    response.StatusCode = 200;
                }
            }
            catch (InvalidOperationException ex) // ✅ CATCH DUPLICATE/VALIDATION ERRORS
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 409; // Conflict
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<CustomApiResponse> Delete(int id)
        {
            var response = new CustomApiResponse();
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    response.IsSucess = false;
                    response.Error = "Year not found or already deleted";
                    response.StatusCode = 404;
                }
                else
                {
                    response.IsSucess = true;
                    response.Value = "Year deleted successfully";
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
    }
}