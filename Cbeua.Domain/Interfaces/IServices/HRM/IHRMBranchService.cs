using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMBranchService
    {
        Task<List<HRMBranchDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMBranchDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMBranchCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMBranchCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMBranchDTO>> GetPagedAsync(HRMBranchPaginationParams parameters);
    }
}