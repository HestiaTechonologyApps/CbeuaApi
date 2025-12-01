using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;

namespace Cbeua.Bussiness.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repo;
        public CompanyService(ICompanyRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Company>> GetAllAsync() => (List<Company>)await _repo.GetAllAsync();

        public async Task<Company?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<Company> CreateAsync(Company company)
        {
            await _repo.AddAsync(company);
            await _repo.SaveChangesAsync();
            return company;
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            _repo.Update(company);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _repo.GetByIdAsync(id);
            if (company == null) return false;
            _repo.Delete(company);
            await _repo.SaveChangesAsync();
            return true;
        }
    }


}
