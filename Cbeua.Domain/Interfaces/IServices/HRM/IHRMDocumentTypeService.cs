using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMDocumentTypeService
    {
        Task<List<HRMDocumentTypeDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMDocumentTypeDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMDocumentTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMDocumentTypeCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMDocumentTypeDTO>> GetPagedAsync(HRMDocumentTypePaginationParams parameters);
    }
}