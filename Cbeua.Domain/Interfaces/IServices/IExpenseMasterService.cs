using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IExpenseMasterService
    {
        Task<List<ExpenseMasterDTO>> GetAllAsync();
        Task<ExpenseMasterDTO?> GetByIdAsync(int id);
        Task<ExpenseMasterDTO> CreateAsync(ExpenseMaster expenseMaster);
        Task<bool> UpdateAsync(ExpenseMaster expenseMaster);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<ExpenseMasterDTO>> GetPagedExpenseMastersAsync(ExpenseMasterPaginationParams parameters);
    }
}
