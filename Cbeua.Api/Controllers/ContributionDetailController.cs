using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContributionDetailController : ControllerBase
    {
        private readonly IContributionDetailService _service;

        public ContributionDetailController(IContributionDetailService service)
        {
            _service = service;
        }

        [HttpGet("{detailId}")]
        public async Task<IActionResult> GetById(long detailId)
        {
             var result = await _service.GetByIdAsync(detailId);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

      
        [HttpPost("{detailId}/park")]
        public async Task<IActionResult> ParkItem(long detailId, [FromBody] ParkItemDto dto)
        {
            var result = await _service.ParkItemAsync(detailId, dto.ParkReason);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
        [HttpPost("{detailId}/unpark")]
        public async Task<IActionResult> UnParkItem(long detailId, [FromQuery] int currentUserId)
        {
            var result = await _service.UnParkItemAsync(detailId, currentUserId);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
    }
}