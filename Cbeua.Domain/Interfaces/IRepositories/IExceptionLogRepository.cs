// File: CbeuaAPI/Repositories/IExceptionLogRepository.cs
using Cbeua.Domain.Entities;

public interface IExceptionLogRepository
{
    Task AddAsync(ExceptionLog log);
}

