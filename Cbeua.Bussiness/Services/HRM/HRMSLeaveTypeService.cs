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
    public class HRMSLeaveTypeService : IHRMSLeaveTypeService
    {
        private readonly IHRMSLeaveTypeRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRMS_LeaveType";

        public HRMSLeaveTypeService(
            IHRMSLeaveTypeRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMSLeaveTypeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();

            if (!ShowDeleted)
                entities = entities.Where(x => x.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(x => x.IsActive == false);

            return await entities.ToListAsync();
        }

        public async Task<HRMSLeaveTypeDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMSLeaveTypeCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMSLeaveType
                {
                    Name = entitydto.Name,
                    Description = entitydto.Description,
                    MaxDaysAllowed = entitydto.MaxDaysAllowed,
                    IsPaid = entitydto.IsPaid,
                    CarryForward = entitydto.CarryForward,
                    CarryForwardLimit = entitydto.CarryForwardLimit,
                    ApplicableGender = entitydto.ApplicableGender,
                    RequiresDocument = entitydto.RequiresDocument,
                    NoticeDaysRequired = entitydto.NoticeDaysRequired,
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

        public async Task<CustomApiResponse> UpdateAsync(HRMSLeaveTypeCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "LeaveType not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.Name = entitydto.Name;
                entity.Description = entitydto.Description;
                entity.MaxDaysAllowed = entitydto.MaxDaysAllowed;
                entity.IsPaid = entitydto.IsPaid;
                entity.CarryForward = entitydto.CarryForward;
                entity.CarryForwardLimit = entitydto.CarryForwardLimit;
                entity.ApplicableGender = entitydto.ApplicableGender;
                entity.RequiresDocument = entitydto.RequiresDocument;
                entity.NoticeDaysRequired = entitydto.NoticeDaysRequired;
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
                        "LeaveType not found or already deleted",
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

        public async Task<PagedResult<HRMSLeaveTypeDTO>> GetPagedAsync(HRMSLeaveTypePaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(x => x.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(x => x.IsActive == false);

            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(x => x.Id == parameters.Id.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Name))
                allEntities = allEntities.Where(x => x.Name.ToLower().Contains(parameters.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.ApplicableGender))
                allEntities = allEntities.Where(x => x.ApplicableGender.ToLower() == parameters.ApplicableGender.ToLower());

            if (parameters.IsPaid.HasValue)
                allEntities = allEntities.Where(x => x.IsPaid == parameters.IsPaid.Value);

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(x =>
                    x.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(x.ApplicableGender) && x.ApplicableGender.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.Name)
                        : allEntities.OrderBy(x => x.Name),
                    "maxdaysallowed" => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.MaxDaysAllowed)
                        : allEntities.OrderBy(x => x.MaxDaysAllowed),
                    "createdat" => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.CreatedAt)
                        : allEntities.OrderBy(x => x.CreatedAt),
                    _ => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.Id)
                        : allEntities.OrderBy(x => x.Id)
                };
            }
            else
            {
                allEntities = allEntities.OrderByDescending(x => x.Id);
            }

            var totalRecords = allEntities.Count();
            var pagedData = allEntities
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            return new PagedResult<HRMSLeaveTypeDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        private HRMSLeaveType CloneEntity(HRMSLeaveType e) => new HRMSLeaveType
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            MaxDaysAllowed = e.MaxDaysAllowed,
            IsPaid = e.IsPaid,
            CarryForward = e.CarryForward,
            CarryForwardLimit = e.CarryForwardLimit,
            ApplicableGender = e.ApplicableGender,
            RequiresDocument = e.RequiresDocument,
            NoticeDaysRequired = e.NoticeDaysRequired,
            IsActive = e.IsActive,
            IsDeleted = e.IsDeleted,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };
    }
}