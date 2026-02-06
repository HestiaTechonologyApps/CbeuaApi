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
    public class RefundContributionRepository : GenericRepository<RefundContribution>, IRefundContributionRepository
    {
        private readonly AppDbContext _context;
        public RefundContributionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<RefundContributionDTO> QueryableRefundContributions()
        {
            var q = from rc in _context.RefundContributions
                    join m in _context.Members on rc.MemberId equals m.MemberId
                    join s in _context.States on rc.StateId equals s.StateId into stateJoin
                    from s in stateJoin.DefaultIfEmpty()
                    join d in _context.Designations on rc.DesignationId equals d.DesignationId into designationJoin
                    from d in designationJoin.DefaultIfEmpty()
                    join y in _context.YearMasters on rc.YearOF equals y.YearOf
                    where !rc.IsDeleted
                    select new RefundContributionDTO
                    {
                        RefundContributionId = rc.RefundContributionId,
                        MemberId = rc.MemberId,
                        MemberName = m.Name,
                        StaffNo = m.StaffNo,
                        StateId = rc.StateId,
                        StateName = s.Name ?? "",
                        DesignationId = rc.DesignationId,
                        DesignationName = d.Name ?? "",
                        RefundNO = rc.RefundNO,
                        BranchNameOFTime = rc.BranchNameOFTime,
                        DPCODEOfTime = rc.DPCODEOfTime,
                        Type = rc.Type,
                        Remark = rc.Remark,
                        DDNO = rc.DDNO,
                        DDDATE = rc.DDDATE,
                        Amount = rc.Amount,
                        LastContribution = rc.LastContribution,
                        YearOF = rc.YearOF,
                        YearName = y.YearName,
                        IsDeleted = rc.IsDeleted
                    };
            return q;
        }
    }
}