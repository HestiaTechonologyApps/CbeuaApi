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
    public class ContributionMasterController : ControllerBase
    {
        private readonly IContributionMasterService _service;

        public ContributionMasterController(IContributionMasterService service)
        {
            _service = service;
        }


        [HttpGet("getall-forwarded-contributions")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllContributionMastersAsync();
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

      
        [HttpGet("{masterId}")]
        public async Task<IActionResult> GetById(long masterId)
        {
            var result = await _service.GetByIdAsync(masterId);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

     
       

        [HttpDelete("{masterId}")]
        public async Task<IActionResult> Delete(long masterId)
        {
            var result = await _service.DeleteAsync(masterId);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        
        [HttpPost("{masterId}/forward")]
        public async Task<IActionResult> Forward(long masterId)
        {
            var result = await _service.ForwardAsync(masterId);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }


        [HttpGet("{masterId}/parked")]
        public async Task<IActionResult> GetParked(long masterId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("pageNumber and pageSize must be greater than 0");

            var result = await _service.GetParkedDetailsAsync(masterId, pageNumber, pageSize);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
        [HttpPost("{masterId}/approve")]
        public async Task<IActionResult> Approve(long masterId, [FromQuery] bool approve, [FromQuery] int currentUserId)
        {
            var result = await _service.ApproveAsync(masterId, currentUserId, approve);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
    }
}