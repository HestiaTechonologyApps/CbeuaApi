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
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly AppDbContext _context;
        public StateRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
   
}
