using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class ContributionDetailRepository : IContributionDetailRepository
    {
        private readonly AppDbContext _context;

        public ContributionDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ContributionDetail?> GetByIdAsync(long detailId)
        {
            return await _context.ContributionDetails
                .FirstOrDefaultAsync(d => d.ContributionDetailId == detailId);
        }

        public void Update(ContributionDetail detail)
        {
            _context.ContributionDetails.Update(detail);
        }

        public async Task<bool> AccountsEntryExistsAsync(int memberId, int year, int month)
        {
            return await _context.Accounts
                .AnyAsync(a => a.MemeberId == memberId
                            && a.YearOf == year
                            && a.MonthCode == month);
        }

        public async Task AddAccountAsync(Accounts account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public async Task<int?> GetCircleIdByCircleCodeAsync(int circleCode)
        {
            return await _context.Circles
                .Where(c => c.CircleCode == circleCode && !c.IsDeleted)
                .Select(c => (int?)c.CircleId)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetBranchIdByDpCodeAsync(int dpCode)
        {
            return await _context.Branches
                .Where(b => b.DpCode == dpCode && !b.IsDeleted)
                .Select(b => (int?)b.BranchId)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetMemberIdByStaffNoAsync(int staffNo)
        {
            return await _context.Members
                .Where(m => m.StaffNo == staffNo && !m.IsDeleted)
                .Select(m => (int?)m.MemberId)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetMasterStatusAsync(long masterId)
        {
            return await _context.ContributionMasters
                .AsNoTracking()
                .Where(m => m.ContributionMasterId == masterId)
                .Select(m => m.ContributionStatus)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<string, List<long>>> GetUnparkedDiscrepancyIdsAsync(long masterId)
        {
            // Same rules as the NEWMEMBERS / WRONGBRANCH / WRONGCIRCLE reports,
            // restricted to rows that are not parked.
            var memberNos = (await _context.Members.AsNoTracking()
                    .Where(m => !m.IsDeleted).Select(m => m.StaffNo).ToListAsync()).ToHashSet();
            var dpCodes = (await _context.Branches.AsNoTracking()
                    .Where(b => !b.IsDeleted).Select(b => b.DpCode).ToListAsync()).ToHashSet();
            var circleCodes = (await _context.Circles.AsNoTracking()
                    .Where(c => !c.IsDeleted).Select(c => c.CircleCode).ToListAsync()).ToHashSet();

            var rows = await _context.ContributionDetails
                .AsNoTracking()
                .Where(d => d.ContributionMasterId == masterId && !d.isParked)
                .Select(d => new { d.ContributionDetailId, d.StaffNo, d.DpCode, d.Circle })
                .ToListAsync();

            var result = new Dictionary<string, List<long>>
            {
                ["NEWMEMBERS"] = new(),
                ["WRONGBRANCH"] = new(),
                ["WRONGCIRCLE"] = new()
            };

            foreach (var r in rows)
            {
                if (!int.TryParse(r.StaffNo?.Trim(), out var staffNo) || !memberNos.Contains(staffNo))
                    result["NEWMEMBERS"].Add(r.ContributionDetailId);
                if (!int.TryParse(r.DpCode?.Trim(), out var dp) || !dpCodes.Contains(dp))
                    result["WRONGBRANCH"].Add(r.ContributionDetailId);
                if (!circleCodes.Contains(r.Circle))
                    result["WRONGCIRCLE"].Add(r.ContributionDetailId);
            }

            return result;
        }

        public async Task<int> CountParkedAsync(long masterId)
        {
            return await _context.ContributionDetails
                .CountAsync(d => d.ContributionMasterId == masterId && d.isParked);
        }

        public async Task<int> ParkDetailsAsync(long masterId, IEnumerable<long> detailIds, string parkReason)
        {
            var now = DateTime.Now;
            int parked = 0;

            // Chunked so the IN list stays small; one set-based UPDATE per chunk.
            foreach (var chunk in detailIds.Distinct().Chunk(1000))
            {
                var ids = chunk.ToList();
                parked += await _context.ContributionDetails
                    .Where(d => d.ContributionMasterId == masterId
                             && !d.isParked
                             && ids.Contains(d.ContributionDetailId))
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(d => d.isParked, true)
                        .SetProperty(d => d.ParkReason, parkReason)
                        .SetProperty(d => d.Parkedon, (DateTime?)now));
            }

            return parked;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}