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
    public class HRMJobLocationService : IHRMJobLocationService
    {
        private readonly IHRMJobLocationRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRM_JobLocations";

        public HRMJobLocationService(
            IHRMJobLocationRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMJobLocationDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();
            if (!ShowDeleted)
                entities = entities.Where(a => a.IsDeleted == false);
            if (!ShowInactive)
                entities = entities.Where(a => a.IsActive == false);
            return await entities.ToListAsync();
        }

        public async Task<HRMJobLocationDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMJobLocationCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMSJobLocation
                {
                    Name = entitydto.Name,
                    Address = entitydto.Address,
                    IsRemote = entitydto.IsRemote,
                    City = entitydto.City,
                    State = entitydto.State,
                    Country = entitydto.Country,
                    PostalCode = entitydto.PostalCode,
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

        public async Task<CustomApiResponse> UpdateAsync(HRMJobLocationCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Job Location not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.Name = entitydto.Name;
                entity.Address = entitydto.Address;
                entity.IsRemote = entitydto.IsRemote;
                entity.City = entitydto.City;
                entity.State = entitydto.State;
                entity.Country = entitydto.Country;
                entity.PostalCode = entitydto.PostalCode;
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
                        "Job Location not found or already deleted",
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

        public async Task<PagedResult<HRMJobLocationDTO>> GetPagedAsync(HRMJobLocationPaginationParams parameters)
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

            if (!string.IsNullOrWhiteSpace(parameters.City))
                allEntities = allEntities.Where(a => a.City.ToLower().Contains(parameters.City.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.State))
                allEntities = allEntities.Where(a => a.State.ToLower().Contains(parameters.State.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Country))
                allEntities = allEntities.Where(a => a.Country.ToLower().Contains(parameters.Country.ToLower()));

            if (parameters.IsRemote.HasValue)
                allEntities = allEntities.Where(a => a.IsRemote == parameters.IsRemote.Value);

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(a =>
                    a.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(a.Name) && a.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.City) && a.City.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.State) && a.State.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.Country) && a.Country.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(a.PostalCode) && a.PostalCode.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.Name)
                        : allEntities.OrderBy(a => a.Name),
                    "city" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.City)
                        : allEntities.OrderBy(a => a.City),
                    "state" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.State)
                        : allEntities.OrderBy(a => a.State),
                    "country" => parameters.SortDescending
                        ? allEntities.OrderByDescending(a => a.Country)
                        : allEntities.OrderBy(a => a.Country),
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

            return new PagedResult<HRMJobLocationDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        private HRMSJobLocation CloneEntity(HRMSJobLocation entity) => new HRMSJobLocation
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            IsRemote = entity.IsRemote,
            City = entity.City,
            State = entity.State,
            Country = entity.Country,
            PostalCode = entity.PostalCode,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}