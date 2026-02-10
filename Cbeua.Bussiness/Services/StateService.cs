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
            // ✅ GET ONLY NON-DELETED STATES
            var states = await _repo.GetAllActiveAsync();
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
            if (q == null || q.IsDeleted) return null; // ✅ CHECK IF DELETED
            var stateDTO = await ConvertStateToDTO(q);
            return stateDTO;
        }

        public async Task<StateDTO> CreateAsync(State state)
        {
            // ✅ TRIM INPUT
            state.Name = state.Name?.Trim() ?? "";
            state.Abbreviation = state.Abbreviation?.Trim() ?? "";

            // ✅ VALIDATE: Check for duplicate name
            if (await _repo.ExistsByNameAsync(state.Name))
            {
                throw new InvalidOperationException($"A state with the name '{state.Name}' already exists.");
            }

            // ✅ VALIDATE: Check for duplicate abbreviation
            if (await _repo.ExistsByAbbreviationAsync(state.Abbreviation))
            {
                throw new InvalidOperationException($"A state with the abbreviation '{state.Abbreviation}' already exists.");
            }

            state.IsDeleted = false; // ✅ ENSURE NOT DELETED
            await _repo.AddAsync(state);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<State>(
               tableName: AuditTableName,
               action: "create",
               recordId: state.StateId,
               oldEntity: null,
               newEntity: state,
               changedBy: "System" // Replace with actual user if available
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
            stateDTO.IsDeleted = state.IsDeleted; // ✅ ADDED
            return stateDTO;
        }

        // ✅ ADDED CLONE METHOD FOR AUDIT
        private State CloneState(State state)
        {
            return new State
            {
                StateId = state.StateId,
                Name = state.Name,
                Abbreviation = state.Abbreviation,
                IsActive = state.IsActive,
                IsDeleted = state.IsDeleted
            };
        }

        public async Task<bool> UpdateAsync(State state)
        {
            var oldEntity = await _repo.GetByIdAsync(state.StateId);
            if (oldEntity == null || oldEntity.IsDeleted) return false; // ✅ CHECK IF DELETED

            // ✅ TRIM INPUT
            state.Name = state.Name?.Trim() ?? "";
            state.Abbreviation = state.Abbreviation?.Trim() ?? "";

            // ✅ VALIDATE: Check for duplicate name (excluding current state)
            if (await _repo.ExistsByNameAsync(state.Name, state.StateId))
            {
                throw new InvalidOperationException($"A state with the name '{state.Name}' already exists.");
            }

            // ✅ VALIDATE: Check for duplicate abbreviation (excluding current state)
            if (await _repo.ExistsByAbbreviationAsync(state.Abbreviation, state.StateId))
            {
                throw new InvalidOperationException($"A state with the abbreviation '{state.Abbreviation}' already exists.");
            }

            // ✅ CLONE FOR AUDIT
            var oldStateClone = CloneState(oldEntity);

            // Update fields
            oldEntity.Name = state.Name;
            oldEntity.Abbreviation = state.Abbreviation;
            oldEntity.IsActive = state.IsActive;

            _repo.Update(oldEntity);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<State>(
               tableName: AuditTableName,
               action: "update",
               recordId: oldEntity.StateId,
               oldEntity: oldStateClone,
               newEntity: oldEntity,
               changedBy: "System" // Replace with actual user if available
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var state = await _repo.GetByIdAsync(id);
            if (state == null || state.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            // ✅ CLONE FOR AUDIT
            var oldState = CloneState(state);

            // ✅ SOFT DELETE
            state.IsDeleted = true;
            _repo.Update(state);

            await _auditRepository.LogAuditAsync<State>(
               tableName: AuditTableName,
               action: "delete",
               recordId: state.StateId,
               oldEntity: oldState,
               newEntity: state,
               changedBy: "System" // Replace with actual user if available
            );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}