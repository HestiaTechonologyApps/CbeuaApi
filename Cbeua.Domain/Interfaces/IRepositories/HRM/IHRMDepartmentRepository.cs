using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMDepartmentRepository : IGenericRepository<HRMDepartment>
    {
        Task<IQueryable<HRMDepartmentDTO>> GetQuerableList();
    }
}