// File: CbeuaAPI/Repositories/Implementations/ExceptionLogRepository.cs
using Cbeua.Domain.Entities;
using Cbeua.InfraCore.Data;

public class ExceptionLogRepository : IExceptionLogRepository
{
    private readonly AppDbContext _context;
    public ExceptionLogRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(ExceptionLog log)
    {
        await _context.ExceptionLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}