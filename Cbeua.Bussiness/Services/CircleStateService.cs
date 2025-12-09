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
    public class CircleStateService : ICircleStateService
    {
        private readonly ICircleStateRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public string AuditTableName = "CIRCLESTATE";
        public CircleStateService(ICircleStateRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<CircleStateDTO> CreateAsync(CircleState circleState)
        {
            await _repo.AddAsync(circleState);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<CircleState>(
               tableName: AuditTableName,
               action: "create",
               recordId: circleState.CircleId,
               oldEntity: null,
               newEntity: circleState,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertCircleStateToDTO(circleState);
        }

        private async Task<CircleStateDTO> ConvertCircleStateToDTO(CircleState circleState)
        {
            CircleStateDTO circleStateDTO = new CircleStateDTO();
            circleStateDTO.CircleId = circleState.CircleId;
            circleStateDTO.StateId = circleState.StateId;
            circleStateDTO.CreatedByUserId = circleState.CreatedByUserId;
            circleStateDTO.CreatedDate = circleState.CreatedDate;
            circleStateDTO.ModifiedDate = circleState.ModifiedDate;
            circleStateDTO.ModifiedByUserId = circleState.ModifiedByUserId;
            return circleStateDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var circleState = await _repo.GetByIdAsync(id);
            if (circleState == null) return false;
            _repo.Delete(circleState);
            await _auditRepository.LogAuditAsync<CircleState>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: circleState.CircleId,
               oldEntity: circleState,
               newEntity: circleState,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<CircleStateDTO>> GetAllAsync()
        {
            List<CircleStateDTO> circleStateDTOs = new List<CircleStateDTO>();

            var circleStates = await _repo.GetAllAsync();

            foreach (var circleState in circleStates)
            {
                CircleStateDTO circleStateDTO = await ConvertCircleStateToDTO(circleState);
                circleStateDTOs.Add(circleStateDTO);


            }

            return circleStateDTOs;
        }

        public async Task<CircleStateDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var circleStateDTO = await ConvertCircleStateToDTO(q);
            return circleStateDTO;
        }

        public async Task<bool> UpdateAsync(CircleState circleState)
        {
            var oldentity = await _repo.GetByIdAsync(circleState.CircleId);
            _repo.Detach(oldentity);
            _repo.Update(circleState);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<CircleState>(
               tableName: AuditTableName,
               action: "update",
               recordId: circleState.CircleId,
               oldEntity: oldentity,
               newEntity: circleState,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }
    }
}