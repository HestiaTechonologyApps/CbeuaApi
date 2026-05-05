using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class ContributionDetailRepository : IContributionDetailRepository
    {
        private readonly AppDbContext _context;

        public ContributionDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ContributionDetail?> GetByIdAsync(long detailId)
        {
            return await _context.ContributionDetails
                .FirstOrDefaultAsync(d => d.ContributionDetailId == detailId);
        }

        public void Update(ContributionDetail detail)
        {
            _context.ContributionDetails.Update(detail);
        }

        public async Task<bool> AccountsEntryExistsAsync(int memberId, int year, int month)
        {
            return await _context.Accounts
                .AnyAsync(a => a.MemeberId == memberId
                            && a.YearOf == year
                            && a.MonthCode == month);
        }

        public async Task AddAccountAsync(Accounts account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public async Task<int?> GetCircleIdByCircleCodeAsync(int circleCode)
        {
            return await _context.Circles
                .Where(c => c.CircleCode == circleCode && !c.IsDeleted)
                .Select(c => (int?)c.CircleId)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetBranchIdByDpCodeAsync(int dpCode)
        {
            return await _context.Branches
                .Where(b => b.DpCode == dpCode && !b.IsDeleted)
                .Select(b => (int?)b.BranchId)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetMemberIdByStaffNoAsync(int staffNo)
        {
            return await _context.Members
                .Where(m => m.StaffNo == staffNo && !m.IsDeleted)
                .Select(m => (int?)m.MemberId)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}