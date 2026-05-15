using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberAccountsDetailsController : ControllerBase
    {
        private readonly IMemberAccountDetailsService _service;
        public MemberAccountsDetailsController(IMemberAccountDetailsService service)
        {
            _service = service;
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
    }
}
