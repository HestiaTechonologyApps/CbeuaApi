using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
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
            return await _repo.QueryableCircles().ToListAsync();
        }

        public async Task<CircleDTO?> GetByIdAsync(int id)
        {
            var q = _repo.QueryableCircles();
            var circle = await q.Where(c => c.CircleId == id).FirstOrDefaultAsync();
            return circle;
        }

        public async Task<CircleDTO> CreateAsync(Circle circle)
        {
            circle.IsDeleted = false; // ✅ ENSURE NOT DELETED
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
            circleDTO.IsDeleted = circle.IsDeleted; // ✅ ADDED
            return circleDTO;
        }

        public async Task<bool> UpdateAsync(Circle circle)
        {
            var oldentity = await _repo.GetByIdAsync(circle.CircleId);
            if (oldentity == null || oldentity.IsDeleted) return false; // ✅ CHECK IF DELETED

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
            if (circle == null || circle.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            var oldEntity = CloneCircle(circle); // ✅ CLONE FOR AUDIT

            // ✅ SOFT DELETE
            circle.IsDeleted = true;
            _repo.Update(circle);

            await _auditRepository.LogAuditAsync<Circle>(
               tableName: AuditTableName,
               action: "delete",
               recordId: circle.CircleId,
               oldEntity: oldEntity,
               newEntity: circle,
               changedBy: "System" // Replace with actual user info
            );

            await _repo.SaveChangesAsync();
            return true;
        }

        // ✅ ADDED CLONE METHOD FOR AUDIT
        private Circle CloneCircle(Circle circle)
        {
            return new Circle
            {
                CircleId = circle.CircleId,
                CircleCode = circle.CircleCode,
                Name = circle.Name,
                Abbreviation = circle.Abbreviation,
                IsActive = circle.IsActive,
                StateId = circle.StateId,
                IsDeleted = circle.IsDeleted,
                DateFrom = circle.DateFrom,
                DateTo = circle.DateTo
            };
        }
    }
}