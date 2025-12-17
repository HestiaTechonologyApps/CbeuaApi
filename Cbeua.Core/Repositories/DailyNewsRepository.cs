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
    public class DailyNewsRepository : GenericRepository<DailyNews>, IDailyNewsRepository
    {
        private readonly AppDbContext _context;
        public DailyNewsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<DailyNewsDTO> GetQueryableDailyNews()
        {
            var q = from dn in _context.DailyNews
                    select new DailyNewsDTO
                    {
                        DailyNewsId = dn.DailyNewsId,
                        Title = dn.Title,
                        Description = dn.Description,
                        NewsDate = dn.NewsDate,
                        CompanyId = dn.CompanyId,
                        IsActive = dn.IsActive,
                        IsDeleted = dn.IsDeleted,
                        CreatedOn = dn.CreatedOn,
                        CreatedBy = dn.CreatedBy
                    };
            return q;
        }
    }
}
