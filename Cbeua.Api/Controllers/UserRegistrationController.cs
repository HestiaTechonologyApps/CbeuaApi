using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IUserRegistrationService _service;

        public UserRegistrationController(IUserRegistrationService service)
        {
            _service = service;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetAllPending()
        {
            var result = await _service.GetAllPendingAsync();
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromQuery] bool approve, [FromQuery] int currentUserId, [FromQuery] string? rejectReason = null)
        {
            var result = await _service.ApproveAsync(id, currentUserId, approve, rejectReason);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
    }
}