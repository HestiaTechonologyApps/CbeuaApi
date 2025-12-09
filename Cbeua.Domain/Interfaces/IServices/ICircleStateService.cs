using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface ICircleStateService
    {
        Task<List<CircleStateDTO>> GetAllAsync();
        Task<CircleStateDTO?> GetByIdAsync(int id);
        Task<CircleStateDTO> CreateAsync(CircleState coupon);
        Task<bool> UpdateAsync(CircleState coupon);
        Task<bool> DeleteAsync(int id);
    }
}
