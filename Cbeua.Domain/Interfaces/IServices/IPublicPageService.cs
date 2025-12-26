using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IPublicPageService
    {
        Task<List<PublicPageDTO>> GetAllAsync();
        Task<PublicPageDTO?> GetByIdAsync(int id);
        Task<PublicPageDTO> CreateAsync(PublicPage publicPage);
        Task<bool> UpdateAsync(PublicPage publicPage);
        Task<bool> DeleteAsync(int id);
    }
}
