using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class MonthlyContributionRepository : GenericRepository<MonthlyContribution>, IMonthlyContributionRepository
    {
        private readonly AppDbContext _context;

        public MonthlyContributionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<MonthlyContributionDTO> GetQueryableMonthlyContributions()
        {
            var q = from mc in _context.MonthlyContributions
                    join m in _context.Months on mc.MonthCode equals m.MonthCode
                    join y in _context.YearMasters on mc.YearOf equals y.YearOf
                    where !mc.IsDeleted
                    select new MonthlyContributionDTO
                    {
                        MonthlyContributionId = mc.MonthlyContributionId,
                        FileName = mc.FileName,
                        FileLocation = mc.FileLocation,
                        FileType = mc.FileType,
                        FileExtension = mc.FileExtension,
                        FileSize = mc.FileSize,
                        MonthCode = mc.MonthCode,
                        MonthName = m.MonthName,
                        YearOf = mc.YearOf,
                        YearName = y.YearName,
                        IsDeleted = mc.IsDeleted
                    };
            return q;
        }

        public List<ContributionMaster> GetExistingContributionMasters(string month, string year)
        {
            return _context.ContributionMasters
                .Where(cm => cm.Month == month && cm.Year == year)
                .ToList();
        }

        public async Task AddContributionMasterAsync(ContributionMaster master)
        {
            await _context.ContributionMasters.AddAsync(master);
        }

        public async Task AddContributionDetailAsync(ContributionDetail detail)
        {
            await _context.ContributionDetails.AddAsync(detail);
        }

   
        public async Task AddContributionDetailsRangeAsync(List<ContributionDetail> details)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                await _context.ContributionDetails.AddRangeAsync(details);
            }
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        public List<ContributionDetail> GetContributionDetailsByMasterId(long masterId)
        {
            return _context.ContributionDetails
                .Where(d => d.ContributionMasterId == masterId)
                .ToList();
        }
        public async Task<List<ContributionMasterListDTO>> GetAllContributionMasters()
        {
            var masters = await _context.ContributionMasters
                .AsNoTracking()
                .OrderByDescending(cm => cm.ContributionMasterId)
                .ToListAsync();

            return masters.Select(cm => new ContributionMasterListDTO
            {
                ContributionMasterId = cm.ContributionMasterId,
                FileName = cm.FileName,
                FileLocation = cm.FileLocation,
                FileType = cm.FileType,
                FileExtension = cm.FileExtension,
                FileSize = cm.FileSize,
                Month = cm.Month,
                Year = cm.Year,
                Circle = cm.Circle,
                TotalAmount = cm.totalamount,
                TotalEntry = cm.totalentry,
                ContributionStatus = cm.ContributionStatus,
                NewMemberCount = cm.NewMemberCount,
                ApprovedBy = cm.ApprovedBy,
                ApprovedDate = cm.ApprovedDate,
                IsApproved = cm.isApproved
            }).ToList();
        }
        public int GetContributionDetailsCountByMasterId(long masterId)
        {
            return _context.ContributionDetails
                .Count(d => d.ContributionMasterId == masterId);
        }

        public void RemoveContributionDetails(List<ContributionDetail> details)
        {
            _context.ContributionDetails.RemoveRange(details);
        }

        public void RemoveContributionMaster(ContributionMaster master)
        {
            _context.ContributionMasters.Remove(master);
        }

     
        public void DetachAll()
        {
            var entries = _context.ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }
        public async Task<int> GetNewMemberCountAsync(long contributionMasterId)
        {
            return await _context.ContributionDetails
                .Where(d => d.ContributionMasterId == contributionMasterId
                         && !_context.Members.Any(m => m.StaffNo.ToString() == d.StaffNo))
                .CountAsync();
        }
        public async Task UpdateContributionMasterAsync(ContributionMaster master)
        {
            _context.ContributionMasters.Update(master);
        }
        public async Task<ContributionMaster?> GetContributionMasterByIdAsync(long contributionMasterId)
        {
            return await _context.ContributionMasters
                .FirstOrDefaultAsync(cm => cm.ContributionMasterId == contributionMasterId);
        }
        public IQueryable<ContributionDetail> GetContributionDetailsQueryable(long monthlyContributionId)
        {
            var monthly = _context.MonthlyContributions
                .FirstOrDefault(mc => mc.MonthlyContributionId == monthlyContributionId && !mc.IsDeleted);

            if (monthly == null)
                return Enumerable.Empty<ContributionDetail>().AsQueryable();

            var master = _context.ContributionMasters
                .FirstOrDefault(cm => cm.Month == monthly.MonthCode.ToString()
                                   && cm.Year == monthly.YearOf.ToString());

            if (master == null)
                return Enumerable.Empty<ContributionDetail>().AsQueryable();

            return _context.ContributionDetails
                .Where(d => d.ContributionMasterId == master.ContributionMasterId);
        }
        //reportss
        public async Task<List<ContributionDetail>> GetNewMembersAsync(long contributionMasterId)
        {
            return await _context.ContributionDetails
                .Where(d => d.ContributionMasterId == contributionMasterId
                         && !_context.Members
                             .Where(m => !m.IsDeleted)
                             .Any(m => m.StaffNo.ToString() == d.StaffNo.Trim()))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<ContributionDetail>> GetWrongBranchAsync(long contributionMasterId)
        {
            return await _context.ContributionDetails
                .Where(d => d.ContributionMasterId == contributionMasterId
                         && !_context.Branches
                             .Where(b => !b.IsDeleted)
                             .Any(b => b.DpCode.ToString() == d.DpCode.Trim()))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<ContributionDetail>> GetWrongCircleAsync(long contributionMasterId)
        {
            return await _context.ContributionDetails
                .Where(d => d.ContributionMasterId == contributionMasterId
                         && !_context.Circles
                             .Where(c => !c.IsDeleted)
                             .Any(c => c.CircleCode == d.Circle))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<ContributionDetail>> GetParkedItemsAsync(long contributionMasterId)
        {
            return await _context.ContributionDetails
                .Where(d => d.ContributionMasterId == contributionMasterId && d.isParked)
                .ToListAsync();
        }

        public async Task<List<ContributionDetail>> GetAllDetailsAsync(long contributionMasterId)
        {
            return await _context.ContributionDetails
                .Where(d => d.ContributionMasterId == contributionMasterId).AsNoTracking()
                .ToListAsync();
        }

        // ✅ AFTER — single query with NOT EXISTS
        public async Task<List<DefaulterDTO>> GetDefaultersAsync(string month, string year)
        {
            return await _context.Members
                .Where(m => !m.IsDeleted
                         && !_context.ContributionDetails
                             .Any(d => d.Month == month
                                    && d.Year == year
                                    && !d.isParked
                                    && d.StaffNo == m.StaffNo.ToString()))
                .Select(m => new DefaulterDTO
                {
                    MemberId = m.MemberId,
                    StaffNo = m.StaffNo,
                    Name = m.Name,
                    BranchId = m.BranchId
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}