using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IUserRoleRightService
    {
        Task<List<UserRoleRightDTO>> GetAllAsync();
        Task<UserRoleRightDTO?> GetByIdAsync(int id);
        Task<UserRoleRightDTO> CreateAsync(UserRoleRight coupon);
        Task<bool> UpdateAsync(UserRoleRight coupon);
        Task<bool> DeleteAsync(int id);
    }
}
