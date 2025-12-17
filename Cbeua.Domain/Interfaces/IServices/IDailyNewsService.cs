using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IDailyNewsService
    {
        Task<List<DailyNewsDTO>> GetAllAsync();
        Task<DailyNewsDTO?> GetByIdAsync(int id);
        Task<DailyNewsDTO> CreateAsync(DailyNews dailyNews);
        Task<bool> UpdateAsync(DailyNews dailyNews);
        Task<bool> DeleteAsync(int id);
    }
}
