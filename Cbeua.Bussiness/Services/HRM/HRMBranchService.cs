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
    public class HRMBranchService : IHRMBranchService
    {
        private readonly IHRMBranchRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public string AuditTableName { get; set; } = "HRM_Branch";

        public HRMBranchService(
            IHRMBranchRepository repo,
            IAuditRepository auditRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<List<HRMBranchDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true)
        {
            var entities = await _repo.GetQuerableList();

            if (!ShowDeleted)
                entities = entities.Where(b => b.IsDeleted == false);

            if (!ShowInactive)
                entities = entities.Where(b => b.IsActive == false);

            return await entities.ToListAsync();
        }

        public async Task<HRMBranchDTO?> GetByIdAsync(int id)
        {
            var entities = await _repo.GetQuerableList();
            var entity = await entities.Where(b => b.Id == id).FirstOrDefaultAsync();

            if (entity == null || entity.IsDeleted) return null;
            return entity;
        }

        public async Task<CustomApiResponse> CreateAsync(HRMBranchCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = new HRMBranch
                {
                    Name = entitydto.Name,
                    Address = entitydto.Address,
                    State = entitydto.State,
                    Country = entitydto.Country,
                    ZiPcode = entitydto.ZiPcode,
                    Phone = entitydto.Phone,
                    Email = entitydto.Email,
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

        public async Task<CustomApiResponse> UpdateAsync(HRMBranchCreateUpdateDTO entitydto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(entitydto.Id);
                if (entity == null || entity.IsDeleted)
                    return ApiResponseFactory.Fail(
                        "Branch not found or already deleted",
                        System.Net.HttpStatusCode.NotFound);

                var oldEntity = CloneEntity(entity);

                entity.Name = entitydto.Name;
                entity.Address = entitydto.Address;
                entity.State = entitydto.State;
                entity.Country = entitydto.Country;
                entity.ZiPcode = entitydto.ZiPcode;
                entity.Phone = entitydto.Phone;
                entity.Email = entitydto.Email;
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
                        "Branch not found or already deleted",
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

        public async Task<PagedResult<HRMBranchDTO>> GetPagedAsync(HRMBranchPaginationParams parameters)
        {
            var allEntities = await _repo.GetQuerableList();

            if (!parameters.ShowDeleted)
                allEntities = allEntities.Where(b => b.IsDeleted == false);

            if (!parameters.ShowInactive)
                allEntities = allEntities.Where(b => b.IsActive == false);

            // Specific filters
            if (parameters.Id.HasValue && parameters.Id.Value > 0)
                allEntities = allEntities.Where(b => b.Id == parameters.Id.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Name))
                allEntities = allEntities.Where(b => b.Name.ToLower().Contains(parameters.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.State))
                allEntities = allEntities.Where(b => b.State.ToLower().Contains(parameters.State.ToLower()));

            if (!string.IsNullOrWhiteSpace(parameters.Country))
                allEntities = allEntities.Where(b => b.Country.ToLower().Contains(parameters.Country.ToLower()));

            // General search
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();
                allEntities = allEntities.Where(b =>
                    b.Id.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(b.Name) && b.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.Address) && b.Address.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.State) && b.State.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.Country) && b.Country.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.Phone) && b.Phone.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.Email) && b.Email.ToLower().Contains(searchLower))
                );
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                allEntities = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.SortDescending
                        ? allEntities.OrderByDescending(b => b.Name)
                        : allEntities.OrderBy(b => b.Name),
                    "state" => parameters.SortDescending
                        ? allEntities.OrderByDescending(b => b.State)
                        : allEntities.OrderBy(b => b.State),
                    "country" => parameters.SortDescending
                        ? allEntities.OrderByDescending(b => b.Country)
                        : allEntities.OrderBy(b => b.Country),
                    "createdat" => parameters.SortDescending
                        ? allEntities.OrderByDescending(b => b.CreatedAt)
                        : allEntities.OrderBy(b => b.CreatedAt),
                    _ => parameters.SortDescending
                        ? allEntities.OrderByDescending(b => b.Id)
                        : allEntities.OrderBy(b => b.Id)
                };
            }
            else
            {
                allEntities = allEntities.OrderByDescending(b => b.Id);
            }

            var totalRecords = allEntities.Count();
            var pagedData = allEntities
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            return new PagedResult<HRMBranchDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        // ===== Helpers =====

        private HRMBranch CloneEntity(HRMBranch entity) => new HRMBranch
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            State = entity.State,
            Country = entity.Country,
            ZiPcode = entity.ZiPcode,
            Phone = entity.Phone,
            Email = entity.Email,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}