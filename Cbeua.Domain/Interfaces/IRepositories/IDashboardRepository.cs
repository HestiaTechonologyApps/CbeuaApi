using Cbeua.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalMembersAsync();
        Task<int> GetMembersUpToYearAsync(int year);
        Task<int> GetActiveContributionsAsync(int year);
        Task<int> GetActiveContributionsAsync(int year, bool previous);
        Task<int> GetTotalClaimsAsync(int year);
        Task<decimal> GetTotalCollectionAsync(int year);

        Task<List<MonthlyContributionVsClaimDto>> GetMonthlyContributionVsClaimsAsync(int year);
        Task<ClaimTypeDistributionDto> GetClaimTypeDistributionAsync(int year);
        Task<List<StateWiseMembershipDto>> GetStateWiseMembershipAsync();
        Task<List<TopPerformingStateDto>> GetTopPerformingStatesAsync(int year);
        Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int count);
        Task<List<MonthlyFinancialComparisonDto>> GetMonthlyFinancialComparisonAsync(int year);
        Task<List<ContributionTrendDto>> GetContributionTrendsAsync(int year);
    }
}