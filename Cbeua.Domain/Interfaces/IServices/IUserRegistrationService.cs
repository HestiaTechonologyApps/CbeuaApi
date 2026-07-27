using Cbeua.Domain.DTO;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IUserRegistrationService
    {
        Task<CustomApiResponse> GetAllPendingAsync();
        Task<CustomApiResponse> GetAllAsync();
        Task<CustomApiResponse> GetByIdAsync(int id);
        Task<CustomApiResponse> ApproveAsync(int id, int currentUserId, bool approve, string? rejectReason);
    }
}