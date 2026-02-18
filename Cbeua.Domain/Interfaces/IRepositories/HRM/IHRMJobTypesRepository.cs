using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMJobTypeRepository : IGenericRepository<HRMSJobType>
    {
        Task<IQueryable<HRMJobTypeDTO>> GetQuerableList();
    }
}