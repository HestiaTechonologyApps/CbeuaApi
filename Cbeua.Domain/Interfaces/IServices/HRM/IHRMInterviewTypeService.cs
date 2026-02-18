using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMInterviewTypeService
    {
        Task<List<HRMInterviewTypeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMInterviewTypeDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMInterviewTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMInterviewTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMInterviewTypeDTO>> GetPagedAsync(HRMInterviewTypePaginationParams parameters);
    }
}