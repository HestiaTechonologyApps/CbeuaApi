using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IBranchService
    {
        Task<List<BranchDTO>> GetAllAsync();
        Task<BranchDTO?> GetByIdAsync(int id);
        Task<BranchDTO> CreateAsync(Branch branch);
        Task<bool> UpdateAsync(Branch branch);
        Task<bool> DeleteAsync(int id);
    }
}
