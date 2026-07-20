using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "BRANCH";

        public BranchService(IBranchRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<BranchDTO>> GetAllAsync()
        {
            return _repo.GetQuerableBranch().ToList();
        }
        public async Task<List<CircleFilterStateDTO>> GetCirclesByStateAsync(int stateId)
        {
            return _repo.GetCirclesByState(stateId).ToList();
        }

        public async Task<BranchDTO?> GetByIdAsync(int id)
        {
            var q = _repo.GetQuerableBranch();
            var branch = q.Where(b => b.BranchId == id).FirstOrDefault();
            return branch;
        }

        public async Task<BranchDTO> CreateAsync(Branch branch)
        {
            branch.IsDeleted = false; // ✅ ENSURE NOT DELETED
            await _repo.AddAsync(branch);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<Branch>(
               tableName: AuditTableName,
               action: "create",
               recordId: branch.BranchId,
               oldEntity: null,
               newEntity: branch,
               changedBy: "System" // Replace with actual user info
            );

            return await ConvertBranchToDTO(branch);
        }

        private async Task<BranchDTO> ConvertBranchToDTO(Branch branch)
        {
            BranchDTO branchDTO = new BranchDTO();
            branchDTO.BranchId = branch.BranchId;
            branchDTO.DpCode = branch.DpCode;
            branchDTO.Name = branch.Name;
            branchDTO.Address1 = branch.Address1;
            branchDTO.Address2 = branch.Address2;
            branchDTO.Address3 = branch.Address3;
            branchDTO.District = branch.District;
            branchDTO.Status = branch.Status;
            branchDTO.CircleId = branch.CircleId;
            branchDTO.StateId = branch.StateId;
            branchDTO.IsRegCompleted = branch.IsRegCompleted;
            branchDTO.IsDeleted = branch.IsDeleted; // ✅ ADDED
            return branchDTO;
        }

        public async Task<bool> UpdateAsync(Branch branch)
        {
            var oldentity = await _repo.GetByIdAsync(branch.BranchId);
            if (oldentity == null || oldentity.IsDeleted) return false; // ✅ CHECK IF DELETED

            _repo.Detach(oldentity);
            _repo.Update(branch);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<Branch>(
               tableName: AuditTableName,
               action: "update",
               recordId: branch.BranchId,
               oldEntity: oldentity,
               newEntity: branch,
               changedBy: "System" // Replace with actual user info
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await _repo.GetByIdAsync(id);
            if (branch == null || branch.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            var oldEntity = CloneBranch(branch); // ✅ CLONE FOR AUDIT

            // ✅ SOFT DELETE
            branch.IsDeleted = true;
            _repo.Update(branch);

            await _auditRepository.LogAuditAsync<Branch>(
               tableName: AuditTableName,
               action: "delete",
               recordId: branch.BranchId,
               oldEntity: oldEntity,
               newEntity: branch,
               changedBy: "System" // Replace with actual user info
            );

            await _repo.SaveChangesAsync();
            return true;
        }

        // ✅ ADDED CLONE METHOD FOR AUDIT
        private Branch CloneBranch(Branch branch)
        {
            return new Branch
            {
                BranchId = branch.BranchId,
                DpCode = branch.DpCode,
                Name = branch.Name,
                Address1 = branch.Address1,
                Address2 = branch.Address2,
                Address3 = branch.Address3,
                District = branch.District,
                Status = branch.Status,
                CircleId = branch.CircleId,
                StateId = branch.StateId,
                IsRegCompleted = branch.IsRegCompleted,
                IsDeleted = branch.IsDeleted
            };
        }

        public async Task<PagedResult<BranchDTO>> GetPagedBranchesAsync(BranchPaginationParams parameters)
        {
            var query = _repo.GetQuerableBranch();

            if (parameters.BranchId.HasValue && parameters.BranchId.Value > 0)
                query = query.Where(b => b.BranchId == parameters.BranchId.Value);

            if (parameters.CircleId.HasValue && parameters.CircleId.Value > 0)
                query = query.Where(b => b.CircleId == parameters.CircleId.Value);

            if (parameters.StateId.HasValue && parameters.StateId.Value > 0)
                query = query.Where(b => b.StateId == parameters.StateId.Value);

            if (parameters.IsRegCompleted.HasValue)
                query = query.Where(b => b.IsRegCompleted == parameters.IsRegCompleted.Value);

            var allBranches = query.ToList();

            IEnumerable<BranchDTO> filteredBranches = allBranches;

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();

                filteredBranches = allBranches.Where(b =>
                    (b.BranchId.ToString().Contains(searchLower)) ||
                    (b.DpCode.ToString().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.Name) && b.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.District) && b.District.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.Status) && b.Status.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.CircleName) && b.CircleName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.StateName) && b.StateName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(b.Address1) && b.Address1.ToLower().Contains(searchLower))
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                var sortBy = parameters.SortBy.ToLower();

                filteredBranches = sortBy switch
                {
                    "branchid" => parameters.SortDescending
                        ? filteredBranches.OrderByDescending(b => b.BranchId)
                        : filteredBranches.OrderBy(b => b.BranchId),
                    "name" => parameters.SortDescending
                        ? filteredBranches.OrderByDescending(b => b.Name)
                        : filteredBranches.OrderBy(b => b.Name),
                    "dpcode" => parameters.SortDescending
                        ? filteredBranches.OrderByDescending(b => b.DpCode)
                        : filteredBranches.OrderBy(b => b.DpCode),
                    "district" => parameters.SortDescending
                        ? filteredBranches.OrderByDescending(b => b.District)
                        : filteredBranches.OrderBy(b => b.District),
                    "status" => parameters.SortDescending
                        ? filteredBranches.OrderByDescending(b => b.Status)
                        : filteredBranches.OrderBy(b => b.Status),
                    "circle" or "circlename" => parameters.SortDescending
                        ? filteredBranches.OrderByDescending(b => b.CircleName)
                        : filteredBranches.OrderBy(b => b.CircleName),
                    "state" or "statename" => parameters.SortDescending
                        ? filteredBranches.OrderByDescending(b => b.StateName)
                        : filteredBranches.OrderBy(b => b.StateName),
                    _ => parameters.SortDescending
                        ? filteredBranches.OrderByDescending(b => b.BranchId)
                        : filteredBranches.OrderBy(b => b.BranchId)
                };
            }
            else
            {
                filteredBranches = filteredBranches.OrderByDescending(b => b.BranchId);
            }

            var totalRecords = filteredBranches.Count();

            var pageNumber = parameters.PageNumber;
            var pageSize = parameters.PageSize;

            List<BranchDTO> pagedData;

            if (parameters.GetAll)
            {
                pagedData = filteredBranches.ToList();
            }
            else
            {
                pagedData = filteredBranches
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            return new PagedResult<BranchDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = parameters.GetAll ? totalRecords : pageSize
            };
        }
    }
}