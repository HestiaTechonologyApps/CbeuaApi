using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IDayQuoteService
    {
        Task<List<DayQuoteDTO>> GetAllAsync();
        Task<DayQuoteDTO?> GetByIdAsync(int id);
        Task<DayQuoteDTO> CreateAsync(DayQuote coupon);
        Task<bool> UpdateAsync(DayQuote coupon);
        Task<bool> DeleteAsync(int id);
    }
}
