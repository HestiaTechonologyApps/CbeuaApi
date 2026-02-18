using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMSJobTypeRepository : IGenericRepository<HRMSJobType>
    {
        Task<IQueryable<HRMSJobTypeDTO>> GetQuerableList();
    }
}