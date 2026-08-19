using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class ExpenseMasterService : IExpenseMasterService
    {
        private readonly IExpenseMasterRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "EXPENSEMASTER";

        public ExpenseMasterService(IExpenseMasterRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<ExpenseMasterDTO>> GetAllAsync()
        {
            return _repo.QueryableExpenseMasters().OrderByDescending(em => em.ExpenseDate).ToList();
        }

        public async Task<ExpenseMasterDTO?> GetByIdAsync(int id)
        {
            var q = _repo.QueryableExpenseMasterById(id);
            return await q.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<ExpenseMasterDTO> CreateAsync(ExpenseMaster expenseMaster)
        {
            expenseMaster.IsDeleted = false;
            await _repo.AddAsync(expenseMaster);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<ExpenseMaster>(
               tableName: AuditTableName,
               action: "create",
               recordId: expenseMaster.ExpenseMasterId,
               oldEntity: null,
               newEntity: expenseMaster,
               changedBy: "System"
            );

            return await ConvertExpenseMasterToDTO(expenseMaster);
        }

        private async Task<ExpenseMasterDTO> ConvertExpenseMasterToDTO(ExpenseMaster expenseMaster)
        {
            var dto = _repo.QueryableExpenseMasterById(expenseMaster.ExpenseMasterId).AsNoTracking().FirstOrDefault();
            if (dto != null) return dto;

            return new ExpenseMasterDTO
            {
                ExpenseMasterId = expenseMaster.ExpenseMasterId,
                ExpenseTypeId = expenseMaster.ExpenseTypeId,
                ExpenseDate = expenseMaster.ExpenseDate,
                Amount = expenseMaster.Amount,
                PaidTo = expenseMaster.PaidTo,
                ReferenceNo = expenseMaster.ReferenceNo,
                PaymentMode = expenseMaster.PaymentMode,
                Description = expenseMaster.Description,
                IsDeleted = expenseMaster.IsDeleted
            };
        }

        private ExpenseMaster CloneExpenseMaster(ExpenseMaster expenseMaster)
        {
            return new ExpenseMaster
            {
                ExpenseMasterId = expenseMaster.ExpenseMasterId,
                ExpenseTypeId = expenseMaster.ExpenseTypeId,
                ExpenseDate = expenseMaster.ExpenseDate,
                Amount = expenseMaster.Amount,
                PaidTo = expenseMaster.PaidTo,
                ReferenceNo = expenseMaster.ReferenceNo,
                PaymentMode = expenseMaster.PaymentMode,
                Description = expenseMaster.Description,
                IsDeleted = expenseMaster.IsDeleted,
                isApproved = expenseMaster.isApproved,
                ApprovedBy = expenseMaster.ApprovedBy,
                ApprovedDate = expenseMaster.ApprovedDate
            };
        }

        public async Task<bool> UpdateAsync(ExpenseMaster expenseMaster)
        {
            var oldentity = await _repo.GetByIdAsync(expenseMaster.ExpenseMasterId);
            if (oldentity == null || oldentity.IsDeleted) return false;

            _repo.Detach(oldentity);
            _repo.Update(expenseMaster);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<ExpenseMaster>(
               tableName: AuditTableName,
               action: "update",
               recordId: expenseMaster.ExpenseMasterId,
               oldEntity: oldentity,
               newEntity: expenseMaster,
               changedBy: "System"
            );

            return true;
        }

        public async Task<CustomApiResponse> ApproveAsync(int id, int currentUserId, bool approve)
        {
            try
            {
                var entry = await _repo.GetByIdAsync(id);
                if (entry == null || entry.IsDeleted)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Expense not found",
                        StatusCode = 404
                    };

                if (entry.isApproved)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Expense is already approved",
                        StatusCode = 400
                    };

                var oldEntity = CloneExpenseMaster(entry);

                entry.isApproved = approve;
                entry.ApprovedBy = currentUserId.ToString();
                entry.ApprovedDate = DateTime.Now;

                _repo.Update(entry);

                await _auditRepository.LogAuditAsync<ExpenseMaster>(
                   tableName: AuditTableName,
                   action: "update",
                   recordId: entry.ExpenseMasterId,
                   oldEntity: oldEntity,
                   newEntity: entry,
                   changedBy: currentUserId.ToString()
                );

                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = approve
                        ? new { Message = "Expense approved successfully" }
                        : new { Message = "Expense rejected successfully" }
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}",
                    StatusCode = 500
                };
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expenseMaster = await _repo.GetByIdAsync(id);
            if (expenseMaster == null || expenseMaster.IsDeleted) return false;

            var oldEntity = CloneExpenseMaster(expenseMaster);

            expenseMaster.IsDeleted = true;
            _repo.Update(expenseMaster);

            await _auditRepository.LogAuditAsync<ExpenseMaster>(
               tableName: AuditTableName,
               action: "delete",
               recordId: expenseMaster.ExpenseMasterId,
               oldEntity: oldEntity,
               newEntity: expenseMaster,
               changedBy: "System"
            );

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<ExpenseMasterDTO>> GetPagedExpenseMastersAsync(ExpenseMasterPaginationParams parameters)
        {
            var query = _repo.QueryableExpenseMasters();

            if (parameters.ExpenseMasterId.HasValue && parameters.ExpenseMasterId.Value > 0)
                query = query.Where(em => em.ExpenseMasterId == parameters.ExpenseMasterId.Value);

            if (parameters.ExpenseTypeId.HasValue && parameters.ExpenseTypeId.Value > 0)
                query = query.Where(em => em.ExpenseTypeId == parameters.ExpenseTypeId.Value);

            if (parameters.FromDate.HasValue)
                query = query.Where(em => em.ExpenseDate >= parameters.FromDate.Value);

            if (parameters.ToDate.HasValue)
                query = query.Where(em => em.ExpenseDate <= parameters.ToDate.Value);

            var allExpenses = query.ToList();

            IEnumerable<ExpenseMasterDTO> filteredExpenses = allExpenses;

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();

                filteredExpenses = allExpenses.Where(em =>
                    em.ExpenseMasterId.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(em.ExpenseTypeName) && em.ExpenseTypeName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(em.PaidTo) && em.PaidTo.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(em.ReferenceNo) && em.ReferenceNo.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(em.PaymentMode) && em.PaymentMode.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(em.Description) && em.Description.ToLower().Contains(searchLower)) ||
                    em.Amount.ToString().Contains(searchLower)
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                var sortBy = parameters.SortBy.ToLower();

                filteredExpenses = sortBy switch
                {
                    "expensemasterid" => parameters.SortDescending
                        ? filteredExpenses.OrderByDescending(em => em.ExpenseMasterId)
                        : filteredExpenses.OrderBy(em => em.ExpenseMasterId),
                    "expensetypename" => parameters.SortDescending
                        ? filteredExpenses.OrderByDescending(em => em.ExpenseTypeName)
                        : filteredExpenses.OrderBy(em => em.ExpenseTypeName),
                    "expensedate" => parameters.SortDescending
                        ? filteredExpenses.OrderByDescending(em => em.ExpenseDate)
                        : filteredExpenses.OrderBy(em => em.ExpenseDate),
                    "amount" => parameters.SortDescending
                        ? filteredExpenses.OrderByDescending(em => em.Amount)
                        : filteredExpenses.OrderBy(em => em.Amount),
                    "paidto" => parameters.SortDescending
                        ? filteredExpenses.OrderByDescending(em => em.PaidTo)
                        : filteredExpenses.OrderBy(em => em.PaidTo),
                    _ => parameters.SortDescending
                        ? filteredExpenses.OrderByDescending(em => em.ExpenseDate)
                        : filteredExpenses.OrderBy(em => em.ExpenseDate)
                };
            }
            else
            {
                filteredExpenses = filteredExpenses.OrderByDescending(em => em.ExpenseDate);
            }

            var totalRecords = filteredExpenses.Count();

            var pageNumber = parameters.PageNumber;
            var pageSize = parameters.PageSize;

            List<ExpenseMasterDTO> pagedData;

            if (parameters.GetAll)
            {
                pagedData = filteredExpenses.ToList();
            }
            else
            {
                pagedData = filteredExpenses
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            return new PagedResult<ExpenseMasterDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = parameters.GetAll ? totalRecords : pageSize
            };
        }
    }
}
