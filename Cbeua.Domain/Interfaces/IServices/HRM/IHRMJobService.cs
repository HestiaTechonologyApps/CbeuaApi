using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMJobService
    {
        Task<List<HRMJobDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMJobDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMJobCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMJobCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMJobDTO>> GetPagedAsync(HRMJobPaginationParams parameters);
    }
}