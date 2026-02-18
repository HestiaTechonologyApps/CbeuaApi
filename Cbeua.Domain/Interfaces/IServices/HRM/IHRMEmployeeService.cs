using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMEmployeeService
    {
        Task<List<HRMEmployeeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMEmployeeDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMEmployeeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMEmployeeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<CustomApiResponse> UpdateProfilePicAsync(int Id, string ProfileImageSrc);
        Task<PagedResult<HRMEmployeeDTO>> GetPagedAsync(HRMEmployeePaginationParams parameters);
    }
}