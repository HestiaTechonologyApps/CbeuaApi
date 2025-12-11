using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "ACCOUNT";
        public AccountService(IAccountRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<AccountsDTO>> GetAllAsync()
        {
            List<AccountsDTO> accountDTOs = new List<AccountsDTO>();
            var accounts = await _repo.GetAllAsync();

            foreach (var account in accounts)
            {
                AccountsDTO accountDTO = await ConvertAccountToDTO(account);
                accountDTOs.Add(accountDTO);


            }

            return accountDTOs;
        }

        public async Task<AccountsDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var accountDTO = await ConvertAccountToDTO(q);
            return accountDTO;
        }

        public async Task<AccountsDTO> CreateAsync(Accounts accounts)
        {
            await _repo.AddAsync(accounts);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Accounts>(
               tableName: AuditTableName,
               action: "create",
               recordId: (int)accounts.AccountId,
               oldEntity: null,
               newEntity: accounts,
               changedBy: "System" // Replace with actual user info
               );
            return await ConvertAccountToDTO(accounts);
        }

        private async Task<AccountsDTO> ConvertAccountToDTO(Accounts accounts)
        {
            AccountsDTO accountsDTO = new AccountsDTO();
            accountsDTO.AccountId = accounts.AccountId;
            accountsDTO.CircleId = accounts.CircleId;
            accountsDTO.BranchId = accounts.BranchId;
            accountsDTO.MemeberId = accounts.MemeberId;
            accountsDTO.MonthCode = accounts.MonthCode;
            accountsDTO.YearOf = accounts.YearOf;
            accountsDTO.Amount = accounts.Amount;
            accountsDTO.Amount=accounts.Amount;
            accountsDTO.TransMode = accounts.TransMode;
            accounts.Reference = accounts.Reference;
            accountsDTO.Remark = accounts.Remark;
            return accountsDTO;
        }

        public async Task<bool> UpdateAsync(Accounts accounts)
        {
            var oldentity = await _repo.GetByIdAsync(accounts.AccountId);
            _repo.Detach(oldentity);
            _repo.Update(accounts);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Accounts>(
               tableName: AuditTableName,
               action: "update",
               recordId: (int)accounts.AccountId,
               oldEntity: oldentity,
               newEntity: accounts,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var accounts = await _repo.GetByIdAsync(id);
            if (accounts == null) return false;
            _repo.Delete(accounts);
            await _auditRepository.LogAuditAsync<Accounts>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: (int)accounts.AccountId,
               oldEntity: accounts,
               newEntity: accounts,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
