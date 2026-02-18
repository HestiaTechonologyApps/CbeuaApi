using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMJobRepository : IGenericRepository<HRMSJob>
    {
        Task<IQueryable<HRMJobDTO>> GetQuerableList();
    }
}