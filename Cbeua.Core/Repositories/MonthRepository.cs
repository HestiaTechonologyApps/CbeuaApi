using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class MonthRepository : GenericRepository<Month>, IMonthRepository
    {
        private readonly AppDbContext _context;
        public MonthRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<MonthDTO> GetQueryableMonths()
        {
            var q = from m in _context.Months
                    select new MonthDTO
                    {
                        MonthCode = m.MonthCode,
                        MonthName = m.MonthName,
                        Abbrivation = m.Abbrivation

                    };
            return q;
        }
    }
}
