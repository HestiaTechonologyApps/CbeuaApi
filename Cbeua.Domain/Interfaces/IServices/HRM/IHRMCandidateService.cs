using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMCandidateService
    {
        Task<List<HRMCandidateDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMCandidateDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMCandidateCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMCandidateCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMCandidateDTO>> GetPagedAsync(HRMCandidatePaginationParams parameters);
    }
}