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
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IExpenseTypeRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "EXPENSETYPE";

        public ExpenseTypeService(IExpenseTypeRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<ExpenseTypeDTO>> GetAllAsync()
        {
            return _repo.QueryableExpenseTypes().ToList();
        }

        public async Task<ExpenseTypeDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null || q.IsDeleted) return null;
            return await ConvertExpenseTypeToDTO(q);
        }

        public async Task<ExpenseTypeDTO> CreateAsync(ExpenseType expenseType)
        {
            expenseType.IsDeleted = false;
            await _repo.AddAsync(expenseType);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<ExpenseType>(
               tableName: AuditTableName,
               action: "create",
               recordId: expenseType.ExpenseTypeId,
               oldEntity: null,
               newEntity: expenseType,
               changedBy: "System"
            );
            return await ConvertExpenseTypeToDTO(expenseType);
        }

        private async Task<ExpenseTypeDTO> ConvertExpenseTypeToDTO(ExpenseType expenseType)
        {
            ExpenseTypeDTO expenseTypeDTO = new ExpenseTypeDTO();
            expenseTypeDTO.ExpenseTypeId = expenseType.ExpenseTypeId;
            expenseTypeDTO.Name = expenseType.Name;
            expenseTypeDTO.Description = expenseType.Description;
            expenseTypeDTO.IsDeleted = expenseType.IsDeleted;
            return expenseTypeDTO;
        }

        private ExpenseType CloneExpenseType(ExpenseType expenseType)
        {
            return new ExpenseType
            {
                ExpenseTypeId = expenseType.ExpenseTypeId,
                Name = expenseType.Name,
                Description = expenseType.Description,
                IsDeleted = expenseType.IsDeleted
            };
        }

        public async Task<bool> UpdateAsync(ExpenseType expenseType)
        {
            var oldentity = await _repo.GetByIdAsync(expenseType.ExpenseTypeId);
            if (oldentity == null || oldentity.IsDeleted) return false;

            _repo.Detach(oldentity);
            _repo.Update(expenseType);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<ExpenseType>(
               tableName: AuditTableName,
               action: "update",
               recordId: expenseType.ExpenseTypeId,
               oldEntity: oldentity,
               newEntity: expenseType,
               changedBy: "System"
            );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expenseType = await _repo.GetByIdAsync(id);
            if (expenseType == null || expenseType.IsDeleted) return false;

            var oldEntity = CloneExpenseType(expenseType);

            expenseType.IsDeleted = true;
            _repo.Update(expenseType);

            await _auditRepository.LogAuditAsync<ExpenseType>(
               tableName: AuditTableName,
               action: "delete",
               recordId: expenseType.ExpenseTypeId,
               oldEntity: oldEntity,
               newEntity: expenseType,
               changedBy: "System"
            );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
