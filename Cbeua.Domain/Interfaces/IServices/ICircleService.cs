using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface ICircleService
    {
        Task<List<CircleDTO>> GetAllAsync();
        Task<CircleDTO?> GetByIdAsync(int id);
        Task<CircleDTO> CreateAsync(Circle coupon);
        Task<bool> UpdateAsync(Circle coupon);
        Task<bool> DeleteAsync(int id);
    }
}
