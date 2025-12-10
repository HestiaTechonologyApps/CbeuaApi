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
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        private readonly AppDbContext _context;
        public BranchRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
