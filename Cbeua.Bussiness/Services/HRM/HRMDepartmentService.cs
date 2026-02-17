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
    public class HRMDepartmentService : IHRMDepartmentService
    {
        private readonly IHRMDepartmentRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRM_Department";

        public HRMDepartmentService(
            IHRMDepartmentRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMDepartmentDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();

            if (!ShowDeleted)
                entities = entities.Where(d => d.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(d => d.IsActive == false);

            return await entities.ToListAsync();
        }

        public async Task<HRMDepartmentDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(d => d.Id == id).FirstOrDefaultAsync();

            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMDepartmentCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMDepartment
                {
                    HRMBranchId = entitydto.HRMBranchId,
                    Name = entitydto.Name,
                    Description = entitydto.Description,
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

        public async Task<CustomApiResponse> UpdateAsync(HRMDepartmentCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Department not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.HRMBranchId = entitydto.HRMBranchId;
                entity.Name = entitydto.Name;
                entity.Description = entitydto.Description;
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
                        "Department not found or already deleted",
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

        public async Task<PagedResult<HRMDepartmentDTO>> GetPagedAsync(HRMDepartmentPaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(d => d.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(d => d.IsActive == false);

            // Specific filters
            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(d => d.Id == parameters.Id.Value);

            if (parameters.HRMBranchId.HasValue && parameters.HRMBranchId.Value > 0)
                allEntities = allEntities.Where(d => d.HRMBranchId == parameters.HRMBranchId.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Name))
                allEntities = allEntities.Where(d => d.Name.ToLower().Contains(parameters.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Description))
                allEntities = allEntities.Where(d => d.Description.ToLower().Contains(parameters.Description.ToLower()));

            // General search
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(d =>
                    d.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(d.Name) && d.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.Description) && d.Description.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.BranchName) && d.BranchName.ToLower().Contains(searchLower))
                );
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.Name)
                        : allEntities.OrderBy(d => d.Name),
                    "description" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.Description)
                        : allEntities.OrderBy(d => d.Description),
                    "branchname" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.BranchName)
                        : allEntities.OrderBy(d => d.BranchName),
                    "createdat" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.CreatedAt)
                        : allEntities.OrderBy(d => d.CreatedAt),
                    _ => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.Id)
                        : allEntities.OrderBy(d => d.Id)
                };
            }
            else
            {
                allEntities = allEntities.OrderByDescending(d => d.Id);
            }

            var totalRecords = allEntities.Count();
            var pagedData = allEntities
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            return new PagedResult<HRMDepartmentDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        // ===== Helpers =====

        private HRMDepartment CloneEntity(HRMDepartment entity) => new HRMDepartment
        {
            Id = entity.Id,
            HRMBranchId = entity.HRMBranchId,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}