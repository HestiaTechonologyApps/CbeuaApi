using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;


namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<UserDTO> GetUserDetailsAsync(int userId);
        Task<List<UserListDTO>> GetUsersAsync();
        IQueryable<UserListDTO> QueryableUserList();

        Task AddLoginLogAsync(UserLoginLog log);
        Task<List<UserLoginLogDTO>> GetUserLogsAsync(int userId);
    }
}
