using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMCandidateSourceRepository : IGenericRepository<HRMSCandidateSource>
    {
        Task<IQueryable<HRMCandidateSourceDTO>> GetQuerableList();
    }
}