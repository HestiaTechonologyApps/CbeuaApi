using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IAccountService
    {
        Task<List<AccountsDTO>> GetAllAsync();
        Task<AccountsDTO?> GetByIdAsync(int id);
        Task<AccountsDTO> CreateAsync(Accounts accounts);
        Task<bool> UpdateAsync(Accounts accounts);
        Task<bool> DeleteAsync(int id);
    }
}
