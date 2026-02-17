using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMDesignationService
    {
        Task<List<HRMDesignationDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMDesignationDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMDesignationCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMDesignationCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMDesignationDTO>> GetPagedAsync(HRMDesignationPaginationParams parameters);
    }
}