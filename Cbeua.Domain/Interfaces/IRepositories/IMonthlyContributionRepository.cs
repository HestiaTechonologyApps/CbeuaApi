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
        Task AddContributionDetailAsync(ContributionDetail detail);
        Task AddContributionDetailsRangeAsync(List<ContributionDetail> details); 
        List<ContributionDetail> GetContributionDetailsByMasterId(long masterId);
        int GetContributionDetailsCountByMasterId(long masterId);               
        void RemoveContributionDetails(List<ContributionDetail> details);
        void RemoveContributionMaster(ContributionMaster master);
        void DetachAll();
    }
}