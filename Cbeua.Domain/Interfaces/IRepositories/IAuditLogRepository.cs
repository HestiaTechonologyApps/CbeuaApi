using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cbeua.Domain.Entities.Common;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);
    Task<List<AuditLog>> GetAllAsync();
    Task<AuditLog> GetByIdAsync(Guid logId);
}
