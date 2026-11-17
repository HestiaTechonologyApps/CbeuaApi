using Cbeua.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IContributionDetailRepository
    {
        Task<ContributionDetail?> GetByIdAsync(long detailId);
        void Update(ContributionDetail detail);
        Task<bool> AccountsEntryExistsAsync(int memberId, int year, int month);
        Task AddAccountAsync(Accounts account);
        Task<int?> GetCircleIdByCircleCodeAsync(int circleCode);
        Task<int?> GetBranchIdByDpCodeAsync(int dpCode);
        Task<int?> GetMemberIdByStaffNoAsync(int staffNo);
        Task SaveChangesAsync();

        /// <summary>Status of the master, or null when the master does not exist.</summary>
        Task<string?> GetMasterStatusAsync(long masterId);

        /// <summary>
        /// Unparked details of the master that are discrepancies, grouped by
        /// NEWMEMBERS / WRONGBRANCH / WRONGCIRCLE (keys always present).
        /// </summary>
        Task<Dictionary<string, List<long>>> GetUnparkedDiscrepancyIdsAsync(long masterId);

        /// <summary>Number of details of the master that are currently parked.</summary>
        Task<int> CountParkedAsync(long masterId);

        /// <summary>Parks the given (not yet parked) details of the master; returns rows parked.</summary>
        Task<int> ParkDetailsAsync(long masterId, IEnumerable<long> detailIds, string parkReason);
    }
}
