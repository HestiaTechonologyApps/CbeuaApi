using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMSJobTypeService
    {
        Task<List<HRMSJobTypeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMSJobTypeDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMSJobTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMSJobTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMSJobTypeDTO>> GetPagedAsync(HRMSJobTypePaginationParams parameters);
    }
}