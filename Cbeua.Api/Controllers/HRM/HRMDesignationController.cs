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
    public class HRMDesignationService : IHRMDesignationService
    {
        private readonly IHRMDesignationRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRM_Designation";

        public HRMDesignationService(
            IHRMDesignationRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMDesignationDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();

            if (!ShowDeleted)
                entities = entities.Where(d => d.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(d => d.IsActive == false);

            return await entities.ToListAsync();
        }

        public async Task<HRMDesignationDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(d => d.Id == id).FirstOrDefaultAsync();

            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMDesignationCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMDesignation
                {
                    HRMDepartmentId = entitydto.HRMDepartmentId,
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

        public async Task<CustomApiResponse> UpdateAsync(HRMDesignationCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Designation not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.HRMDepartmentId = entitydto.HRMDepartmentId;
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
                        "Designation not found or already deleted",
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

        public async Task<PagedResult<HRMDesignationDTO>> GetPagedAsync(HRMDesignationPaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(d => d.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(d => d.IsActive == false);

            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(d => d.Id == parameters.Id.Value);

            if (parameters.HRMDepartmentId.HasValue && parameters.HRMDepartmentId.Value > 0)
                allEntities = allEntities.Where(d => d.HRMDepartmentId == parameters.HRMDepartmentId.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Name))
                allEntities = allEntities.Where(d => d.Name.ToLower().Contains(parameters.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Description))
                allEntities = allEntities.Where(d => d.Description.ToLower().Contains(parameters.Description.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(d =>
                    d.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(d.Name) && d.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.Description) && d.Description.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.DepartmentName) && d.DepartmentName.ToLower().Contains(searchLower))
                );
            }

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
                    "departmentname" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.DepartmentName)
                        : allEntities.OrderBy(d => d.DepartmentName),
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

            return new PagedResult<HRMDesignationDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        private HRMDesignation CloneEntity(HRMDesignation entity) => new HRMDesignation
        {
            Id = entity.Id,
            HRMDepartmentId = entity.HRMDepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}