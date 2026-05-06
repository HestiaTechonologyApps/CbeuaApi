using Cbeua.Domain.DTO;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IContributionMasterService
    {
        Task<CustomApiResponse> GetAllAsync();
        Task<CustomApiResponse> GetByIdAsync(long masterId);
       
        Task<CustomApiResponse> DeleteAsync(long masterId);
        Task<CustomApiResponse> ForwardAsync(long masterId);
        Task<CustomApiResponse> ApproveAsync(long masterId, int currentUserId, bool approve);
    }
}