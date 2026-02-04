using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllAsync();
        Task<Company?> GetByIdAsync(int id);
        Task<Company> CreateAsync(Company company);
        Task<bool> UpdateAsync(Company company);
        Task<bool> DeleteAsync(int id);
        Task<CustomApiResponse> UpdateCompanyLogoAsync(int companyId, string companyLogoPath);
    }
}