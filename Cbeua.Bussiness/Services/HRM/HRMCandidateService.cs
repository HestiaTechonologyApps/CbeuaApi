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
    public class HRMCandidateService : IHRMCandidateService
    {
        private readonly IHRMCandidateRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRMS_Candidate";

        public HRMCandidateService(
            IHRMCandidateRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMCandidateDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();

            if (!ShowDeleted)
                entities = entities.Where(d => d.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(d => d.IsActive == false);

            return await entities.ToListAsync();
        }

        public async Task<HRMCandidateDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(d => d.Id == id).FirstOrDefaultAsync();

            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMCandidateCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMSCandidate
                {
                    Email = entitydto.Email,
                    Phone = entitydto.Phone,
                    Address = entitydto.Address,
                    City = entitydto.City,
                    State = entitydto.State,
                    Country = entitydto.Country,
                    ZipCode = entitydto.ZipCode,
                    ExperienceInYears = entitydto.ExperienceInYears,
                    CurrentSalary = entitydto.CurrentSalary,
                    ExpectedSalary = entitydto.ExpectedSalary,
                    NoticePeriod = entitydto.NoticePeriod,
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

        public async Task<CustomApiResponse> UpdateAsync(HRMCandidateCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Candidate not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.Email = entitydto.Email;
                entity.Phone = entitydto.Phone;
                entity.Address = entitydto.Address;
                entity.City = entitydto.City;
                entity.State = entitydto.State;
                entity.Country = entitydto.Country;
                entity.ZipCode = entitydto.ZipCode;
                entity.ExperienceInYears = entitydto.ExperienceInYears;
                entity.CurrentSalary = entitydto.CurrentSalary;
                entity.ExpectedSalary = entitydto.ExpectedSalary;
                entity.NoticePeriod = entitydto.NoticePeriod;
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
                        "Candidate not found or already deleted",
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

        public async Task<PagedResult<HRMCandidateDTO>> GetPagedAsync(HRMCandidatePaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(d => d.IsDeleted == false);
            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(d => d.IsActive == false);

            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(d => d.Id == parameters.Id.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Email))
                allEntities = allEntities.Where(d => d.Email.ToLower().Contains(parameters.Email.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Phone))
                allEntities = allEntities.Where(d => d.Phone.ToLower().Contains(parameters.Phone.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.City))
                allEntities = allEntities.Where(d => d.City.ToLower().Contains(parameters.City.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Country))
                allEntities = allEntities.Where(d => d.Country.ToLower().Contains(parameters.Country.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.NoticePeriod))
                allEntities = allEntities.Where(d => d.NoticePeriod.ToLower().Contains(parameters.NoticePeriod.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(d =>
                    d.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.Phone) && d.Phone.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.City) && d.City.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.Country) && d.Country.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.NoticePeriod) && d.NoticePeriod.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "email" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.Email)
                        : allEntities.OrderBy(d => d.Email),
                    "city" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.City)
                        : allEntities.OrderBy(d => d.City),
                    "country" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.Country)
                        : allEntities.OrderBy(d => d.Country),
                    "experienceinyears" => parameters.SortDescending
                        ? allEntities.OrderByDescending(d => d.ExperienceInYears)
                        : allEntities.OrderBy(d => d.ExperienceInYears),
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

            return new PagedResult<HRMCandidateDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        private HRMSCandidate CloneEntity(HRMSCandidate entity) => new HRMSCandidate
        {
            Id = entity.Id,
            Email = entity.Email,
            Phone = entity.Phone,
            Address = entity.Address,
            City = entity.City,
            State = entity.State,
            Country = entity.Country,
            ZipCode = entity.ZipCode,
            ExperienceInYears = entity.ExperienceInYears,
            CurrentSalary = entity.CurrentSalary,
            ExpectedSalary = entity.ExpectedSalary,
            NoticePeriod = entity.NoticePeriod,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}