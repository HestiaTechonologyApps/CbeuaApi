using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMDepartmentService
    {
        Task<List<HRMDepartmentDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMDepartmentDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMDepartmentCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMDepartmentCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMDepartmentDTO>> GetPagedAsync(HRMDepartmentPaginationParams parameters);
    }
}