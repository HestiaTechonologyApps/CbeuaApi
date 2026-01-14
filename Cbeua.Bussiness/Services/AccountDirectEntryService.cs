using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class AccountDirectEntryService : IAccountDirectEntryService
    {
        private readonly IAccountDirectEntryRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "ACCOUNT_DIRECT_ENTRY";

        public AccountDirectEntryService(IAccountDirectEntryRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<AccountsDirectEntryDTO>> GetAllAsync()
        {
            return await _repo.GetQueryableListAccountDirect().ToListAsync();
        }

        public async Task<List<AccountsDirectEntryDTO>> GetByMemberId(int id)
        {
            var q = _repo.GetQueryableListAccountDirect();
            var items = await q.Where(x => x.MemberId == id).ToListAsync();
            return items;
        }

        public async Task<AccountsDirectEntryDTO?> GetByIdAsync(int id)
        {
            var q = _repo.GetQueryableListAccountDirect();
            var accountsDirectEntry = await q.FirstOrDefaultAsync(x => x.AccountsDirectEntryID == id);
            return accountsDirectEntry;
        }

        public async Task<AccountsDirectEntryDTO> CreateAsync(AccountsDirectEntry accountsDirect)
        {
            await _repo.AddAsync(accountsDirect);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<AccountsDirectEntry>(
               tableName: AuditTableName,
               action: "create",
               recordId: accountsDirect.AccountsDirectEntryID,
               oldEntity: null,
               newEntity: accountsDirect,
               changedBy: "System"
            );

            // Return the full DTO with joined data
            return await GetByIdAsync(accountsDirect.AccountsDirectEntryID)
                   ?? new AccountsDirectEntryDTO();
        }

        public async Task<bool> UpdateAsync(AccountsDirectEntry accountsDirect)
        {
            var oldentity = await _repo.GetByIdAsync(accountsDirect.AccountsDirectEntryID);
            _repo.Detach(oldentity);
            _repo.Update(accountsDirect);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<AccountsDirectEntry>(
               tableName: AuditTableName,
               action: "update",
               recordId: accountsDirect.AccountsDirectEntryID,
               oldEntity: oldentity,
               newEntity: accountsDirect,
               changedBy: "System"
            );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var accountsDirectEntry = await _repo.GetByIdAsync(id);
            if (accountsDirectEntry == null) return false;
            _repo.Delete(accountsDirectEntry);
            await _auditRepository.LogAuditAsync<AccountsDirectEntry>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: accountsDirectEntry.AccountsDirectEntryID,
               oldEntity: accountsDirectEntry,
               newEntity: accountsDirectEntry,
               changedBy: "System"
            );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}