using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System.Linq;
using System.Threading;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IMonthlyContributionRepository : IGenericRepository<MonthlyContribution>
    {
        //(read-only preview, saves nothing)
        IQueryable<MonthlyContributionDTO> GetQueryableMonthlyContributions();

        //(read-only check: how many parked rows are also new members)
        Task<int> GetParkedNewMemberCountAsync(long id, CancellationToken ct = default);

        Task<(int Count, long Amount)> GetActiveTotalsAsync(long id, CancellationToken ct = default);

        List<ContributionMaster> GetExistingContributionMasters(string month, string year);
        Task AddContributionMasterAsync(ContributionMaster master);
        Task<List<ContributionMasterListDTO>> GetAllContributionMasters();
        Task AddContributionDetailAsync(ContributionDetail detail);
        Task AddContributionDetailsRangeAsync(List<ContributionDetail> details);
        List<ContributionDetail> GetContributionDetailsByMasterId(long masterId);
        IQueryable<ContributionDetail> GetContributionDetailsByMasterIdQueryable(long contributionMasterId);

        //Task<int> GetNewMemberCountAsync(long contributionMasterId);
        int GetContributionDetailsCountByMasterId(long masterId);
        void RemoveContributionDetails(List<ContributionDetail> details);
        void RemoveContributionMaster(ContributionMaster master);
        Task UpdateContributionMasterAsync(ContributionMaster master);
        Task<ContributionMaster?> GetContributionMasterByIdAsync(long contributionMasterId);
        void DetachAll();

        Task<(List<ContributionDetail>, int)> GetNewMembersPagedAsync(long id, int page, int size, CancellationToken ct);
        Task<(List<ContributionDetail>, int)> GetWrongBranchPagedAsync(long id, int page, int size, CancellationToken ct);
        Task<(List<ContributionDetail>, int)> GetWrongCirclePagedAsync(long id, int page, int size, CancellationToken ct);
        Task<(List<ContributionDetail>, int)> GetParkedPagedAsync(long id, int page, int size, CancellationToken ct);
        Task<(List<ContributionDetail>, int)> GetAllPagedAsync(long id, int page, int size, CancellationToken ct);
        Task<int> GetNewMemberCountAsync(long id, CancellationToken ct = default);
        void InvalidateReportCache(long masterId);

        Task RecalculateMasterTotalsAsync(long masterId, CancellationToken ct = default);//added line
        Task<int> GetParkedCountAsync(long id, CancellationToken ct = default);//added line
        void InvalidateMemberLookup();
        //reportss

        //Task<List<ContributionDetail>> GetNewMembersAsync(long contributionMasterId);
        //Task<List<ContributionDetail>> GetWrongBranchAsync(long contributionMasterId);
        //Task<List<ContributionDetail>> GetWrongCircleAsync(long contributionMasterId);
        //Task<List<ContributionDetail>> GetParkedItemsAsync(long contributionMasterId);
        //Task<List<ContributionDetail>> GetAllDetailsAsync(long contributionMasterId);
        Task<List<DefaulterDTO>> GetDefaultersAsync(string month, string year);

    }
}