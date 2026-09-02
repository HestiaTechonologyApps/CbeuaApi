using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
                        ApprovedBy = rc.ApprovedBy ?? "",
                        isApproved = rc.isApproved,
                        ApprovedDate = rc.ApprovedDate,
                        YearOF = rc.YearOF,
                        YearName = y.YearName,
                        IsDeleted = rc.IsDeleted
                    };
            return q;
        }
        public IQueryable<RefundContributionDTO> QueryableRefundContributionByMemberId(int memberId)
        {
            var query = from rc in _context.RefundContributions
                        join m in _context.Members on rc.MemberId equals m.MemberId
                        join s in _context.States on rc.StateId equals s.StateId into stateJoin
                        from s in stateJoin.DefaultIfEmpty()
                        join d in _context.Designations on rc.DesignationId equals d.DesignationId into designationJoin
                        from d in designationJoin.DefaultIfEmpty()
                        join y in _context.YearMasters on rc.YearOF equals y.YearOf into yearJoin
                        from y in yearJoin.DefaultIfEmpty()
                        where !rc.IsDeleted && rc.MemberId == memberId
                        select new RefundContributionDTO
                        {
                            RefundContributionId = rc.RefundContributionId,
                            MemberId = rc.MemberId,
                            MemberName = m.Name,
                            StaffNo = m.StaffNo,
                            StateId = rc.StateId,
                            StateName = s != null ? s.Name : "",
                            DesignationId = rc.DesignationId,
                            DesignationName = d != null ? d.Name : "",
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
                            ApprovedBy = rc.ApprovedBy ?? "",
                            isApproved = rc.isApproved,
                            ApprovedDate = rc.ApprovedDate,
                            YearName = y != null ? y.YearName : 0,
                            IsDeleted = rc.IsDeleted
                        };
            return query;
        }
        public IQueryable<RefundContributionDTO> QueryableRefundContributionById(int refundContributionId)
        {
            var query = from rc in _context.RefundContributions.AsNoTracking()
                        join m in _context.Members on rc.MemberId equals m.MemberId
                        join s in _context.States.AsNoTracking() on rc.StateId equals s.StateId into stateJoin
                        from s in stateJoin.DefaultIfEmpty()
                        join d in _context.Designations.AsNoTracking() on rc.DesignationId equals d.DesignationId into designationJoin
                        from d in designationJoin.DefaultIfEmpty()
                        join y in _context.YearMasters.AsNoTracking() on rc.YearOF equals y.YearOf into yearJoin
                        from y in yearJoin.DefaultIfEmpty()
                        where !rc.IsDeleted && rc.RefundContributionId == refundContributionId
                        select new RefundContributionDTO
                        {
                            RefundContributionId = rc.RefundContributionId,
                            MemberId = rc.MemberId,
                            MemberName = m.Name,
                            StaffNo = m.StaffNo,
                            StateId = rc.StateId,
                            StateName = s != null ? s.Name : "",
                            DesignationId = rc.DesignationId,
                            DesignationName = d != null ? d.Name : "",
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
                            ApprovedBy = rc.ApprovedBy ?? "",
                            isApproved = rc.isApproved,
                            ApprovedDate = rc.ApprovedDate,
                            YearName = y != null ? y.YearName : 0,
                            IsDeleted = rc.IsDeleted
                        };
            return query;
        }

        public async Task<MemberRefundEligibilityDTO> GetMemberRefundEligibilityAsync(int memberId, int? excludeRefundContributionId = null)
        {
            var totalContribution = await _context.Accounts
                .Where(a => a.MemeberId == memberId)
                .SumAsync(a => (decimal?)a.Amount) ?? 0m;

            var lastAccount = await (
                from a in _context.Accounts
                join m in _context.Months on a.MonthCode equals m.MonthCode
                join y in _context.YearMasters on a.YearOf equals y.YearOf
                where a.MemeberId == memberId
                orderby y.YearName descending, a.MonthCode descending
                select new { m.MonthName, y.YearName, a.Amount }
            ).FirstOrDefaultAsync();

            var refundQuery = _context.RefundContributions
                .Where(rc => rc.MemberId == memberId && !rc.IsDeleted);

            if (excludeRefundContributionId.HasValue)
                refundQuery = refundQuery.Where(rc => rc.RefundContributionId != excludeRefundContributionId.Value);

            var approvedAmount = await refundQuery
                .Where(rc => rc.isApproved)
                .SumAsync(rc => (decimal?)rc.Amount) ?? 0m;

            var pendingAmount = await refundQuery
                .Where(rc => !rc.isApproved)
                .SumAsync(rc => (decimal?)rc.Amount) ?? 0m;

            return new MemberRefundEligibilityDTO
            {
                MemberId = memberId,
                LastContributionMonth = lastAccount?.MonthName ?? "",
                LastContributionYear = lastAccount?.YearName ?? 0,
                LastContributionAmount = lastAccount?.Amount ?? 0m,
                TotalContribution = totalContribution,
                ApprovedAmount = approvedAmount,
                PendingAmount = pendingAmount,
                AvailableAmount = totalContribution - approvedAmount - pendingAmount
            };
        }

    }
}