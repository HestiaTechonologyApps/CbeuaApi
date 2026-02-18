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
    public class HRMEmployeeService : IHRMEmployeeService
    {
        private readonly IHRMEmployeeRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRM_Employee";

        public HRMEmployeeService(
            IHRMEmployeeRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMEmployeeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();

            if (!ShowDeleted)
                entities = entities.Where(e => e.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(e => e.IsActive == false);

            return await entities.ToListAsync();
        }

        public async Task<HRMEmployeeDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMEmployeeCreateUpdateDTO entitydto)
        {
            try
            {
                // ========== AUTO-GENERATE EMPLOYEE ID ==========
                var employees = await _repo.GetQuerableList();
                var lastEmployee = await employees
                    .OrderByDescending(e => e.Id)
                    .FirstOrDefaultAsync();

                int nextId = (lastEmployee?.Id ?? 0) + 1;
                string employeeId = $"EMP{nextId:D2}"; 

                var entity = new HRMEmployee
                {
                    FullName = entitydto.FullName,
                    EmployeeId = employeeId, 
                    EmployeeCode = entitydto.EmployeeCode,
                    Email = entitydto.Email,
                    PasswordHash = entitydto.PasswordHash,
                    PhoneNumber = entitydto.PhoneNumber,
                    DateOfBirth = entitydto.DateOfBirth,
                    Gender = entitydto.Gender,
                    ProfileImagePath = entitydto.ProfileImagePath,

                    HRMBranchId = entitydto.HRMBranchId,
                    HRMDepartmentId = entitydto.HRMDepartmentId,
                    HRMDesignationId = entitydto.HRMDesignationId,
                    DateOfJoining = entitydto.DateOfJoining,
                    EmploymentType = entitydto.EmploymentType,
                    EmployeeStatus = entitydto.EmployeeStatus,
                    ShiftId = entitydto.ShiftId,
                    AttendancePolicyId = entitydto.AttendancePolicyId,

                    AddressLine1 = entitydto.AddressLine1,
                    AddressLine2 = entitydto.AddressLine2,
                    City = entitydto.City,
                    State = entitydto.State,
                    Country = entitydto.Country,
                    PostalCode = entitydto.PostalCode,

                    EmergencyContactName = entitydto.EmergencyContactName,
                    EmergencyContactRelationship = entitydto.EmergencyContactRelationship,

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

                return ApiResponseFactory.Success(new
                {
                    id = entity.Id,
                    employeeId = entity.EmployeeId
                }, "Created Successfully");
            }
            catch (DbUpdateException dbEx)
            {
                // Return detailed database error for debugging
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return ApiResponseFactory.Fail(
                    $"Database error: {innerMessage}",
                    System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> UpdateAsync(HRMEmployeeCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Employee not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.FullName = entitydto.FullName;
                // NOTE: EmployeeId is auto-generated and should NOT be updated
                entity.EmployeeCode = entitydto.EmployeeCode;
                entity.Email = entitydto.Email;
                entity.PasswordHash = entitydto.PasswordHash;
                entity.PhoneNumber = entitydto.PhoneNumber;
                entity.DateOfBirth = entitydto.DateOfBirth;
                entity.Gender = entitydto.Gender;
                entity.ProfileImagePath = entitydto.ProfileImagePath;

                entity.HRMBranchId = entitydto.HRMBranchId;
                entity.HRMDepartmentId = entitydto.HRMDepartmentId;
                entity.HRMDesignationId = entitydto.HRMDesignationId;
                entity.DateOfJoining = entitydto.DateOfJoining;
                entity.EmploymentType = entitydto.EmploymentType;
                entity.EmployeeStatus = entitydto.EmployeeStatus;
                entity.ShiftId = entitydto.ShiftId;
                entity.AttendancePolicyId = entitydto.AttendancePolicyId;

                entity.AddressLine1 = entitydto.AddressLine1;
                entity.AddressLine2 = entitydto.AddressLine2;
                entity.City = entitydto.City;
                entity.State = entitydto.State;
                entity.Country = entitydto.Country;
                entity.PostalCode = entitydto.PostalCode;

                entity.EmergencyContactName = entitydto.EmergencyContactName;
                entity.EmergencyContactRelationship = entitydto.EmergencyContactRelationship;

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
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return ApiResponseFactory.Fail(
                    $"Database error: {innerMessage}",
                    System.Net.HttpStatusCode.InternalServerError);
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
                        "Employee not found or already deleted",
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

        public async Task<PagedResult<HRMEmployeeDTO>> GetPagedAsync(HRMEmployeePaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(e => e.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(e => e.IsActive == false);

            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(e => e.Id == parameters.Id.Value);

            if (parameters.HRMBranchId.HasValue && parameters.HRMBranchId.Value > 0)
                allEntities = allEntities.Where(e => e.HRMBranchId == parameters.HRMBranchId.Value);

            if (parameters.HRMDepartmentId.HasValue && parameters.HRMDepartmentId.Value > 0)
                allEntities = allEntities.Where(e => e.HRMDepartmentId == parameters.HRMDepartmentId.Value);

            if (parameters.HRMDesignationId.HasValue && parameters.HRMDesignationId.Value > 0)
                allEntities = allEntities.Where(e => e.HRMDesignationId == parameters.HRMDesignationId.Value);

            if (!string.IsNullOrWhiteSpace(parameters.FullName))
                allEntities = allEntities.Where(e => e.FullName.ToLower().Contains(parameters.FullName.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.EmployeeCode))
                allEntities = allEntities.Where(e => e.EmployeeCode.ToLower().Contains(parameters.EmployeeCode.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Email))
                allEntities = allEntities.Where(e => e.Email.ToLower().Contains(parameters.Email.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Gender))
                allEntities = allEntities.Where(e => e.Gender.ToLower() == parameters.Gender.ToLower());

            if (!string.IsNullOrWhiteSpace(parameters.EmploymentType))
                allEntities = allEntities.Where(e => e.EmploymentType.ToLower() == parameters.EmploymentType.ToLower());

            if (!string.IsNullOrWhiteSpace(parameters.EmployeeStatus))
                allEntities = allEntities.Where(e => e.EmployeeStatus.ToLower() == parameters.EmployeeStatus.ToLower());

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(e =>
                    e.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(e.FullName) && e.FullName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(e.EmployeeId) && e.EmployeeId.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(e.EmployeeCode) && e.EmployeeCode.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(e.Email) && e.Email.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(e.PhoneNumber) && e.PhoneNumber.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(e.BranchName) && e.BranchName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(e.DepartmentName) && e.DepartmentName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(e.DesignationName) && e.DesignationName.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "fullname" => parameters.SortDescending
                        ? allEntities.OrderByDescending(e => e.FullName)
                        : allEntities.OrderBy(e => e.FullName),
                    "employeeid" => parameters.SortDescending
                        ? allEntities.OrderByDescending(e => e.EmployeeId)
                        : allEntities.OrderBy(e => e.EmployeeId),
                    "email" => parameters.SortDescending
                        ? allEntities.OrderByDescending(e => e.Email)
                        : allEntities.OrderBy(e => e.Email),
                    "dateofjoining" => parameters.SortDescending
                        ? allEntities.OrderByDescending(e => e.DateOfJoining)
                        : allEntities.OrderBy(e => e.DateOfJoining),
                    "branchname" => parameters.SortDescending
                        ? allEntities.OrderByDescending(e => e.BranchName)
                        : allEntities.OrderBy(e => e.BranchName),
                    "departmentname" => parameters.SortDescending
                        ? allEntities.OrderByDescending(e => e.DepartmentName)
                        : allEntities.OrderBy(e => e.DepartmentName),
                    "createdat" => parameters.SortDescending
                        ? allEntities.OrderByDescending(e => e.CreatedAt)
                        : allEntities.OrderBy(e => e.CreatedAt),
                    _ => parameters.SortDescending
                        ? allEntities.OrderByDescending(e => e.Id)
                        : allEntities.OrderBy(e => e.Id)
                };
            }
            else
            {
                allEntities = allEntities.OrderByDescending(e => e.Id);
            }

            var totalRecords = allEntities.Count();
            var pagedData = allEntities
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            return new PagedResult<HRMEmployeeDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        public async Task<CustomApiResponse> UpdateProfilePicAsync(int Id, string ProfileImageSrc)
        {
            var employee = await _repo.GetByIdAsync(Id);
            if (employee == null || employee.IsDeleted)
                return new CustomApiResponse { IsSucess = false, Error = "Employee not found", StatusCode = 404 };

            employee.ProfileImagePath = ProfileImageSrc;
            _repo.Update(employee);
            await _repo.SaveChangesAsync();

            return new CustomApiResponse { IsSucess = true, Value = ProfileImageSrc, StatusCode = 200 };
        }

        private HRMEmployee CloneEntity(HRMEmployee entity) => new HRMEmployee
        {
            Id = entity.Id,
            FullName = entity.FullName,
            EmployeeId = entity.EmployeeId,
            EmployeeCode = entity.EmployeeCode,
            Email = entity.Email,
            PasswordHash = entity.PasswordHash,
            PhoneNumber = entity.PhoneNumber,
            DateOfBirth = entity.DateOfBirth,
            Gender = entity.Gender,
            ProfileImagePath = entity.ProfileImagePath,
            HRMBranchId = entity.HRMBranchId,
            HRMDepartmentId = entity.HRMDepartmentId,
            HRMDesignationId = entity.HRMDesignationId,
            DateOfJoining = entity.DateOfJoining,
            EmploymentType = entity.EmploymentType,
            EmployeeStatus = entity.EmployeeStatus,
            ShiftId = entity.ShiftId,
            AttendancePolicyId = entity.AttendancePolicyId,
            AddressLine1 = entity.AddressLine1,
            AddressLine2 = entity.AddressLine2,
            City = entity.City,
            State = entity.State,
            Country = entity.Country,
            PostalCode = entity.PostalCode,
            EmergencyContactName = entity.EmergencyContactName,
            EmergencyContactRelationship = entity.EmergencyContactRelationship,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}