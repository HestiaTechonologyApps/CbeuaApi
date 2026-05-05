using Cbeua.Domain.DTO;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IContributionDetailService
    {
        Task<CustomApiResponse> GetByIdAsync(long detailId);
        Task<CustomApiResponse> ParkItemAsync(long detailId, string parkReason);
        Task<CustomApiResponse> UnParkItemAsync(long detailId, int currentUserId);
    }
}