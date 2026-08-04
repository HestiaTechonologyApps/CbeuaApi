using Cbeua.API.Controllers;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClaimsSettledController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;
        public ClaimsSettledController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }
        [HttpGet("claims-settled-stats")]
       
        public async Task<IActionResult> GetClaimsSettledStats()
        {
            try
            {
                var result = await _dashboardService.GetClaimsSettledStatsAsync();
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetClaimsSettledStats error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load claims stats" });
            }
        }
    }
}
