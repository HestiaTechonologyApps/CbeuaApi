using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IStatusService
    {
        Task<List<StatusDTO>> GetAllAsync();
        Task<StatusDTO?> GetByIdAsync(int id);
        Task<StatusDTO> CreateAsync(Status coupon);
        Task<bool> UpdateAsync(Status coupon);
        Task<bool> DeleteAsync(int id);
    }
}
