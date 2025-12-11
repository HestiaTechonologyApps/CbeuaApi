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
     public class DesignationRepository : GenericRepository<Designation>, IDesignationRepository
    {
        private readonly AppDbContext _context;
        public DesignationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
