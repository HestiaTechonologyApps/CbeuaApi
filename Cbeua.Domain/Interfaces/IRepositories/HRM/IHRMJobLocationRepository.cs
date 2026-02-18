using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMJobLocationRepository : IGenericRepository<HRMSJobLocation>
    {
        Task<IQueryable<HRMJobLocationDTO>> GetQuerableList();
    }
}