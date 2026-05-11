using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IMonthlyContributionService
    {
        Task<List<MonthlyContributionDTO>> GetAllAsync();
        Task<MonthlyContributionDTO?> GetByIdAsync(long id);
        Task<MonthlyContributionDTO> CreateAsync(MonthlyContribution monthlyContribution);
        Task<CustomApiResponse> SaveContributionAsync(long monthlyContributionId);

        Task<bool> UpdateAsync(MonthlyContribution monthlyContribution);
        Task<CustomApiResponse> GetAllContributionMastersAsync();
        Task<bool> DeleteAsync(long id);
        Task<CustomApiResponse> ReadContributionFileAsync(long monthlyContributionId);
        Task<CustomApiResponse> UploadContributionFileAsync(int monthCode, int yearOf, string fileName, string fileLocation, string fileType, string fileExtension, decimal fileSize);

        Task<CustomApiResponse> UpdateContributionFileAsync(long contributionMasterId, int monthCode, int yearOf,string fileName, string fileLocation,string fileType, string fileExtension,
        decimal fileSize);
        Task<CustomApiResponse> UploadAndSaveAsync(int monthCode, int yearOf, string fileName, string fileLocation, string fileType, string fileExtension, decimal fileSize);

        Task<CustomApiResponse> DeleteWithContributionDataAsync(long monthlyContributionId);
        Task<PagedResult<ContributionDetail>> GetPagedContributionDetailsAsync(long monthlyContributionId,ContributionDetailPaginationParams p);
        Task<CustomApiResponse> GetContributionReportAsync(long contributionMasterId, string reportType, int pageNumber,
      int pageSize);
    }
}