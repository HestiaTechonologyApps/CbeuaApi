using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IExpenseTypeService
    {
        Task<List<ExpenseTypeDTO>> GetAllAsync();
        Task<ExpenseTypeDTO?> GetByIdAsync(int id);
        Task<ExpenseTypeDTO> CreateAsync(ExpenseType expenseType);
        Task<bool> UpdateAsync(ExpenseType expenseType);
        Task<bool> DeleteAsync(int id);
    }
}
