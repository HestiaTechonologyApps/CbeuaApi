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
        Task<bool> DeleteAsync(long id);
        Task<CustomApiResponse> ReadContributionFileAsync(long monthlyContributionId);
        Task<CustomApiResponse> UploadContributionFileAsync(int monthCode, int yearOf, string fileName, string fileLocation, string fileType, string fileExtension, decimal fileSize);

        
        Task<CustomApiResponse> UploadAndSaveAsync(int monthCode, int yearOf, string fileName, string fileLocation, string fileType, string fileExtension, decimal fileSize);

        Task<CustomApiResponse> DeleteWithContributionDataAsync(long monthlyContributionId);
    }
}