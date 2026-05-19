using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IContributionMasterRepository
    {

        Task<List<ContributionMasterListDTO>> GetAllContributionMasters();
        Task<ContributionMaster?> GetById(long masterId);
        Task<ContributionMasterDTO?> GetByIdAsync(long masterId);
        Task AddAsync(ContributionMaster master);
        void Update(ContributionMaster master);
        void Delete(ContributionMaster master);
        Task<List<AccountReadyDto>> GetDetailsWithLookupsAsync(long masterId);
        Task<List<ParkedDetailDto>> GetParkedDetailsAsync(long masterId);
        Task AddAccountsRangeAsync(List<Accounts> accounts);
        Task<bool> ContributionExistsForMonthYearAsync(string month, string year);
        Task SaveChangesAsync();
    }
}