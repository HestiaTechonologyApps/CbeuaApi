using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IStateService
    {
        Task<List<StateDTO>> GetAllAsync();
        Task<StateDTO?> GetByIdAsync(int id);
        Task<StateDTO> CreateAsync(State coupon);
        Task<bool> UpdateAsync(State coupon);
        Task<bool> DeleteAsync(int id);
    }
}
