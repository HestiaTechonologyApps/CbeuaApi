using Cbeua.Domain.Entities.Common;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IFinancialYearService
    {
        Task<List<FinancialYear>> GetAllAsync();
        Task<FinancialYear?> GetByIdAsync(int id);
        Task<FinancialYear> CreateAsync(FinancialYear coupon);
        Task<bool> UpdateAsync(FinancialYear coupon);
        Task<bool> DeleteAsync(int id);
    }


}
