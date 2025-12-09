using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IUserTypeService
    {
        Task<List<UserTypeDTO>> GetAllAsync();
        Task<UserTypeDTO?> GetByIdAsync(int id);
        Task<UserTypeDTO> CreateAsync(UserType coupon);
        Task<bool> UpdateAsync(UserType coupon);
        Task<bool> DeleteAsync(int id);
    }
}
