using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IMonthService
    {
        Task<List<MonthDTO>> GetAllAsync();
        Task<MonthDTO?> GetByIdAsync(int id);
        Task<MonthDTO> CreateAsync(Month month);
        Task<bool> UpdateAsync(Month month);
        Task<bool> DeleteAsync(int id);
    }
}
