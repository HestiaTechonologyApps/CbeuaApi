using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMEmployeeAwardService
    {
        Task<List<HRMEmployeeAwardDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMEmployeeAwardDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMEmployeeAwardCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMEmployeeAwardCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMEmployeeAwardDTO>> GetPagedAsync(HRMEmployeeAwardPaginationParams parameters);
    }
}