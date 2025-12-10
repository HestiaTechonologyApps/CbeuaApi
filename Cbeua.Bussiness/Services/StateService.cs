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
    public class StateService : IStateService
    {
        private readonly IStateRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "STATE";
        public StateService(IStateRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<StateDTO>> GetAllAsync()
        {
            List<StateDTO> stateDTOs = new List<StateDTO>();
            var states = await _repo.GetAllAsync();

            foreach (var state in states)
            {
                StateDTO stateDTO = await ConvertStateToDTO(state);
                stateDTOs.Add(stateDTO);


            }

            return stateDTOs;
        }

        public async Task<StateDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var stateDTO = await ConvertStateToDTO(q);
            return stateDTO;
        }

        public async Task<StateDTO> CreateAsync(State state)
        {
            await _repo.AddAsync(state);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<State>(
               tableName: AuditTableName,
            action: "create",
               recordId: state.StateId,
            oldEntity: null,
               newEntity: state,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertStateToDTO(state);
        }

        private async Task<StateDTO> ConvertStateToDTO(State state)
        {
            StateDTO stateDTO = new StateDTO();
            stateDTO.StateId = state.StateId;
            stateDTO.Name = state.Name;
            stateDTO.Abbreviation = state.Abbreviation;
            stateDTO.IsActive = state.IsActive;
            return stateDTO;
        }

        public async Task<bool> UpdateAsync(State state)
        {
            var oldentity = await _repo.GetByIdAsync(state.StateId);
            _repo.Detach(oldentity);
            _repo.Update(state);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<State>(
               tableName: AuditTableName,
               action: "update",
               recordId: state.StateId,
               oldEntity: oldentity,
               newEntity: state,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var state = await _repo.GetByIdAsync(id);
            if (state == null) return false;
            _repo.Delete(state);
            await _auditRepository.LogAuditAsync<State>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: state.StateId,
               oldEntity: state,
               newEntity: state,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}