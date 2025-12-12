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
            return _context.Circles
                .Include(c => c.State)
                .Select(c => new CircleDTO
                {
                    CircleId = c.CircleId,
                    CircleCode = c.CircleCode,
                    Name = c.Name,
                    Abbreviation = c.Abbreviation,
                    IsActive = c.IsActive,
                    StateId = c.StateId,
                    StateName = c.State.Name,
                    DateFrom = c.DateFrom,
                    DateTo = c.DateTo
                });
        }
       




    }
}
