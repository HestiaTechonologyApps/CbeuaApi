using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class DesignationService : IDesignationsService
    {
        private readonly IDesignationRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "DESIGNATION";

        public DesignationService(IDesignationRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<DesignationDTO>> GetAllAsync()
        {
            List<DesignationDTO> designationDTOs = new List<DesignationDTO>();
            var designations = await _repo.GetAllAsync();
            var activeDesignations = designations.Where(d => !d.IsDeleted).ToList();

            foreach (var designation in activeDesignations)
            {
                DesignationDTO designationDTO = await ConvertDesignationToDTO(designation);
                designationDTOs.Add(designationDTO);
            }
            return designationDTOs;
        }

        public async Task<DesignationDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null || q.IsDeleted) return null;
            var designationDTO = await ConvertDesignationToDTO(q);
            return designationDTO;
        }

        public async Task<DesignationDTO> CreateAsync(Designation designation)
        {
            designation.IsDeleted = false;
            await _repo.AddAsync(designation);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Designation>(
               tableName: AuditTableName,
               action: "create",
               recordId: designation.DesignationId,
               oldEntity: null,
               newEntity: designation,
               changedBy: "System"
            );
            return await ConvertDesignationToDTO(designation);
        }

        private async Task<DesignationDTO> ConvertDesignationToDTO(Designation designation)
        {
            DesignationDTO designationDTO = new DesignationDTO();
            designationDTO.DesignationId = designation.DesignationId;
            designationDTO.Name = designation.Name;
            designationDTO.Description = designation.Description;
            return designationDTO;
        }

        private Designation CloneDesignation(Designation designation)
        {
            return new Designation
            {
                DesignationId = designation.DesignationId,
                Name = designation.Name,
                Description = designation.Description,
                IsDeleted = designation.IsDeleted
            };
        }

        public async Task<bool> UpdateAsync(Designation designation)
        {
            var oldentity = await _repo.GetByIdAsync(designation.DesignationId);
            if (oldentity == null || oldentity.IsDeleted) return false;

            _repo.Detach(oldentity);
            _repo.Update(designation);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Designation>(
               tableName: AuditTableName,
               action: "update",
               recordId: designation.DesignationId,
               oldEntity: oldentity,
               newEntity: designation,
               changedBy: "System"
            );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var designation = await _repo.GetByIdAsync(id);
            if (designation == null || designation.IsDeleted) return false;

            var oldEntity = CloneDesignation(designation);

            designation.IsDeleted = true;
            _repo.Update(designation);

            await _auditRepository.LogAuditAsync<Designation>(
               tableName: AuditTableName,
               action: "delete",
               recordId: designation.DesignationId,
               oldEntity: oldEntity,
               newEntity: designation,
               changedBy: "System"
            );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<DesignationDTO>> GetPagedDesignationsAsync(DesignationPaginationParams parameters)
        {
            // Get all designations
            var allDesignations = await _repo.GetAllAsync();

            // Filter out deleted items
            IEnumerable<Designation> activeDesignations = allDesignations.Where(d => !d.IsDeleted);

            // Apply specific filters if provided
            if (parameters.DesignationId.HasValue)
            {
                activeDesignations = activeDesignations.Where(d => d.DesignationId == parameters.DesignationId.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                var nameLower = parameters.Name.ToLower().Trim();
                activeDesignations = activeDesignations.Where(d =>
                    !string.IsNullOrEmpty(d.Name) && d.Name.ToLower().Contains(nameLower));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Description))
            {
                var descLower = parameters.Description.ToLower().Trim();
                activeDesignations = activeDesignations.Where(d =>
                    !string.IsNullOrEmpty(d.Description) && d.Description.ToLower().Contains(descLower));
            }

            // Apply general search filter (searches across all fields)
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();

                activeDesignations = activeDesignations.Where(d =>
                    (d.DesignationId.ToString().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.Name) && d.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(d.Description) && d.Description.ToLower().Contains(searchLower))
                );
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                var sortBy = parameters.SortBy.ToLower();

                activeDesignations = sortBy switch
                {
                    "designationid" => parameters.SortDescending
                        ? activeDesignations.OrderByDescending(d => d.DesignationId)
                        : activeDesignations.OrderBy(d => d.DesignationId),
                    "name" => parameters.SortDescending
                        ? activeDesignations.OrderByDescending(d => d.Name)
                        : activeDesignations.OrderBy(d => d.Name),
                    "description" => parameters.SortDescending
                        ? activeDesignations.OrderByDescending(d => d.Description)
                        : activeDesignations.OrderBy(d => d.Description),
                    _ => parameters.SortDescending
                        ? activeDesignations.OrderByDescending(d => d.DesignationId)
                        : activeDesignations.OrderBy(d => d.DesignationId)
                };
            }
            else
            {
                // Default sort: Latest first
                activeDesignations = activeDesignations.OrderByDescending(d => d.DesignationId);
            }

            // Get total count before pagination
            var totalRecords = activeDesignations.Count();

            // Apply pagination
            var pageNumber = parameters.PageNumber;
            var pageSize = parameters.PageSize;

            var pagedData = activeDesignations
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Convert to DTOs
            var designationDTOs = new List<DesignationDTO>();
            foreach (var designation in pagedData)
            {
                designationDTOs.Add(await ConvertDesignationToDTO(designation));
            }

            return new PagedResult<DesignationDTO>
            {
                Data = designationDTOs,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}