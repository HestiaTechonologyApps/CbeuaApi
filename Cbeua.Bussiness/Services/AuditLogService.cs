using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities.Common;


public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repo;
    private readonly IAuditRepository auditRepository;
    public AuditLogService(IAuditLogRepository repo, IAuditRepository auditRepository)
    {
        _repo = repo;
        this.auditRepository = auditRepository;
    }

    public async Task AddLogAsync(AuditLog log)
    {
        await _repo.AddAsync(log);
    }

    public async Task<List<AuditLog>> GetAllLogsAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<List<AuditLogDTO>> GetAuditLogsForEntityAsync(string tableName, int recordId)
    {
        return await auditRepository.GetAuditLogsForEntityAsync(tableName, recordId);
    }

    public async Task<AuditLog> GetLogByIdAsync(Guid logId)
    {
        return await _repo.GetByIdAsync(logId);
    }

    public async Task LogAuditAsync<T>(string tableName, string action, int? recordId, T oldEntity, T newEntity, string changedBy) where T : class
    {
        await auditRepository.LogAuditAsync(tableName, action, recordId, oldEntity, newEntity, changedBy);
    }
}
