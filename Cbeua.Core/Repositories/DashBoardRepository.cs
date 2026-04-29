using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;

namespace Cbeua.Core.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        private static readonly string[] MonthNames =
            { "Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec" };

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<int> GetTotalMembersAsync()
        {
            return await (
                from m in _context.Members
                join c in _context.Categories on m.CategoryId equals c.CategoryId
                join d in _context.Designations on m.DesignationId equals d.DesignationId
                join b in _context.Branches on m.BranchId equals b.BranchId
                join s in _context.statuses on m.StatusId equals s.StatusId
                where !m.IsDeleted
                select m.MemberId
            ).CountAsync();
        }

        public async Task<int> GetMembersUpToYearAsync(int year)
        {
            return await _context.Members
                .CountAsync(m => !m.IsDeleted
                              && m.DojtoScheme.HasValue
                              && m.DojtoScheme.Value.Year <= year);
        }

        public async Task<int> GetActiveContributionsAsync(int year)
        {
            return await _context.ContributionDetails
                .Where(cd => cd.Year == year.ToString())
                .Select(cd => cd.StaffNo)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetActiveContributionsAsync(int year, bool previous)
        {
            int targetYear = previous ? year - 1 : year;
            return await GetActiveContributionsAsync(targetYear);
        }

        public async Task<int> GetTotalClaimsAsync(int year)
        {
            return await (
                from dc in _context.DeathClaims
                join y in _context.YearMasters on dc.YearOF equals y.YearOf
                where !dc.IsDeleted && y.YearName == year
                select dc.DeathClaimId
            ).CountAsync();
        }

        public async Task<decimal> GetTotalCollectionAsync(int year)
        {
            return await _context.ContributionDetails
                .Where(cd => cd.Year == year.ToString())
                .SumAsync(cd => (decimal?)cd.Amount) ?? 0;
        }

        public async Task<List<MonthlyContributionVsClaimDto>> GetMonthlyContributionVsClaimsAsync(int year)
        {
            var contribByMonth = await _context.ContributionDetails
                .Where(cd => cd.Year == year.ToString())
                .GroupBy(cd => cd.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Amount) })
                .ToListAsync();

            var claimsByMonth = await _context.DeathClaims
                .Where(dc => !dc.IsDeleted && dc.YearOF == year && dc.DeathDate.HasValue)
                .GroupBy(dc => dc.DeathDate!.Value.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => (decimal?)x.Amount) ?? 0 })
                .ToListAsync();

            var result = new List<MonthlyContributionVsClaimDto>();
            for (int m = 1; m <= 12; m++)
            {
                string ms = m.ToString();
                result.Add(new MonthlyContributionVsClaimDto
                {
                    Month = MonthNames[m - 1],
                    Contributions = contribByMonth.FirstOrDefault(x => x.Month == ms)?.Total ?? 0,
                    Claims = claimsByMonth.FirstOrDefault(x => x.Month == m)?.Total ?? 0,
                });
            }
            return result;
        }

        public async Task<ClaimTypeDistributionDto> GetClaimTypeDistributionAsync(int year)
        {
          
            decimal deathCount = await _context.DeathClaims
                .CountAsync(dc => !dc.IsDeleted && dc.YearOF == year);

            decimal refundCount = await _context.RefundContributions
                .CountAsync(rc => rc.YearOF == year);

            return new ClaimTypeDistributionDto
            {
                DeathClaims = deathCount,
                MedicalClaims = 0,
                RefundClaims = refundCount,
                Others = 0,
            };
        }

        public async Task<List<StateWiseMembershipDto>> GetStateWiseMembershipAsync()
        {
            return await (
                from m in _context.Members.AsNoTracking()
                where !m.IsDeleted && m.BranchId.HasValue
                join b in _context.Branches.AsNoTracking() on m.BranchId equals b.BranchId
                join c in _context.Circles.AsNoTracking() on b.CircleId equals c.CircleId
                join s in _context.States.AsNoTracking() on c.StateId equals s.StateId
                group m by s.Name into g
                orderby g.Count() descending
                select new StateWiseMembershipDto
                {
                    StateName = g.Key,
                    MemberCount = g.Count()
                }
            ).Take(10).ToListAsync();
        }

        public async Task<List<TopPerformingStateDto>> GetTopPerformingStatesAsync(int year)
        {
            var stateMembers = await (
                from m in _context.Members.AsNoTracking()
                where !m.IsDeleted && m.BranchId.HasValue
                join b in _context.Branches.AsNoTracking() on m.BranchId equals b.BranchId
                join c in _context.Circles.AsNoTracking() on b.CircleId equals c.CircleId
                join s in _context.States.AsNoTracking() on c.StateId equals s.StateId
                group m by new { s.Name, s.Abbreviation } into g
                select new
                {
                    g.Key.Name,
                    g.Key.Abbreviation,
                    TotalMembers = g.Count(),
                    StaffNos = g.Select(x => x.StaffNo.ToString()).ToList()
                }
            ).ToListAsync();

            var activeStaffNos = await _context.ContributionDetails
                .Where(cd => cd.Year == year.ToString())
                .Select(cd => cd.StaffNo)
                .Distinct()
                .ToListAsync();

            var activeSet = activeStaffNos.ToHashSet();

            return stateMembers
                .Select(s => new TopPerformingStateDto
                {
                    StateName = s.Name,
                    Abbreviation = s.Abbreviation ?? s.Name[..2].ToUpper(),
                    PerformancePercent = s.TotalMembers == 0 ? 0
                        : Math.Round(
                            (double)s.StaffNos.Count(sn => activeSet.Contains(sn))
                            / s.TotalMembers * 100, 1)
                })
                .OrderByDescending(x => x.PerformancePercent)
                .Take(5)
                .ToList();
        }

        public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int count)
        {
            var activities = new List<(DateTime Date, RecentActivityDto Activity)>();

            var recentContribs = await _context.ContributionMasters
                .AsNoTracking()
                .OrderByDescending(cm => cm.ContributionMasterId)
                .Take(5)
                .ToListAsync();

            foreach (var cm in recentContribs)
            {
                activities.Add((DateTime.Now, new RecentActivityDto
                {
                    Type = "contribution",
                    Title = "New contribution received",
                    Description = $"Circle {cm.Circle} – ₹{cm.totalamount} ({cm.totalentry} entries)",
                    TimeAgo = "Recently",
                }));
            }

            var recentClaims = await _context.DeathClaims
                .AsNoTracking()
                .OrderByDescending(dc => dc.DeathClaimId)
                .Take(5)
                .ToListAsync();

            foreach (var dc in recentClaims)
            {
                activities.Add((dc.DeathDate ?? DateTime.Now, new RecentActivityDto
                {
                    Type = "claim",
                    Title = "Death claim processed",
                    Description = $"Claim #{dc.DeathClaimId} – ₹{dc.Amount:N0}",
                    TimeAgo = TimeAgo(dc.DeathDate),
                }));
            }

            var recentMembers = await _context.Members
                .AsNoTracking()
                .Where(m => !m.IsDeleted)
                .OrderByDescending(m => m.CreatedDate)
                .Take(5)
                .ToListAsync();

            foreach (var m in recentMembers)
            {
                activities.Add((m.CreatedDate ?? DateTime.MinValue, new RecentActivityDto
                {
                    Type = "member",
                    Title = "New member registered",
                    Description = m.Name,
                    TimeAgo = TimeAgo(m.CreatedDate),
                }));
            }

            return activities
                .OrderByDescending(x => x.Date)
                .Take(count)
                .Select(x => x.Activity)
                .ToList();
        }

        // ── Monthly Financial Comparison ─────────────────────────────────

        public async Task<List<MonthlyFinancialComparisonDto>> GetMonthlyFinancialComparisonAsync(int year)
        {
            var income = await _context.ContributionDetails
                .Where(cd => cd.Year == year.ToString())
                .GroupBy(cd => cd.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Amount) })
                .ToListAsync();

            var expenses = await _context.DeathClaims
                .Where(dc => !dc.IsDeleted && dc.YearOF == year && dc.DeathDate.HasValue)
                .GroupBy(dc => dc.DeathDate!.Value.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => (decimal?)x.Amount) ?? 0 })
                .ToListAsync();

            var result = new List<MonthlyFinancialComparisonDto>();
            for (int m = 1; m <= 12; m++)
            {
                string ms = m.ToString();
                result.Add(new MonthlyFinancialComparisonDto
                {
                    Month = MonthNames[m - 1],
                    Income = income.FirstOrDefault(x => x.Month == ms)?.Total ?? 0,
                    Expense = expenses.FirstOrDefault(x => x.Month == m)?.Total ?? 0,
                });
            }
            return result;
        }

        // ── Contribution Trends ──────────────────────────────────────────

        public async Task<List<ContributionTrendDto>> GetContributionTrendsAsync(int year)
        {
            var byMonth = await _context.ContributionDetails
                .Where(cd => cd.Year == year.ToString())
                .GroupBy(cd => cd.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Amount) })
                .ToListAsync();

            var result = new List<ContributionTrendDto>();
            for (int m = 1; m <= 12; m++)
            {
                string ms = m.ToString();
                result.Add(new ContributionTrendDto
                {
                    Month = MonthNames[m - 1],
                    Amount = byMonth.FirstOrDefault(x => x.Month == ms)?.Total ?? 0,
                });
            }
            return result;
        }

        // ── Helper ───────────────────────────────────────────────────────

        private static string TimeAgo(DateTime? date)
        {
            if (date == null) return "";
            var diff = DateTime.Now - date.Value;
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} minutes ago";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} hours ago";
            if (diff.TotalDays < 7) return $"{(int)diff.TotalDays} day(s) ago";
            return date.Value.ToString("dd MMM yyyy");
        }
    }
}