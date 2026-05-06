using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class ContributionMasterRepository : IContributionMasterRepository
    {
        private readonly AppDbContext _context;

        public ContributionMasterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContributionMaster>> GetAllAsync()
        {
            return await _context.ContributionMasters
                .Where(m => m.ContributionStatus.Trim().ToUpper() == "FORWARD")
                .OrderByDescending(m => m.Year)
                .ThenByDescending(m => m.Month)
                .ToListAsync();
        }

        public async Task<ContributionMaster?> GetByIdAsync(long masterId)
        {
            return await _context.ContributionMasters
                .FirstOrDefaultAsync(m => m.ContributionMasterId == masterId);
        }

        public async Task AddAsync(ContributionMaster master)
        {
            await _context.ContributionMasters.AddAsync(master);
        }

        public void Update(ContributionMaster master)
        {
            _context.ContributionMasters.Update(master);
        }

        public void Delete(ContributionMaster master)
        {
            _context.ContributionMasters.Remove(master);
        }

        public async Task<List<AccountReadyDto>> GetDetailsWithLookupsAsync(long masterId)
        {
            var rawDetails = await (
                from detail in _context.ContributionDetails
                join member in _context.Members
                    on detail.StaffNo equals member.StaffNo.ToString()
                join branch in _context.Branches
                    on detail.DpCode equals branch.DpCode.ToString()
                join circle in _context.Circles
                    on detail.Circle equals circle.CircleCode
                where detail.ContributionMasterId == masterId
                      && !detail.isParked
                      && !member.IsDeleted
                      && !branch.IsDeleted
                      && !circle.IsDeleted
                select new
                {
                    detail.ContributionDetailId,
                    detail.ContributionMasterId,
                    member.MemberId,
                    branch.BranchId,
                    circle.CircleId,
                    detail.Month,
                    detail.Year,
                    detail.Amount
                }
            ).ToListAsync();

            return rawDetails.Select(d => new AccountReadyDto
            {
                ContributionDetailId = d.ContributionDetailId,
                ContributionMasterId = d.ContributionMasterId,
                MemberId = d.MemberId,
                BranchId = d.BranchId,
                CircleId = d.CircleId,
                MonthCode = int.Parse(d.Month),
                YearOf = int.Parse(d.Year),
                Amount = d.Amount
            }).ToList();
        }

        public async Task AddAccountsRangeAsync(List<Accounts> accounts)
        {
            await _context.Accounts.AddRangeAsync(accounts);
        }
        public async Task<int> AutoParkInvalidDetailsAsync(long masterId)
        {
            var now = DateTime.Now;

            // Load only the 3 lookup sets — small tables, fast
            var validStaffNos = await _context.Members
                .Where(m => !m.IsDeleted)
                .Select(m => m.StaffNo.ToString())
                .ToHashSetAsync();

            var validDpCodes = await _context.Branches
                .Where(b => !b.IsDeleted)
                .Select(b => b.DpCode.ToString())
                .ToHashSetAsync();

            var validCircleCodes = await _context.Circles
                .Where(c => !c.IsDeleted)
                .Select(c => c.CircleCode)
                .ToHashSetAsync();

            // Pull ONLY invalid rows — not all 100k
            var invalidDetails = await _context.ContributionDetails
                .Where(d => d.ContributionMasterId == masterId
                         && !d.isParked
                         && (
                             !_context.Members.Any(m => !m.IsDeleted && m.StaffNo.ToString() == d.StaffNo.Trim())
                             || !_context.Branches.Any(b => !b.IsDeleted && b.DpCode.ToString() == d.DpCode.Trim())
                             || !_context.Circles.Any(c => !c.IsDeleted && c.CircleCode == d.Circle)
                         ))
                .ToListAsync();

            if (!invalidDetails.Any()) return 0;

            // Update in memory — instant
            foreach (var detail in invalidDetails)
            {
                var reasons = new List<string>();

                if (!validStaffNos.Contains(detail.StaffNo?.Trim()))
                    reasons.Add("New member not in system");

                if (!validDpCodes.Contains(detail.DpCode?.Trim()))
                    reasons.Add("Wrong branch / DpCode not found");

                if (!validCircleCodes.Contains(detail.Circle))
                    reasons.Add("Wrong circle / CircleCode not found");

                if (reasons.Any())
                {
                    detail.isParked = true;
                    detail.ParkReason = string.Join("; ", reasons);
                    detail.Parkedon = now;
                }
            }

            // ✅ Single SaveChanges — no batching loop needed
            // EF Core tracks all changes, commits in one transaction
            _context.ContributionDetails.UpdateRange(invalidDetails);
            await _context.SaveChangesAsync();

            return invalidDetails.Count;
        }

        public async Task<bool> ContributionExistsForMonthYearAsync(string month, string year)
        {
            return await _context.ContributionMasters
                .AnyAsync(m => m.Month == month && m.Year == year);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}