using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IAccountDirectEntryRepository : IGenericRepository<AccountsDirectEntry>
    {
        IQueryable<AccountsDirectEntryDTO> GetQueryableListAccountDirect();
        Task<int> GetCircleIdByBranchIdAsync(int branchId);
        Task AddAccountAsync(Accounts account);
        Task AddAccountsRangeAsync(List<Accounts> accounts);
    }
}
