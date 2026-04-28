using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cbeua.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard([FromQuery] int? year)
        {
            try
            {
                int selectedYear = year ?? DateTime.Now.Year;
                var result = await _dashboardService.GetDashboardAsync(selectedYear);
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetDashboard error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load dashboard" });
            }
        }
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview([FromQuery] int? year)
        {
            try
            {
                int selectedYear = year ?? DateTime.Now.Year;
                var result = await _dashboardService.GetOverviewAsync(selectedYear);
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetOverview error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load overview" });
            }
        }
        [HttpGet("monthly-contributions-vs-claims")]
        public async Task<IActionResult> GetMonthlyContributionVsClaims([FromQuery] int? year)
        {
            try
            {
                int selectedYear = year ?? DateTime.Now.Year;
                var result = await _dashboardService.GetMonthlyContributionVsClaimsAsync(selectedYear);
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetMonthlyContributionVsClaims error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load chart data" });
            }
        }

        [HttpGet("claim-type-distribution")]
        public async Task<IActionResult> GetClaimTypeDistribution([FromQuery] int? year)
        {
            try
            {
                int selectedYear = year ?? DateTime.Now.Year;
                var result = await _dashboardService.GetClaimTypeDistributionAsync(selectedYear);
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetClaimTypeDistribution error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load distribution" });
            }
        }

        [HttpGet("state-wise-membership")]
        public async Task<IActionResult> GetStateWiseMembership()
        {
            try
            {
                var result = await _dashboardService.GetStateWiseMembershipAsync();
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetStateWiseMembership error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load state data" });
            }
        }
        [HttpGet("top-performing-states")]
        public async Task<IActionResult> GetTopPerformingStates([FromQuery] int? year)
        {
            try
            {
                int selectedYear = year ?? DateTime.Now.Year;
                var result = await _dashboardService.GetTopPerformingStatesAsync(selectedYear);
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetTopPerformingStates error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load state rankings" });
            }
        }
        [HttpGet("recent-activities")]
        public async Task<IActionResult> GetRecentActivities([FromQuery] int count = 10)
        {
            try
            {
                var result = await _dashboardService.GetRecentActivitiesAsync(count);
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetRecentActivities error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load activities" });
            }
        }

        [HttpGet("monthly-financial-comparison")]
        public async Task<IActionResult> GetMonthlyFinancialComparison([FromQuery] int? year)
        {
            try
            {
                int selectedYear = year ?? DateTime.Now.Year;
                var result = await _dashboardService.GetMonthlyFinancialComparisonAsync(selectedYear);
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetMonthlyFinancialComparison error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load comparison data" });
            }
        }

        [HttpGet("contribution-trends")]
        public async Task<IActionResult> GetContributionTrends([FromQuery] int? year)
        {
            try
            {
                int selectedYear = year ?? DateTime.Now.Year;
                var result = await _dashboardService.GetContributionTrendsAsync(selectedYear);
                return Ok(new { isSuccess = true, value = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Dashboard] GetContributionTrends error");
                return StatusCode(500, new { isSuccess = false, message = "Failed to load trends" });
            }
        }
    }
}