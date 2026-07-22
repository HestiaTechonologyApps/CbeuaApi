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
    public class DirectPaymentRepository : GenericRepository<DirectPayment>, IDirectPaymentRepository
    {
        private readonly AppDbContext _context;

        public DirectPaymentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<DirectPaymentDTO> QueryableDirectPayments()
        {
            var query = from dp in _context.DirectPayments
                        join m in _context.Members on dp.MemberId equals m.MemberId
                        select new DirectPaymentDTO
                        {
                            DirectPaymentId = dp.DirectPaymentId,
                            MemberId = dp.MemberId,
                            MemberName = m.Name,
                            Amount = dp.Amount,
                            PaymentDate = dp.PaymentDate,
                            PaymentMode = dp.PaymentMode,
                            ReferenceNo = dp.ReferenceNo,
                            Remarks = dp.Remarks,
                            CreatedByUserId = dp.CreatedByUserId,
                            CreatedDate = dp.CreatedDate,
                            ApprovedBy = dp.ApprovedBy ?? "",
                            isApproved=dp.isApproved,
                            ApprovedDate=dp.ApprovedDate,
                            IsDeleted = dp.IsDeleted
                        };
            return query;
        }
        public async Task<int?> GetBranchIdByMemberIdAsync(int memberId)
        {
            return await _context.Members
                .Where(m => m.MemberId == memberId)
                .Select(m => m.BranchId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetCircleIdByMemberIdAsync(int memberId)
        {
            return await (from m in _context.Members
                          join b in _context.Branches on m.BranchId equals b.BranchId
                          where m.MemberId == memberId
                          select b.CircleId)
                           .FirstOrDefaultAsync();
        }

        public async Task AddAccountAsync(Accounts account)
        {
            await _context.Accounts.AddAsync(account);
        }
    }
}