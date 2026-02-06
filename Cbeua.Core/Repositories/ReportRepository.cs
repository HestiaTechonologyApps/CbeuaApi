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
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<ReportDTO> QueryableReports()
        {
            var query = from r in _context.Set<Report>()
                        join y in _context.YearMasters on r.YearOf equals y.YearOf
                        join m in _context.Months on r.MonthCode equals m.MonthCode
                        join c in _context.Circles on r.CircleId equals c.CircleId
                        join b in _context.Branches on r.BranchId equals b.BranchId
                        join mem in _context.Members on r.MemberId equals mem.MemberId
                        select new ReportDTO
                        {
                            ReportId = r.ReportId,
                            ReportType = r.ReportType,
                            YearOf = r.YearOf,
                            YearName = y.YearName.ToString(),
                            MonthCode = r.MonthCode,
                            MonthName = m.MonthName,
                            CircleId = r.CircleId,
                            CircleName = c.Name,
                            BranchId = r.BranchId,
                            DpCode = b.DpCode,
                            BranchName = b.Name,
                            MemberId = r.MemberId,
                            MemberName = mem.Name,
                            StaffNo = mem.StaffNo,
                            CreatedDate = r.CreatedDate,
                            ModifiedDate = r.ModifiedDate,
                            IsActive = r.IsActive
                        };

            return query;
        }
    }
}