using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class CircleRepository : GenericRepository<Circle>, ICircleRepository
    {
        private readonly AppDbContext _context;

        public CircleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<CircleDTO> QueryableCircles()
        {
            var q = from c in _context.Circles
                    join s in _context.States on c.StateId equals s.StateId into stateJoin
                    from s in stateJoin.DefaultIfEmpty()
                    select new CircleDTO
                    {
                        CircleId = c.CircleId,
                        CircleCode = c.CircleCode,
                        Name = c.Name,
                        Abbreviation = c.Abbreviation,
                        IsActive = c.IsActive,
                        StateId = c.StateId,
                        StateName = s.Name ?? "",
                        DateFrom = c.DateFrom,
                        DateTo = c.DateTo
                    };
            return q;
        }
    }
}