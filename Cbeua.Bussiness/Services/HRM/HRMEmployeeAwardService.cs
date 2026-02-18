using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.Domain.Interfaces.IServices.HRMS;
using Microsoft.EntityFrameworkCore;

namespace Cbeua.Bussiness.Services.HRMS
{
    public class HRMEmployeeAwardService : IHRMEmployeeAwardService
    {
        private readonly IHRMEmployeeAwardRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRM_EmployeeAward";

        public HRMEmployeeAwardService(
            IHRMEmployeeAwardRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMEmployeeAwardDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();

            if (!ShowDeleted)
                entities = entities.Where(a => a.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(a => a.IsActive == false);

            return await entities.ToListAsync();
        }

        public async Task<HRMEmployeeAwardDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMEmployeeAwardCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HrmEmployeeAward
                {
                    HRMEmployeeId = entitydto.HRMEmployeeId,
                    HrmAwardTypeId = entitydto.HrmAwardTypeId,
                    AwardDate = entitydto.AwardDate,
                    Gift = entitydto.Gift,
                    Description = entitydto.Description,
                    MonetaryValue = entitydto.MonetaryValue,
                    IsActive = entitydto.IsActive,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync();

                await _auditRepository.LogAuditAsync(
                    tableName: AuditTableName,
                    action: "create",
                    recordId: entity.Id,
                    oldEntity: null,
                    newEntity: entity,
                    changedBy: _currentUser?.UserId ?? "System"
                );

                return ApiResponseFactory.Success(null, "Created Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> UpdateAsync(HRMEmployeeAwardCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Employee Award not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.HRMEmployeeId = entitydto.HRMEmployeeId;
                entity.HrmAwardTypeId = entitydto.HrmAwardTypeId;
                entity.AwardDate = entitydto.AwardDate;
                entity.Gift = entitydto.Gift;
                entity.Description = entitydto.Description;
                entity.MonetaryValue = entitydto.MonetaryValue;
                entity.IsActive = entitydto.IsActive;
                entity.IsDeleted = entitydto.IsDeleted;
                entity.UpdatedAt = DateTime.UtcNow;

                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                await _auditRepository.LogAuditAsync(
                    tableName: AuditTableName,
                    action: "update",
                    recordId: entity.Id,
                    oldEntity: oldEntity,
                    newEntity: entity,
                    changedBy: _currentUser?.UserId ?? "System"
                );

                return ApiResponseFactory.Success(entity, "Updated Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Employee Award not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.IsDeleted = true;
                entity.UpdatedAt = DateTime.UtcNow;
                _repo.Update(entity);

                await _auditRepository.LogAuditAsync(
                    tableName: AuditTableName,
                    action: "delete",
                    recordId: entity.Id,
                    oldEntity: oldEntity,
                    newEntity: entity,
                    changedBy: _currentUser?.UserId ?? "System"
                );

                await _repo.SaveChangesAsync();
                return ApiResponseFactory.Success(entity, "Deleted Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<PagedResult<HRMEmployeeAwardDTO>> GetPagedAsync(HRMEmployeeAwardPaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(a => a.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(a => a.IsActive == false);

            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(a => a.Id == parameters.Id.Value);

            if (parameters.HRMEmployeeId.HasValue && parameters.HRMEmployeeId.Value > 0)
                allEntities = allEntities.Where(a => a.HRMEmployeeId == parameters.HRMEmployeeId.Value);

            if (parameters.HrmAwardTypeId.HasValue && parameters.HrmAwardTypeId.Value > 0)
                allEntities = allEntities.Where(a => a.HrmAwardTypeId == parameters.HrmAwardTypeId.Value);

            if (parameters.AwardDateFrom.HasValue)
                allEntities = allEntities.Where(a => a.AwardDate >= parameters.AwardDateFrom.Value);

            if (parameters.AwardDateTo.HasValue)
                allEntities = allEntities.Where(a => a.AwardDate <= parameters.AwardDateTo.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Gift))
                allEntities = allEntities.Where(a => a.Gift.ToLower().Contains(parameters.Gift.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(a =>
                    a.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(a.EmployeeName) && a.EmployeeName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.EmployeeCode) && a.EmployeeCode.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.AwardTypeName) && a.AwardTypeName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.Gift) && a.Gift.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.Description) && a.Description.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "employeename" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.EmployeeName)
                        : allEntities.OrderBy(a => a.EmployeeName),
                    "awardtypename" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.AwardTypeName)
                        : allEntities.OrderBy(a => a.AwardTypeName),
                    "awarddate" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.AwardDate)
                        : allEntities.OrderBy(a => a.AwardDate),
                    "monetaryvalue" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.MonetaryValue)
                        : allEntities.OrderBy(a => a.MonetaryValue),
                    "createdat" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.CreatedAt)
                        : allEntities.OrderBy(a => a.CreatedAt),
                    _ => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.Id)
                        : allEntities.OrderBy(a => a.Id)
                };
            }
            else
            {
                allEntities = allEntities.OrderByDescending(a => a.Id);
            }

            var totalRecords = allEntities.Count();
            var pagedData = allEntities
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            return new PagedResult<HRMEmployeeAwardDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        private HrmEmployeeAward CloneEntity(HrmEmployeeAward entity) => new HrmEmployeeAward
        {
            Id = entity.Id,
            HRMEmployeeId = entity.HRMEmployeeId,
            HrmAwardTypeId = entity.HrmAwardTypeId,
            AwardDate = entity.AwardDate,
            Gift = entity.Gift,
            Description = entity.Description,
            MonetaryValue = entity.MonetaryValue,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}