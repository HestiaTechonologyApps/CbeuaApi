using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.Entities;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface ICompanyService
    {
      
            Task<List<Company>> GetAllAsync();
            Task<Company?> GetByIdAsync(int id);
            Task<Company> CreateAsync(Company coupon);
            Task<bool> UpdateAsync(Company coupon);
            Task<bool> DeleteAsync(int id);
        
    }


}
