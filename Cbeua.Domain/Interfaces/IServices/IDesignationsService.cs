using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IDesignationsService 
    {
        Task<List<DesignationDTO>> GetAllAsync();
        Task<DesignationDTO?> GetByIdAsync(int id);
        Task<DesignationDTO> CreateAsync(Designation coupon);
        Task<bool> UpdateAsync(Designation coupon);
        Task<bool> DeleteAsync(int id);
    }
}
