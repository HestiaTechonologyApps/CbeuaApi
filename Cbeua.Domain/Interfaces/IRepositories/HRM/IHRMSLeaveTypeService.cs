using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMSLeaveTypeService
    {
        Task<List<HRMSLeaveTypeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMSLeaveTypeDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMSLeaveTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMSLeaveTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMSLeaveTypeDTO>> GetPagedAsync(HRMSLeaveTypePaginationParams parameters);
    }
}