using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMSLeaveApplicationService
    {
        Task<List<HRMSLeaveApplicationDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMSLeaveApplicationDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMSLeaveApplicationCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMSLeaveApplicationCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMSLeaveApplicationDTO>> GetPagedAsync(HRMSLeaveApplicationPaginationParams parameters);
    }
}