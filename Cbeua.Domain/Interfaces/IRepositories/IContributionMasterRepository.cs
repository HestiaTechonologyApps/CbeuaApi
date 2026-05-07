using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IContributionMasterRepository
    {
        Task<List<ContributionMaster>> GetAllAsync();
        Task<ContributionMaster?> GetByIdAsync(long masterId);
        Task AddAsync(ContributionMaster master);
        void Update(ContributionMaster master);
        void Delete(ContributionMaster master);
        Task<List<AccountReadyDto>> GetDetailsWithLookupsAsync(long masterId);
        Task<List<ParkedDetailDto>> GetParkedDetailsAsync(long masterId);
        Task AddAccountsRangeAsync(List<Accounts> accounts);
        Task<int> AutoParkInvalidDetailsAsync(long masterId);
        Task<bool> ContributionExistsForMonthYearAsync(string month, string year);
        Task SaveChangesAsync();
    }
}