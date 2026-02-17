using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMBranchRepository : IGenericRepository<HRMBranch>
    {
        Task<IQueryable<HRMBranchDTO>> GetQuerableList();
    }
}