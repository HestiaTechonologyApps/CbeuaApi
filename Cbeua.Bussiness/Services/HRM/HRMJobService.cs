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
    public class HRMJobService : IHRMJobService
    {
        private readonly IHRMJobRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRM_Jobs";

        public HRMJobService(
            IHRMJobRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMJobDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();
            if (!ShowDeleted)
                entities = entities.Where(a => a.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(a => a.IsActive == false);
            return await entities.ToListAsync();
        }

        public async Task<HRMJobDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMJobCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMSJob
                {
                    JobTitle = entitydto.JobTitle,
                    Location = entitydto.Location,
                    Branch = entitydto.Branch,
                    Department = entitydto.Department,
                    StartDate = entitydto.StartDate,
                    EndDate = entitydto.EndDate,
                    ExistingLink = entitydto.ExistingLink,
                    NumberOfOpenings = entitydto.NumberOfOpenings,
                    MinimumExperienceYears = entitydto.MinimumExperienceYears,
                    MaximumExperienceYears = entitydto.MaximumExperienceYears,
                    MinimumSalary = entitydto.MinimumSalary,
                    MaximumSalary = entitydto.MaximumSalary,
                    JobDescription = entitydto.JobDescription,
                    JobRequrement = entitydto.JobRequrement,
                    JobBenefits = entitydto.JobBenefits,
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

        public async Task<CustomApiResponse> UpdateAsync(HRMJobCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Job not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.JobTitle = entitydto.JobTitle;
                entity.Location = entitydto.Location;
                entity.Branch = entitydto.Branch;
                entity.Department = entitydto.Department;
                entity.StartDate = entitydto.StartDate;
                entity.EndDate = entitydto.EndDate;
                entity.ExistingLink = entitydto.ExistingLink;
                entity.NumberOfOpenings = entitydto.NumberOfOpenings;
                entity.MinimumExperienceYears = entitydto.MinimumExperienceYears;
                entity.MaximumExperienceYears = entitydto.MaximumExperienceYears;
                entity.MinimumSalary = entitydto.MinimumSalary;
                entity.MaximumSalary = entitydto.MaximumSalary;
                entity.JobDescription = entitydto.JobDescription;
                entity.JobRequrement = entitydto.JobRequrement;
                entity.JobBenefits = entitydto.JobBenefits;
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
                        "Job not found or already deleted",
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

        public async Task<PagedResult<HRMJobDTO>> GetPagedAsync(HRMJobPaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(a => a.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(a => a.IsActive == false);

            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(a => a.Id == parameters.Id.Value);

            if (!string.IsNullOrWhiteSpace(parameters.JobTitle))
                allEntities = allEntities.Where(a => a.JobTitle.ToLower().Contains(parameters.JobTitle.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Location))
                allEntities = allEntities.Where(a => a.Location.ToLower().Contains(parameters.Location.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Branch))
                allEntities = allEntities.Where(a => a.Branch.ToLower().Contains(parameters.Branch.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Department))
                allEntities = allEntities.Where(a => a.Department.ToLower().Contains(parameters.Department.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(a =>
                    a.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(a.JobTitle) && a.JobTitle.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.Location) && a.Location.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.Branch) && a.Branch.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.Department) && a.Department.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "jobtitle" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.JobTitle)
                        : allEntities.OrderBy(a => a.JobTitle),
                    "location" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.Location)
                        : allEntities.OrderBy(a => a.Location),
                    "branch" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.Branch)
                        : allEntities.OrderBy(a => a.Branch),
                    "department" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.Department)
                        : allEntities.OrderBy(a => a.Department),
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

            return new PagedResult<HRMJobDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        private HRMSJob CloneEntity(HRMSJob entity) => new HRMSJob
        {
            Id = entity.Id,
            JobTitle = entity.JobTitle,
            Location = entity.Location,
            Branch = entity.Branch,
            Department = entity.Department,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            ExistingLink = entity.ExistingLink,
            NumberOfOpenings = entity.NumberOfOpenings,
            MinimumExperienceYears = entity.MinimumExperienceYears,
            MaximumExperienceYears = entity.MaximumExperienceYears,
            MinimumSalary = entity.MinimumSalary,
            MaximumSalary = entity.MaximumSalary,
            JobDescription = entity.JobDescription,
            JobRequrement = entity.JobRequrement,
            JobBenefits = entity.JobBenefits,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}