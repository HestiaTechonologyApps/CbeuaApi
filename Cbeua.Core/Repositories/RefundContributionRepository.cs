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
        public IQueryable<RefundContribution> GetQueryableRefundContributions()
        {
            var q = from rc in _context.RefundContributions
                    select new RefundContribution
                    {
                        RefundContributionId = rc.RefundContributionId,
                        StaffNo = rc.StaffNo,
                        StateId = rc.StateId,
                        DesignationId = rc.DesignationId,
                        DeathDate = rc.DeathDate,
                        RefundNO = rc.RefundNO,
                        BranchNameOFTime = rc.BranchNameOFTime,
                        DPCODEOfTime = rc.DPCODEOfTime,
                        Type = rc.Type,
                        Remark = rc.Remark,
                        DDNO = rc.DDNO,
                        DDDATE = rc.DDDATE,
                        Amount = rc.Amount,
                        LastContribution = rc.LastContribution,
                        YearOF = rc.YearOF

                    };
            return q;
        }
    }
}
