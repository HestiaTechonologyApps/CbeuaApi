using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IYearMasterService
    {
        Task<List<YearMasterDTO>> GetAllAsync();
        Task<YearMasterDTO?> GetByIdAsync(int id);
        Task<YearMasterDTO> CreateAsync(YearMaster coupon);
        Task<bool> UpdateAsync(YearMaster coupon);
        Task<bool> DeleteAsync(int id);
    }
}
