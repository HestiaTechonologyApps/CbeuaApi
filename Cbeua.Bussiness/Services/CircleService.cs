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
    public class CircleService : ICircleService
    {
        private readonly ICircleRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "CIRCLE";
        public CircleService(ICircleRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<CircleDTO>> GetAllAsync()
        {
            List<CircleDTO> circleDTOs = new List<CircleDTO>();
            var circles = await _repo.GetAllAsync();

            foreach (var circle in circles)
            {
                CircleDTO circleDTO = await ConvertCircleToDTO(circle);
                circleDTOs.Add(circleDTO);


            }

            return circleDTOs;
        }

        public async Task<CircleDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var circleDTO = await ConvertCircleToDTO(q);
            return circleDTO;
        }

        public async Task<CircleDTO> CreateAsync(Circle circle)
        {
            await _repo.AddAsync(circle);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Circle>(
               tableName: AuditTableName,
               action: "create",
               recordId: circle.CircleId,
               oldEntity: null,
               newEntity: circle,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertCircleToDTO(circle);
        }

        private async Task<CircleDTO> ConvertCircleToDTO(Circle circle)
        {
            CircleDTO circleDTO = new CircleDTO();
            circleDTO.CircleId = circle.CircleId;
            circleDTO.CircleCode = circle.CircleCode;
            circleDTO.Name = circle.Name;
            circleDTO.Abbreviation = circle.Abbreviation;
            circleDTO.IsActive = circle.IsActive;
            circleDTO.StateId = circle.StateId;
            circleDTO.DateFrom = circle.DateFrom;
            circleDTO.DateTo = circle.DateTo;
            return circleDTO;
        }

        public async Task<bool> UpdateAsync(Circle circle)
        {
            var oldentity = await _repo.GetByIdAsync(circle.CircleId);
            _repo.Detach(oldentity);
            _repo.Update(circle);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Circle>(
               tableName: AuditTableName,
               action: "update",
               recordId: circle.CircleId,
               oldEntity: oldentity,
               newEntity: circle,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var circle = await _repo.GetByIdAsync(id);
            if (circle == null) return false;
            _repo.Delete(circle);
            await _auditRepository.LogAuditAsync<Circle>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: circle.CircleId,
               oldEntity: circle,
               newEntity: circle,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
