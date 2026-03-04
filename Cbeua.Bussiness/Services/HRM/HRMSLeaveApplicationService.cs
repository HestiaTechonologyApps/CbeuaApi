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
    public class HRMSLeaveApplicationService : IHRMSLeaveApplicationService
    {
        private readonly IHRMSLeaveApplicationRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRMS_LeaveApplication";

        public HRMSLeaveApplicationService(
            IHRMSLeaveApplicationRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMSLeaveApplicationDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();

            if (!ShowDeleted)
                entities = entities.Where(x => x.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(x => x.IsActive == false);

            return await entities.ToListAsync();
        }

        public async Task<HRMSLeaveApplicationDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMSLeaveApplicationCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMSLeaveApplication
                {
                    HRMEmployeeId = entitydto.HRMEmployeeId,
                    HRMSLeaveTypeId = entitydto.HRMSLeaveTypeId,
                    FromDate = entitydto.FromDate,
                    ToDate = entitydto.ToDate,
                    TotalDays = entitydto.TotalDays,
                    DayType = entitydto.DayType,
                    Reason = entitydto.Reason,
                    Status = "Pending",
                    AppliedOn = DateTime.UtcNow,
                    DocumentUrl = entitydto.DocumentUrl,
                    ReviewedOn = null,
                    ReviewerRemarks = entitydto.ReviewerRemarks,
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

                return ApiResponseFactory.Success(null, "Leave Application Submitted Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> UpdateAsync(HRMSLeaveApplicationCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Leave Application not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.HRMEmployeeId = entitydto.HRMEmployeeId;
                entity.HRMSLeaveTypeId = entitydto.HRMSLeaveTypeId;
                entity.FromDate = entitydto.FromDate;
                entity.ToDate = entitydto.ToDate;
                entity.TotalDays = entitydto.TotalDays;
                entity.DayType = entitydto.DayType;
                entity.Reason = entitydto.Reason;
                entity.Status = entitydto.Status;
                entity.DocumentUrl = entitydto.DocumentUrl;
                entity.ReviewedOn = entitydto.ReviewedOn;
                entity.ReviewerRemarks = entitydto.ReviewerRemarks;
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
                        "Leave Application not found or already deleted",
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

        public async Task<PagedResult<HRMSLeaveApplicationDTO>> GetPagedAsync(HRMSLeaveApplicationPaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(x => x.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(x => x.IsActive == false);

            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(x => x.Id == parameters.Id.Value);

            if (parameters.HRMEmployeeId.HasValue && parameters.HRMEmployeeId.Value > 0)
                allEntities = allEntities.Where(x => x.HRMEmployeeId == parameters.HRMEmployeeId.Value);

            if (parameters.HRMSLeaveTypeId.HasValue && parameters.HRMSLeaveTypeId.Value > 0)
                allEntities = allEntities.Where(x => x.HRMSLeaveTypeId == parameters.HRMSLeaveTypeId.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Status))
                allEntities = allEntities.Where(x => x.Status.ToLower() == parameters.Status.ToLower());

            if (parameters.FromDate.HasValue)
                allEntities = allEntities.Where(x => x.FromDate >= parameters.FromDate.Value);

            if (parameters.ToDate.HasValue)
                allEntities = allEntities.Where(x => x.ToDate <= parameters.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(x =>
                    x.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(x.EmployeeName) && x.EmployeeName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(x.LeaveTypeName) && x.LeaveTypeName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(x.Status) && x.Status.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(x.Reason) && x.Reason.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "employeename" => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.EmployeeName)
                        : allEntities.OrderBy(x => x.EmployeeName),
                    "leavetypename" => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.LeaveTypeName)
                        : allEntities.OrderBy(x => x.LeaveTypeName),
                    "fromdate" => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.FromDate)
                        : allEntities.OrderBy(x => x.FromDate),
                    "status" => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.Status)
                        : allEntities.OrderBy(x => x.Status),
                    "appliedon" => parameters.SortDescending
                        ? allEntities.OrderByDescending(x => x.AppliedOn)
                        : allEntities.OrderBy(x => x.AppliedOn),
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

            return new PagedResult<HRMSLeaveApplicationDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        private HRMSLeaveApplication CloneEntity(HRMSLeaveApplication e) => new HRMSLeaveApplication
        {
            Id = e.Id,
            HRMEmployeeId = e.HRMEmployeeId,
            HRMSLeaveTypeId = e.HRMSLeaveTypeId,
            FromDate = e.FromDate,
            ToDate = e.ToDate,
            TotalDays = e.TotalDays,
            DayType = e.DayType,
            Reason = e.Reason,
            Status = e.Status,
            AppliedOn = e.AppliedOn,
            DocumentUrl = e.DocumentUrl,

            ReviewedOn = e.ReviewedOn,
            ReviewerRemarks = e.ReviewerRemarks,
            IsActive = e.IsActive,
            IsDeleted = e.IsDeleted,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };
    }
}