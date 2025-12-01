using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;


namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IUserService
    {
        // Task<List<UserDTO>> GetAllAsync();
        Task<List<UserListDTO>> GetAllAsync();
        Task<UserDTO?> GetByIdAsync(int id);
        Task<UserDTO> CreateAsync(User coupon);
        Task<bool> UpdateAsync(User coupon);
        Task<bool> DeleteAsync(int id);

      // Task<List<UserLoginLogDTO>> GetUserLogsAsync(int userId);
        Task<CustomApiResponse> ChangePassWord(PasswordChangeRequest passwordChangeRequest);
    }
}
