using Cbeua.Core.Helpers;
using Cbeua.Core.Repositories.HRMS;
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
    public class HRMInterviewTypeService : IHRMInterviewTypeService
    {
        private readonly IHRMInterviewTypeRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRMS_InterviewType";

        public HRMInterviewTypeService(
            IHRMInterviewTypeRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMInterviewTypeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();
            if (!ShowDeleted)
                entities = entities.Where(a => a.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(a => a.IsActive == false);
            return await entities.ToListAsync();
        }

        public async Task<HRMInterviewTypeDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMInterviewTypeCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMSInterviewType
                {
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

        public async Task<CustomApiResponse> UpdateAsync(HRMInterviewTypeCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Interview Type not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

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
                        "Interview Type not found or already deleted",
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

        public async Task<PagedResult<HRMInterviewTypeDTO>> GetPagedAsync(HRMInterviewTypePaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(a => a.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(a => a.IsActive == false);

            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(a => a.Id == parameters.Id.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Name))
                allEntities = allEntities.Where(a => a.Name.ToLower().Contains(parameters.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Description))
                allEntities = allEntities.Where(a => a.Description.ToLower().Contains(parameters.Description.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(a =>
                    a.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(a.Name) && a.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.Description) && a.Description.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.Name)
                        : allEntities.OrderBy(a => a.Name),
                    "description" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.Description)
                        : allEntities.OrderBy(a => a.Description),
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

            return new PagedResult<HRMInterviewTypeDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        private HRMSInterviewType CloneEntity(HRMSInterviewType entity) => new HRMSInterviewType
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}