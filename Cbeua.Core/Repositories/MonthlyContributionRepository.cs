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
    }
}