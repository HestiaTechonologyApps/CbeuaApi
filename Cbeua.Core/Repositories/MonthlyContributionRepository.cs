using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using System.Linq;

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
    }
}