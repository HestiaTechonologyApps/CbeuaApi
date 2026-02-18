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
    public class HRMJobController : ControllerBase
    {
        private readonly IHRMJobService _service;
        public HRMJobController(IHRMJobService service)
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
        public async Task<CustomApiResponse> Create([FromBody] HRMJobCreateUpdateDTO entitydto)
        {
            return await _service.CreateAsync(entitydto);
        }

        [HttpPut("{id}")]
        public async Task<CustomApiResponse> Update(int id, [FromBody] HRMJobCreateUpdateDTO entitydto)
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

        [HttpPost("getall-paginated")]
        public async Task<CustomApiResponse> GetPaged([FromBody] HRMJobPaginationParams parameters)
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