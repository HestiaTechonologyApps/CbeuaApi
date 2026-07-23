using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IRefundContributionService
    {
        Task<List<RefundContributionDTO>> GetAllAsync();
        Task<RefundContributionDTO?> GetByIdAsync(int id);
        Task<RefundContributionDTO> CreateAsync(RefundContribution coupon);
        Task<List<RefundContributionDTO?>> GetByMemberIdAsync(int memberId);
        Task<CustomApiResponse> ApproveAsync(int id, int currentUserId, bool approve);
        Task<bool> UpdateAsync(RefundContribution coupon);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<RefundContributionDTO>> GetPagedRefundContributionsAsync(RefundContributionPaginationParams parameters);
    }
}
