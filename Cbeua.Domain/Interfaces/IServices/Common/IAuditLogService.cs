using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities.Common;

public interface IAuditLogService
{
    Task AddLogAsync(AuditLog log);
    Task<List<AuditLog>> GetAllLogsAsync();
    Task<AuditLog> GetLogByIdAsync(Guid logId);

    Task<List<AuditLogDTO>> GetAuditLogsForEntityAsync(string tableName, int recordId);
    Task LogAuditAsync<T>(
      string tableName,
      string action,
      int? recordId,
      T oldEntity,
      T newEntity,
      string changedBy
  ) where T : class;

  
}
