using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;

public interface IAuditRepository
{
    Task LogAuditAsync<T>(
        string tableName,
        string action,
        int? recordId,
        T oldEntity,
        T newEntity,
        string changedBy
    ) where T : class;

    Task<List<AuditLogDTO>> GetAuditLogsForEntityAsync(string tableName, int recordId);
    //Task LogAuditAsync<T>(string tableName, string action, int recordId, object oldEntity, ExpenseType newEntity, string changedBy);
}