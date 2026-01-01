using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IMonthlyContributionRepository : IGenericRepository<Accounts>
    {
        Task<List<IndividualContrbutionDTO>> GetIndividualContrbution(int MemeberId);
    }
}
