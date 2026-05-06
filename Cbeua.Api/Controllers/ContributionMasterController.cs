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

        // GET: api/ContributionMaster
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        // GET: api/ContributionMaster/5
        [HttpGet("{masterId}")]
        public async Task<IActionResult> GetById(long masterId)
        {
            var result = await _service.GetByIdAsync(masterId);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        // POST: api/ContributionMaster
       

        // DELETE: api/ContributionMaster/5
        [HttpDelete("{masterId}")]
        public async Task<IActionResult> Delete(long masterId)
        {
            var result = await _service.DeleteAsync(masterId);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        // POST: api/ContributionMaster/5/forward
        [HttpPost("{masterId}/forward")]
        public async Task<IActionResult> Forward(long masterId)
        {
            var result = await _service.ForwardAsync(masterId);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        // POST: api/ContributionMaster/5/approve?approve=true&currentUserId=1
        [HttpPost("{masterId}/approve")]
        public async Task<IActionResult> Approve(long masterId, [FromQuery] bool approve, [FromQuery] int currentUserId)
        {
            var result = await _service.ApproveAsync(masterId, currentUserId, approve);
            return result.IsSucess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
    }
}