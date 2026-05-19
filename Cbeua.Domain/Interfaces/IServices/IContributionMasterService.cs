using Cbeua.Domain.DTO;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IContributionMasterService
    {
        Task<CustomApiResponse> GetAllContributionMastersAsync();
        Task<CustomApiResponse> GetByIdAsync(long masterId);
        Task<CustomApiResponse> GetParkedDetailsAsync(long masterId, int pageNumber, int pageSize);
        Task<CustomApiResponse> DeleteAsync(long masterId);
        Task<CustomApiResponse> ForwardAsync(long masterId);
        Task<CustomApiResponse> ApproveAsync(long masterId, int currentUserId, bool approve);
    }
}