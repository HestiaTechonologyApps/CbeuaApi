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
    public class ReportTypeRepository : GenericRepository<ReportType>, IReportTypeRepository
    {
        private readonly AppDbContext _context;

        public ReportTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<ReportTypeDTO> QueryableReportTypes()
        {
            var query = from rt in _context.Set<ReportType>()
                        select new ReportTypeDTO
                        {
                            ReportTypeId = rt.ReportTypeId,
                            ReportTypeName = rt.ReportTypeName,
                            Description = rt.Description,
                            IsActive = rt.IsActive,
                            CreatedDate = rt.CreatedDate,
                            ModifiedDate = rt.ModifiedDate
                        };

            return query;
        }
    }
}