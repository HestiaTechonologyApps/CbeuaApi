using Cbeua.Domain.DTO.HRM;
using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMJobCategoryRepository : IGenericRepository<HRMSJobCategory>
    {
        Task<IQueryable<HRMJobCategoryDTO>> GetQuerableList();
    }
}