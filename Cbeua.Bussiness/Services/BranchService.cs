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
            List<BranchDTO> branchDTOs = new List<BranchDTO>();
            var branches = await _repo.GetAllAsync();

            foreach (var branch in branches)
            {
                BranchDTO branchDTO = await ConvertBranchToDTO(branch);
                branchDTOs.Add(branchDTO);


            }

            return branchDTOs;
        }

        public async Task<BranchDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var branchDTO = await ConvertBranchToDTO(q);
            return branchDTO;
        }

        public async Task<BranchDTO> CreateAsync(Branch branch)
        {
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
            return branchDTO;
        }

        public async Task<bool> UpdateAsync(Branch branch)
        {
            var oldentity = await _repo.GetByIdAsync(branch.BranchId);
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
            if (branch == null) return false;
            _repo.Delete(branch);
            await _auditRepository.LogAuditAsync<Branch>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: branch.BranchId,
               oldEntity: branch,
               newEntity: branch,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
