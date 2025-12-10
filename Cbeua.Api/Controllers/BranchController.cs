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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;
        public BranchController(IBranchService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var branches = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = branches;
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
            var branch = await _service.GetByIdAsync(id);
            if (branch == null)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = branch;
                response.StatusCode = 200;
            }
            return response;
        }




        [HttpPost]
        public async Task<CustomApiResponse> Create([FromBody] Branch branch)
        {
            var response = new CustomApiResponse();
            try
            {
                var created = await _service.CreateAsync(branch);
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
        public async Task<CustomApiResponse> Update(int id, [FromBody] Branch branch)
        {
            var response = new CustomApiResponse();

            if (id != branch.BranchId)
            {
                response.IsSucess = false;
                response.Error = "Id mismatch";
                response.StatusCode = 400;
                return response;
            }

            var updated = await _service.UpdateAsync(branch);
            if (!updated)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = branch;
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


    }
}
