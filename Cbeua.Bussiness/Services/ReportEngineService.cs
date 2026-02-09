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
    public class ReportEngineService : IReportEngineService
    {
        private readonly IReportEngineRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public string AuditTableName { get; set; } = "REPORTENGINE";

        public ReportEngineService(
            IReportEngineRepository repo,
            IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<ReportEngineDTO>> GetAllAsync()
        {
            List<ReportEngineDTO> reportEngineDTOs = new List<ReportEngineDTO>();
            var reportEngines = await _repo.GetAllAsync();

            // Filter out deleted records in service layer
            var activeReportEngines = reportEngines.Where(x => !x.IsDeleted).ToList();

            foreach (var reportEngine in activeReportEngines)
            {
                ReportEngineDTO reportEngineDTO = await ConvertReportEngineToDTO(reportEngine);
                reportEngineDTOs.Add(reportEngineDTO);
            }

            return reportEngineDTOs;
        }

        public async Task<ReportEngineDTO?> GetByIdAsync(int id)
        {
            var reportEngine = await _repo.GetByIdAsync(id);

            // Check if deleted
            if (reportEngine == null || reportEngine.IsDeleted)
                return null;

            var reportEngineDTO = await ConvertReportEngineToDTO(reportEngine);
            return reportEngineDTO;
        }

        public async Task<ReportEngineDTO> CreateAsync(ReportEngine reportEngine)
        {
            reportEngine.CreatedDate = DateTime.Now;
            reportEngine.ModifiedDate = DateTime.Now;
            reportEngine.IsDeleted = false; // ✅ ENSURE NOT DELETED

            await _repo.AddAsync(reportEngine);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<ReportEngine>(
                tableName: AuditTableName,
                action: "create",
                recordId: reportEngine.ReportEngineId,
                oldEntity: null,
                newEntity: reportEngine,
                changedBy: "System" // Replace with actual user info
            );

            return await ConvertReportEngineToDTO(reportEngine);
        }

        private async Task<ReportEngineDTO> ConvertReportEngineToDTO(ReportEngine reportEngine)
        {
            ReportEngineDTO reportEngineDTO = new ReportEngineDTO
            {
                ReportEngineId = reportEngine.ReportEngineId,
                Name = reportEngine.Name,
                Description = reportEngine.Description,
                SQLString = reportEngine.SQLString,
                IsActive = reportEngine.IsActive,
                IsDeleted = reportEngine.IsDeleted,
                CreatedDate = reportEngine.CreatedDate,
                ModifiedDate = reportEngine.ModifiedDate,
                CreatedDateString = reportEngine.CreatedDate.HasValue
                    ? reportEngine.CreatedDate.Value.ToString("dd MMMM yyyy hh:mm tt")
                    : "",
                ModifiedDateString = reportEngine.ModifiedDate.HasValue
                    ? reportEngine.ModifiedDate.Value.ToString("dd MMMM yyyy hh:mm tt")
                    : ""
            };

            return reportEngineDTO;
        }

        // ✅ ADDED CLONE METHOD FOR AUDIT
        private ReportEngine CloneReportEngine(ReportEngine reportEngine)
        {
            return new ReportEngine
            {
                ReportEngineId = reportEngine.ReportEngineId,
                Name = reportEngine.Name,
                Description = reportEngine.Description,
                SQLString = reportEngine.SQLString,
                IsActive = reportEngine.IsActive,
                IsDeleted = reportEngine.IsDeleted,
                CreatedDate = reportEngine.CreatedDate,
                ModifiedDate = reportEngine.ModifiedDate
            };
        }

        public async Task<bool> UpdateAsync(ReportEngine reportEngine)
        {
            var oldEntity = await _repo.GetByIdAsync(reportEngine.ReportEngineId);
            if (oldEntity == null || oldEntity.IsDeleted) return false; // ✅ CHECK IF DELETED

            var oldEntityClone = CloneReportEngine(oldEntity); // ✅ CLONE FOR AUDIT

            reportEngine.ModifiedDate = DateTime.Now;
            reportEngine.CreatedDate = oldEntity.CreatedDate; // Preserve original creation date

            _repo.Detach(oldEntity);
            _repo.Update(reportEngine);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<ReportEngine>(
                tableName: AuditTableName,
                action: "update",
                recordId: reportEngine.ReportEngineId,
                oldEntity: oldEntityClone,
                newEntity: reportEngine,
                changedBy: "System" // Replace with actual user info
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reportEngine = await _repo.GetByIdAsync(id);
            if (reportEngine == null || reportEngine.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            var oldReportEngine = CloneReportEngine(reportEngine); // ✅ CLONE FOR AUDIT

            // ✅ SOFT DELETE - Set IsDeleted flag
            reportEngine.IsDeleted = true;
            reportEngine.IsActive = false; // Also mark as inactive
            reportEngine.ModifiedDate = DateTime.Now;

            _repo.Update(reportEngine);

            await _auditRepository.LogAuditAsync<ReportEngine>(
                tableName: AuditTableName,
                action: "delete",
                recordId: reportEngine.ReportEngineId,
                oldEntity: oldReportEngine,
                newEntity: reportEngine,
                changedBy: "System"
            );

            await _repo.SaveChangesAsync();
            return true;
        }
    }
}