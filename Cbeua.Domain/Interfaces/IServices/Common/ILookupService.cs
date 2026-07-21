using Cbeua.Domain.DTO;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface ILookupService
    {
        Task<CustomApiResponse> GetPagedLookupAsync(LookupPaginationParams parameters);
    }
}