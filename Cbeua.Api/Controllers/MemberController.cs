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
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _service;
        public MemberController(IMemberService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var members = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = members;
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
            var member = await _service.GetByIdAsync(id);
            if (member == null)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = member;
                response.StatusCode = 200;
            }
            return response;
        }




        [HttpPost]
        public async Task<CustomApiResponse> Create([FromBody] Member member)
        {
            var response = new CustomApiResponse();
            try
            {
                var created = await _service.CreateAsync(member);
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
        public async Task<CustomApiResponse> Update(int id, [FromBody] Member member)
        {
            var response = new CustomApiResponse();

            if (id != member.MemberId)
            {
                response.IsSucess = false;
                response.Error = "Id mismatch";
                response.StatusCode = 400;
                return response;
            }

            var updated = await _service.UpdateAsync(member);
            if (!updated)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = member;
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
