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
        Task<bool> UpdateAsync(MonthlyContribution monthlyContribution);
        Task<bool> DeleteAsync(long id);
        Task<CustomApiResponse> UploadContributionFileAsync(int monthCode, int yearOf, string fileName, string fileLocation, string fileType, string fileExtension, decimal fileSize);
    }
}