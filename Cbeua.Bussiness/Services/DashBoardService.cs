using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;

namespace Cbeua.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<DashboardResponseDto> GetDashboardAsync(int year)
        {
            return new DashboardResponseDto
            {
                Overview = await GetOverviewAsync(year),
                MonthlyContributionVsClaims = await GetMonthlyContributionVsClaimsAsync(year),
                ClaimTypeDistribution = await GetClaimTypeDistributionAsync(year),
                StateWiseMembership = await GetStateWiseMembershipAsync(),
                TopPerformingStates = await GetTopPerformingStatesAsync(year),
                RecentActivities = await GetRecentActivitiesAsync(),
                MonthlyFinancialComparison = await GetMonthlyFinancialComparisonAsync(year),
                ContributionTrends = await GetContributionTrendsAsync(year),
            };
        }

        public async Task<DashboardOverviewDto> GetOverviewAsync(int year)
        {
            int prevYear = year - 1;
            int totalMembers = await _repository.GetTotalMembersAsync();
            int prevMembers = await _repository.GetMembersUpToYearAsync(prevYear);
            int activeContribs = await _repository.GetActiveContributionsAsync(year);
            int prevContribs = await _repository.GetActiveContributionsAsync(prevYear);
            int totalClaims = await _repository.GetTotalClaimsAsync(year);
            int prevClaims = await _repository.GetTotalClaimsAsync(prevYear);
            decimal collection = await _repository.GetTotalCollectionAsync(year);
            decimal prevCollection = await _repository.GetTotalCollectionAsync(prevYear);

            return new DashboardOverviewDto
            {
                TotalMembers = totalMembers,
                TotalMembersGrowth = GrowthPercent(prevMembers, totalMembers),
                ActiveContributions = activeContribs,
                ActiveContributionsGrowth = GrowthPercent(prevContribs, activeContribs),
                TotalClaims = totalClaims,
                TotalClaimsGrowth = GrowthPercent(prevClaims, totalClaims),
                CollectionLakhs = Math.Round(collection / 100000, 2),
                CollectionGrowth = GrowthPercent((double)prevCollection, (double)collection),
            };
        }

        public Task<List<MonthlyContributionVsClaimDto>> GetMonthlyContributionVsClaimsAsync(int year)
            => _repository.GetMonthlyContributionVsClaimsAsync(year);

        public Task<ClaimTypeDistributionDto> GetClaimTypeDistributionAsync(int year)
            => _repository.GetClaimTypeDistributionAsync(year);

        public Task<List<StateWiseMembershipDto>> GetStateWiseMembershipAsync()
            => _repository.GetStateWiseMembershipAsync();
        public Task<ClaimsSettledStatsDTO> GetClaimsSettledStatsAsync()
    => _repository.GetClaimsSettledStatsAsync();
        public Task<List<TopPerformingStateDto>> GetTopPerformingStatesAsync(int year)
            => _repository.GetTopPerformingStatesAsync(year);

        public Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int count = 10)
            => _repository.GetRecentActivitiesAsync(count);

        public Task<List<MonthlyFinancialComparisonDto>> GetMonthlyFinancialComparisonAsync(int year)
            => _repository.GetMonthlyFinancialComparisonAsync(year);

        public Task<List<ContributionTrendDto>> GetContributionTrendsAsync(int year)
            => _repository.GetContributionTrendsAsync(year);

        private static double GrowthPercent(double prev, double current)
        {
            if (prev == 0) return current > 0 ? 100 : 0;
            return Math.Round((current - prev) / prev * 100, 1);
        }
    }
}