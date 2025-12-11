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
    public class AccountDirectEntryRepository : GenericRepository<AccountsDirectEntry>, IAccountDirectEntryRepository
    {
        private readonly AppDbContext _context;
        public AccountDirectEntryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<AccountsDirectEntryDTO> GetQueryableListAccountDirect()
        {
            var q = from ade in _context.AccountsDirectEntries
                    select new AccountsDirectEntryDTO
                    {
                        AccountsDirectEntryID = ade.AccountsDirectEntryID,
                        MemberId = ade.MemberId,
                        Name = ade.Name,
                        BranchId = ade.BranchId,
                        MonthCode = ade.MonthCode,
                        YearOf = ade.YearOf,
                        DdIba = ade.DdIba,
                        DdIbaDate = ade.DdIbaDate,
                        Amt = ade.Amt,
                        Enrl = ade.Enrl,
                        Fine = ade.Fine,
                        F9 = ade.F9,
                        F10 = ade.F10,
                        F11 = ade.F11,
                        status = ade.status,
                        isApproved = ade.isApproved,
                        ApprovedBy = ade.ApprovedBy,
                        ApprovedDate = ade.ApprovedDate


                    };
            return q;
        }
    }
}
