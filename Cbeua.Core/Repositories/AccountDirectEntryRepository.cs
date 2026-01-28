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
            // FIXED: Changed from INNER JOIN to LEFT JOIN to include all AccountsDirectEntries
            // even if related records don't exist in Members, Branches, Months, or YearMasters tables
            var q = from ade in _context.AccountsDirectEntries
                    join m in _context.Members on ade.MemberId equals m.MemberId into memberGroup
                    from m in memberGroup.DefaultIfEmpty()
                    join b in _context.Branches on ade.BranchId equals b.BranchId into branchGroup
                    from b in branchGroup.DefaultIfEmpty()
                    join month in _context.Months on ade.MonthCode equals month.MonthCode into monthGroup
                    from month in monthGroup.DefaultIfEmpty()
                    join year in _context.YearMasters on ade.YearOf equals year.YearOf into yearGroup
                    from year in yearGroup.DefaultIfEmpty()
                    select new AccountsDirectEntryDTO
                    {
                        AccountsDirectEntryID = ade.AccountsDirectEntryID,
                        MemberId = ade.MemberId,
                        Name = m != null ? m.Name : "",
                        MemberName = m != null ? m.Name : "",
                        BranchName = b != null ? b.Name : "",
                        MonthName = month != null ? month.MonthName : "",
                        BranchId = ade.BranchId,
                        MonthCode = ade.MonthCode,
                        YearOf = ade.YearOf,
                        YearName = year != null ? year.YearName : 0,
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