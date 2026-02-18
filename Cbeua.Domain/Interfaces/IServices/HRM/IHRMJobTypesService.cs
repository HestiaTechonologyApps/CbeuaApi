using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMJobTypeService
    {
        Task<List<HRMJobTypeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMJobTypeDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMJobTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMJobTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMJobTypeDTO>> GetPagedAsync(HRMJobTypePaginationParams parameters);
    }
}