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
    public class CircleStateRepository : GenericRepository<CircleState>, ICircleStateRepository
    {
        private readonly AppDbContext _context;
        public CircleStateRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<CircleStateDTO> GetQueryableCircleStates()
        {
            var q = from cs in _context.CircleStates
                    select new CircleStateDTO
                    {
                        CircleId = cs.CircleId,
                        StateId = cs.StateId,
                        CreatedByUserId = cs.CreatedByUserId,
                        CreatedDate = cs.CreatedDate,
                        ModifiedByUserId = cs.ModifiedByUserId,
                        ModifiedDate = cs.ModifiedDate


                    };
            return q;
        }
    }
}
