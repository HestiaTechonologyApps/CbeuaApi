using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IMainPageService 
    {
        Task<List<MainPageDTO>> GetAllAsync();
        Task<MainPageDTO?> GetByIdAsync(int id);
        Task<MainPageDTO> CreateAsync(MainPage coupon);
        Task<bool> UpdateAsync(MainPage coupon);
        Task<bool> DeleteAsync(int id);
    }
}
