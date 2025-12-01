using Cbeua.Domain.Entities;

namespace Cbeua.Bussiness.Services
{
    public class ExceptionLogService : IExceptionLogService
    {
        private readonly IExceptionLogRepository _repo;
        public ExceptionLogService(IExceptionLogRepository repo)
        {
            _repo = repo;
        }
        public async Task LogExceptionAsync(ExceptionLog log)
        {
            await _repo.AddAsync(log);
        }
    }

}