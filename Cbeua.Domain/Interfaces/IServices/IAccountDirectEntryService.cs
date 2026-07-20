using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IAccountDirectEntryService
    {
        Task<List<AccountsDirectEntryDTO>> GetAllAsync();

        Task<List<AccountsDirectEntryDTO>> GetByMemberId(int id);
        Task<AccountsDirectEntryDTO?> GetByIdAsync(int id);
        Task<AccountsDirectEntryDTO> CreateAsync(AccountsDirectEntry accountsDirect);
        Task<bool> UpdateAsync(AccountsDirectEntry accountsDirect);
        Task<CustomApiResponse> ApproveAsync(int id, int currentUserId, bool approve);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<AccountsDirectEntryDTO>> GetPagedAccountDirectEntriesAsync(AccountDirectEntryPaginationParams parameters);

    }
}
