using Cbeua.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IContributionDetailService
    {
        Task<CustomApiResponse> GetByIdAsync(long detailId);
        Task<CustomApiResponse> ParkItemAsync(long detailId, string parkReason);
        Task<CustomApiResponse> ParkItemsAsync(long masterId, List<long> detailIds, string parkReason);
        Task<CustomApiResponse> ParkDiscrepanciesAsync(long masterId, string reportType, string parkReason);
        Task<CustomApiResponse> UnParkItemAsync(long detailId, int currentUserId);
    }
}
