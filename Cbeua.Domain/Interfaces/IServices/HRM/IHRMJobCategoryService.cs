using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRM;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMJobCategoryService
    {
        Task<List<HRMJobCategoryDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMJobCategoryDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMJobCategoryCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMJobCategoryCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMJobCategoryDTO>> GetPagedAsync(HRMJobCategoryPaginationParams parameters);
    }
}