using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class MemberAccountDetailsRepository: IMemberAccountDetailsRepository
    {
        private readonly AppDbContext _context;

        public MemberAccountDetailsRepository(AppDbContext context) 
        {
            _context = context;
        }
        public IQueryable<MemberAccountDetailsDTO> QueryableUserList(int MemberId)
        {
            var q = from a in _context.Accounts
                    join c in _context.Circles on a.CircleId equals c.CircleId
                    join b in _context.Branches on a.BranchId equals b.BranchId
                    where a.MemeberId == MemberId
                    select new MemberAccountDetailsDTO
                    {
                        AccountId = a.AccountId,
                        CircleId = a.CircleId,
                        BranchId = a.BranchId,
                        MemeberId = a.MemeberId,
                        MonthCode = a.MonthCode,
                        YearOf = a.YearOf,
                        Amount = a.Amount,
                        TransMode = a.TransMode,
                        Reference = a.Reference,
                        Remark = a.Remark,
                        CircleName = c.Name,
                        BranchName = b.Name,
                    };
            return q;
        }
    }
}
