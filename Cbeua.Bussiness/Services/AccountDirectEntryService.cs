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
            // Only return non-deleted entries
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
            var accountsDirectEntry = await q.Where(x => x.AccountsDirectEntryID == id).FirstOrDefaultAsync();
            return accountsDirectEntry;
        }

        public async Task<AccountsDirectEntryDTO> CreateAsync(AccountsDirectEntry accountsDirect)
        {
            accountsDirect.IsDeleted = false; // ✅ ENSURE NOT DELETED
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
            if (oldentity == null || oldentity.IsDeleted) return false; // ✅ CHECK IF DELETED

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
            if (accountsDirectEntry == null || accountsDirectEntry.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            var oldEntity = CloneAccountsDirectEntry(accountsDirectEntry); // ✅ CLONE FOR AUDIT

            // ✅ SOFT DELETE
            accountsDirectEntry.IsDeleted = true;
            _repo.Update(accountsDirectEntry);

            await _auditRepository.LogAuditAsync<AccountsDirectEntry>(
               tableName: AuditTableName,
               action: "delete",
               recordId: accountsDirectEntry.AccountsDirectEntryID,
               oldEntity: oldEntity,
               newEntity: accountsDirectEntry,
               changedBy: "System"
            );

            await _repo.SaveChangesAsync();
            return true;
        }

        // ✅ ADDED CLONE METHOD FOR AUDIT
        private AccountsDirectEntry CloneAccountsDirectEntry(AccountsDirectEntry entry)
        {
            return new AccountsDirectEntry
            {
                AccountsDirectEntryID = entry.AccountsDirectEntryID,
                MemberId = entry.MemberId,
                Name = entry.Name,
                BranchId = entry.BranchId,
                MonthCode = entry.MonthCode,
                YearOf = entry.YearOf,
                DdIba = entry.DdIba,
                DdIbaDate = entry.DdIbaDate,
                Amt = entry.Amt,
                Enrl = entry.Enrl,
                Fine = entry.Fine,
                F9 = entry.F9,
                F10 = entry.F10,
                F11 = entry.F11,
                status = entry.status,
                IsDeleted = entry.IsDeleted,
                isApproved = entry.isApproved,
                ApprovedBy = entry.ApprovedBy,
                ApprovedDate = entry.ApprovedDate
            };
        }
    }
}