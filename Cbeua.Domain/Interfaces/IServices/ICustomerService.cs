using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.DTO;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAllAsync();
        Task<CustomerDTO?> GetByIdAsync(int id);
        Task<CustomerDTO> CreateAsync(Customer coupon);
        Task<bool> UpdateAsync(Customer coupon);
        Task<bool> DeleteAsync(int id);
    }
}
