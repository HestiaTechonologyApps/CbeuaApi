using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.Entities.Common;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;

namespace Cbeua.InfraCore.Repositories
{
    public class FinancialYearRepository : GenericRepository<FinancialYear>, IFinancialYearRepository
    {
        private readonly AppDbContext _context;
        public FinancialYearRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
