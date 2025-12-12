using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IDeathClaimService
    {
        Task<List<DeathClaimDTO>> GetAllAsync();
        Task<DeathClaimDTO?> GetByIdAsync(int id);
        Task<DeathClaimDTO> CreateAsync(DeathClaim deathClaim);
        Task<bool> UpdateAsync(DeathClaim deathClaim);
        Task<bool> DeleteAsync(int id);
    }
}
