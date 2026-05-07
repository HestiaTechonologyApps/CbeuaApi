using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System.Linq;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IMonthlyContributionRepository : IGenericRepository<MonthlyContribution>
    {
        IQueryable<MonthlyContributionDTO> GetQueryableMonthlyContributions();

        List<ContributionMaster> GetExistingContributionMasters(string month, string year);
        Task AddContributionMasterAsync(ContributionMaster master);
       Task<List<ContributionMasterListDTO>> GetAllContributionMasters();
        Task AddContributionDetailAsync(ContributionDetail detail);
        Task AddContributionDetailsRangeAsync(List<ContributionDetail> details); 
        List<ContributionDetail> GetContributionDetailsByMasterId(long masterId);
        IQueryable<ContributionDetail> GetContributionDetailsByMasterIdQueryable(long contributionMasterId);
        Task<int> GetNewMemberCountAsync(long contributionMasterId);
        int GetContributionDetailsCountByMasterId(long masterId);               
        void RemoveContributionDetails(List<ContributionDetail> details);
        void RemoveContributionMaster(ContributionMaster master);
        Task UpdateContributionMasterAsync(ContributionMaster master);
        Task<ContributionMaster?> GetContributionMasterByIdAsync(long contributionMasterId);
        void DetachAll();


        //reportss
        Task<List<ContributionDetail>> GetNewMembersAsync(long contributionMasterId);
        Task<List<ContributionDetail>> GetWrongBranchAsync(long contributionMasterId);
        Task<List<ContributionDetail>> GetWrongCircleAsync(long contributionMasterId);
        Task<List<ContributionDetail>> GetParkedItemsAsync(long contributionMasterId);
        Task<List<ContributionDetail>> GetAllDetailsAsync(long contributionMasterId);
        Task<List<DefaulterDTO>> GetDefaultersAsync(string month, string year);
    }
}