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
    public class AccountRepository : GenericRepository<Accounts>, IAccountRepository
    {
        private readonly AppDbContext _context;
        public AccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<AccountsDTO> GetQueryable()
        {
            var q = from a in _context.Accounts
                    select new AccountsDTO
                    {
                        AccountId = a.AccountId,
                        CircleId = a.CircleId,
                        BranchId = a.BranchId,
                        MemeberId = a.MemeberId,
                        MonthCode = a.MonthCode,
                        YearOf= a.YearOf,
                        Amount = a.Amount,
                        TransMode = a.TransMode,
                        Reference = a.Reference,
                        Remark = a.Remark,
                    };
            return q;
        }
    }
}
