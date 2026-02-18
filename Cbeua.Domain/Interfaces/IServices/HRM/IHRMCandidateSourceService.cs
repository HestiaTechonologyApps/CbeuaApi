using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.HRMS;

namespace Cbeua.Domain.Interfaces.IServices.HRMS
{
    public interface IHRMCandidateSourceService
    {
        Task<List<HRMCandidateSourceDTO>> GetAllAsync(bool ShowDeleted = false, bool ShowInactive = true);
        Task<HRMCandidateSourceDTO?> GetByIdAsync(int id);
        Task<CustomApiResponse> CreateAsync(HRMCandidateSourceCreateUpdateDTO entitydto);
        Task<CustomApiResponse> UpdateAsync(HRMCandidateSourceCreateUpdateDTO entitydto);
        Task<CustomApiResponse> DeleteAsync(int id);
        Task<PagedResult<HRMCandidateSourceDTO>> GetPagedAsync(HRMCandidateSourcePaginationParams parameters);
    }
}