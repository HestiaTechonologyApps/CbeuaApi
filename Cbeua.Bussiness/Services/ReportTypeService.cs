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
    public class ReportTypeService : IReportTypeService
    {
        private readonly IReportTypeRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "REPORTTYPE";

        public ReportTypeService(IReportTypeRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<ReportTypeDTO>> GetAllAsync()
        {
            return await _repo.QueryableReportTypes().ToListAsync();
        }

        public async Task<ReportTypeDTO?> GetByIdAsync(int id)
        {
            var q = _repo.QueryableReportTypes();
            var reportType = await q.Where(rt => rt.ReportTypeId == id).FirstOrDefaultAsync();
            return reportType;
        }

        public async Task<ReportTypeDTO> CreateAsync(ReportType reportType)
        {
            await _repo.AddAsync(reportType);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<ReportType>(
               tableName: AuditTableName,
               action: "create",
               recordId: reportType.ReportTypeId,
               oldEntity: null,
               newEntity: reportType,
               changedBy: "System" // Replace with actual user info
            );

            return await ConvertReportTypeToDTO(reportType);
        }

        private async Task<ReportTypeDTO> ConvertReportTypeToDTO(ReportType reportType)
        {
            ReportTypeDTO reportTypeDTO = new ReportTypeDTO();
            reportTypeDTO.ReportTypeId = reportType.ReportTypeId;
            reportTypeDTO.ReportTypeName = reportType.ReportTypeName;
            reportTypeDTO.Description = reportType.Description;
            reportTypeDTO.IsActive = reportType.IsActive;
            reportTypeDTO.CreatedDate = reportType.CreatedDate;
            reportTypeDTO.ModifiedDate = reportType.ModifiedDate;
            return reportTypeDTO;
        }

        public async Task<bool> UpdateAsync(ReportType reportType)
        {
            var oldentity = await _repo.GetByIdAsync(reportType.ReportTypeId);
            _repo.Detach(oldentity);
            _repo.Update(reportType);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<ReportType>(
               tableName: AuditTableName,
               action: "update",
               recordId: reportType.ReportTypeId,
               oldEntity: oldentity,
               newEntity: reportType,
               changedBy: "System" // Replace with actual user info
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reportType = await _repo.GetByIdAsync(id);
            if (reportType == null) return false;

            _repo.Delete(reportType);

            await _auditRepository.LogAuditAsync<ReportType>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: reportType.ReportTypeId,
               oldEntity: reportType,
               newEntity: reportType,
               changedBy: "System" // Replace with actual user info
            );

            await _repo.SaveChangesAsync();
            return true;
        }
    }
}