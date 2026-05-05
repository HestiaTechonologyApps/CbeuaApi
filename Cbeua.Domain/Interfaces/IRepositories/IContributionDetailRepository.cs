using Cbeua.Domain.Entities;
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
    }
}