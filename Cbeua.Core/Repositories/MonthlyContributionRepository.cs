using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;

namespace Cbeua.Core.Repositories
{
    public class MonthlyContributionRepository : GenericRepository<Accounts>, IMonthlyContributionRepository
    {
        private readonly AppDbContext _context;
        public MonthlyContributionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<IndividualContrbutionDTO>> GetIndividualContrbution(int MemeberId)
        {
            var q = (from accont in _context.Accounts
                     where accont.MemeberId == MemeberId
                     select new IndividualContrbutionDTO
                     {
                         MonthCode = accont.MonthCode,
                         YearNo = accont.YearOf,
                         Amount = accont.Amount,

                     }).ToListAsync();

            return q;
        }
    }
}
