using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseMasterController : ControllerBase
    {
        private readonly IExpenseMasterService _service;
        public ExpenseMasterController(IExpenseMasterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var expenseMasters = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = expenseMasters;
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
            var expenseMaster = await _service.GetByIdAsync(id);
            if (expenseMaster == null)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = expenseMaster;
                response.StatusCode = 200;
            }
            return response;
        }

        [HttpPost]
        public async Task<CustomApiResponse> Create([FromBody] ExpenseMaster expenseMaster)
        {
            var response = new CustomApiResponse();
            try
            {
                var created = await _service.CreateAsync(expenseMaster);
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
        public async Task<CustomApiResponse> Update(int id, [FromBody] ExpenseMaster expenseMaster)
        {
            var response = new CustomApiResponse();

            if (id != expenseMaster.ExpenseMasterId)
            {
                response.IsSucess = false;
                response.Error = "Id mismatch";
                response.StatusCode = 400;
                return response;
            }

            var updated = await _service.UpdateAsync(expenseMaster);
            if (!updated)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = expenseMaster;
                response.StatusCode = 200;
            }
            return response;
        }
        [HttpPut("approve/{id}")]
        public async Task<CustomApiResponse> Approve(int id, [FromQuery] bool approve, [FromQuery] int currentUserId)
        {
            return await _service.ApproveAsync(id, currentUserId, approve);
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

        [HttpGet("paged")]
        public async Task<CustomApiResponse> GetPagedExpenseMasters([FromQuery] ExpenseMasterPaginationParams parameters)
        {
            var response = new CustomApiResponse();
            try
            {
                var pagedResult = await _service.GetPagedExpenseMastersAsync(parameters);
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
