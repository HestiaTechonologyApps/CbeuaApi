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
    public class YearMasterRepository : GenericRepository<YearMaster>, IYearMasterRepository
    {
        private readonly AppDbContext _context;
        public YearMasterRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
