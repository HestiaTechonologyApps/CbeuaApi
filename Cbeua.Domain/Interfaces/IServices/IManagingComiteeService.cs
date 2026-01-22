using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IManagingComiteeService 
    {
        Task<List<ManagingComiteeDTO>> GetAllAsync();
        Task<ManagingComiteeDTO?> GetByIdAsync(int id);
        Task<ManagingComiteeDTO> CreateAsync(ManagingComitee managingComitee);
        Task<bool> UpdateAsync(ManagingComitee managingComitee);
        Task<bool> DeleteAsync(int id);
        Task<CustomApiResponse> UpdateImageAsync(int managingComiteeId, string imageLocation);
    }
}
