using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMDesignationRepository : IGenericRepository<HRMDesignation>
    {
        Task<IQueryable<HRMDesignationDTO>> GetQuerableList();
    }
}