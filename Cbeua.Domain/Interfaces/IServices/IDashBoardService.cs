using Cbeua.Domain.DTO;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetDashboardAsync(int year);
        Task<DashboardOverviewDto> GetOverviewAsync(int year);
        Task<ClaimsSettledStatsDTO> GetClaimsSettledStatsAsync();
        Task<List<MonthlyContributionVsClaimDto>> GetMonthlyContributionVsClaimsAsync(int year);
        Task<ClaimTypeDistributionDto> GetClaimTypeDistributionAsync(int year);
        Task<List<StateWiseMembershipDto>> GetStateWiseMembershipAsync();
        Task<List<TopPerformingStateDto>> GetTopPerformingStatesAsync(int year);
        Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int count = 10);
        Task<List<MonthlyFinancialComparisonDto>> GetMonthlyFinancialComparisonAsync(int year);
        Task<List<ContributionTrendDto>> GetContributionTrendsAsync(int year);
    }
}