using System.Collections.Generic;

namespace Cbeua.Domain.DTO
{
    public class DashboardOverviewDto
    {
        public int TotalMembers { get; set; }
        public double TotalMembersGrowth { get; set; }   

        public int ActiveContributions { get; set; }
        public double ActiveContributionsGrowth { get; set; }

        public int TotalClaims { get; set; }
        public double TotalClaimsGrowth { get; set; }

        public decimal CollectionLakhs { get; set; }       
        public double CollectionGrowth { get; set; }
    }
    public class MonthlyContributionVsClaimDto
    {
        public string Month { get; set; } = "";
        public decimal Contributions { get; set; }
        public decimal Claims { get; set; }
    }

    public class ClaimTypeDistributionDto
    {
        public decimal DeathClaims { get; set; }
        public decimal MedicalClaims { get; set; }
        public decimal RefundClaims { get; set; }
        public decimal Others { get; set; }
    }
    public class StateWiseMembershipDto
    {
        public string StateName { get; set; } = "";
        public int MemberCount { get; set; }
    }
    public class TopPerformingStateDto
    {
        public string Abbreviation { get; set; } = "";
        public string StateName { get; set; } = "";
        public double PerformancePercent { get; set; }
    }

    public class RecentActivityDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string TimeAgo { get; set; } = "";
        public string Type { get; set; } = "";           
    }

    public class MonthlyFinancialComparisonDto
    {
        public string Month { get; set; } = "";
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
    public class ContributionTrendDto
    {
        public string Month { get; set; } = "";
        public decimal Amount { get; set; }
    }
    public class DashboardResponseDto
    {
        public DashboardOverviewDto Overview { get; set; } = new();
        public List<MonthlyContributionVsClaimDto> MonthlyContributionVsClaims { get; set; } = new();
        public ClaimTypeDistributionDto ClaimTypeDistribution { get; set; } = new();
        public List<StateWiseMembershipDto> StateWiseMembership { get; set; } = new();
        public List<TopPerformingStateDto> TopPerformingStates { get; set; } = new();
        public List<RecentActivityDto> RecentActivities { get; set; } = new();
        public List<MonthlyFinancialComparisonDto> MonthlyFinancialComparison { get; set; } = new();
        public List<ContributionTrendDto> ContributionTrends { get; set; } = new();
    }
}