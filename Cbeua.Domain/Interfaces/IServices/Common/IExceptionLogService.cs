using Cbeua.Domain.Entities;

public interface IExceptionLogService
{
    Task LogExceptionAsync(ExceptionLog log);
}

