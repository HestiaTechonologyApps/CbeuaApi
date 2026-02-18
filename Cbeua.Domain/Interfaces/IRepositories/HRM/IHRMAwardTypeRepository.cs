using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMAwardTypeRepository : IGenericRepository<HRMAwardType>
    {
        Task<IQueryable<HRMAwardTypeDTO>> GetQuerableList();
    }
}