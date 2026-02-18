using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMJobLocationService
    {
        Task<List<HRMJobLocationDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMJobLocationDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMJobLocationCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMJobLocationCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMJobLocationDTO>> GetPagedAsync(HRMJobLocationPaginationParams parameters);
    }
}