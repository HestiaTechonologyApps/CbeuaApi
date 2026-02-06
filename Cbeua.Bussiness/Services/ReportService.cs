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
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "REPORT";

        public ReportService(IReportRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<ReportDTO>> GetAllAsync()
        {
            return await _repo.QueryableReports().ToListAsync();
        }

        public async Task<ReportDTO?> GetByIdAsync(int id)
        {
            var q = _repo.QueryableReports();
            var report = await q.Where(r => r.ReportId == id).FirstOrDefaultAsync();
            return report;
        }

        public async Task<ReportDTO> CreateAsync(Report report)
        {
            await _repo.AddAsync(report);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<Report>(
               tableName: AuditTableName,
               action: "create",
               recordId: report.ReportId,
               oldEntity: null,
               newEntity: report,
               changedBy: "System" // Replace with actual user info
            );

            return await ConvertReportToDTO(report);
        }

        private async Task<ReportDTO> ConvertReportToDTO(Report report)
        {
            ReportDTO reportDTO = new ReportDTO();
            reportDTO.ReportId = report.ReportId;
            reportDTO.ReportType = report.ReportType;
            reportDTO.YearOf = report.YearOf;
            reportDTO.MonthCode = report.MonthCode;
            reportDTO.CircleId = report.CircleId;
            reportDTO.BranchId = report.BranchId;
            reportDTO.MemberId = report.MemberId;
            reportDTO.CreatedDate = report.CreatedDate;
            reportDTO.ModifiedDate = report.ModifiedDate;
            reportDTO.IsActive = report.IsActive;
            return reportDTO;
        }

        public async Task<bool> UpdateAsync(Report report)
        {
            var oldentity = await _repo.GetByIdAsync(report.ReportId);
            _repo.Detach(oldentity);
            _repo.Update(report);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<Report>(
               tableName: AuditTableName,
               action: "update",
               recordId: report.ReportId,
               oldEntity: oldentity,
               newEntity: report,
               changedBy: "System" // Replace with actual user info
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var report = await _repo.GetByIdAsync(id);
            if (report == null) return false;

            _repo.Delete(report);

            await _auditRepository.LogAuditAsync<Report>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: report.ReportId,
               oldEntity: report,
               newEntity: report,
               changedBy: "System" // Replace with actual user info
            );

            await _repo.SaveChangesAsync();
            return true;
        }
    }
}