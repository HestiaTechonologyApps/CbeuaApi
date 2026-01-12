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
            return _repo.GetQueryableListAccountDirect().ToList();
        }


        public async Task<List<AccountsDirectEntryDTO>> GetByMemberId(int id)
        {
            var q = _repo.GetQueryableListAccountDirect();
            var items= await q.Where(x => x.MemberId == id).ToListAsync();
            return items;
        }

        public async Task<AccountsDirectEntryDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var accountsDirectEntrieDTO = await ConvertAccountDirectEntryToDTO(q);
            return accountsDirectEntrieDTO;
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
               changedBy: "System" // Replace with actual user info
               );
            return await ConvertAccountDirectEntryToDTO(accountsDirect);
        }

        private async Task<AccountsDirectEntryDTO> ConvertAccountDirectEntryToDTO(AccountsDirectEntry accountsDirect)
        {
            AccountsDirectEntryDTO accountsDirectEntryDTO = new AccountsDirectEntryDTO();
            accountsDirectEntryDTO.AccountsDirectEntryID = accountsDirect.AccountsDirectEntryID;
            accountsDirectEntryDTO.MemberId = accountsDirect.MemberId;
            accountsDirectEntryDTO.Name = accountsDirect.Name;
            accountsDirectEntryDTO.BranchId = accountsDirect.BranchId;
            accountsDirectEntryDTO.MonthCode = accountsDirect.MonthCode;
            accountsDirectEntryDTO.YearOf = accountsDirect.YearOf;
            accountsDirectEntryDTO.DdIba = accountsDirect.DdIba;
            accountsDirectEntryDTO.DdIbaDate = accountsDirect.DdIbaDate;
            accountsDirectEntryDTO.Amt = accountsDirect.Amt;
            accountsDirectEntryDTO.Enrl = accountsDirect.Enrl;
            accountsDirectEntryDTO.Fine = accountsDirect.Fine;
            accountsDirectEntryDTO.F9 = accountsDirect.F9;
            accountsDirectEntryDTO.F10 = accountsDirect.F10;
            accountsDirectEntryDTO.F11 = accountsDirect.F11;
            accountsDirectEntryDTO.status = accountsDirect.status;
            accountsDirectEntryDTO.isApproved = accountsDirect.isApproved;
            accountsDirectEntryDTO.ApprovedBy = accountsDirect.ApprovedBy;
            accountsDirectEntryDTO.ApprovedDate = accountsDirect.ApprovedDate;
            return accountsDirectEntryDTO;
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
               changedBy: "System" // Replace with actual user info
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
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
