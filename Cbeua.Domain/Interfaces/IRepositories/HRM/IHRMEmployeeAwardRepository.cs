using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMEmployeeAwardRepository : IGenericRepository<HrmEmployeeAward>
    {
        Task<IQueryable<HRMEmployeeAwardDTO>> GetQuerableList();
    }
}