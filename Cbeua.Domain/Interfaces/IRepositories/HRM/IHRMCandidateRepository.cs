using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMCandidateRepository : IGenericRepository<HRMSCandidate>
    {
        Task<IQueryable<HRMCandidateDTO>> GetQuerableList();
    }
}