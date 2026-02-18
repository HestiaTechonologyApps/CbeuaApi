using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMAwardTypeService
    {
        Task<List<HRMAwardTypeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMAwardTypeDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMAwardTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMAwardTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMAwardTypeDTO>> GetPagedAsync(HRMAwardTypePaginationParams parameters);
    }
}