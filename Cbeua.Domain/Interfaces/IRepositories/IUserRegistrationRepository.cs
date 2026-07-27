using Cbeua.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IUserRegistrationRepository
    {
        Task AddAsync(UserRegistration registration);
        Task<UserRegistration?> GetByIdAsync(int id);
        Task<List<UserRegistration>> GetPendingAsync();
        Task<List<UserRegistration>> GetAllAsync();
        Task<bool> AnyPendingStaffNoAsync(int staffNo);
        void Update(UserRegistration registration);
        Task SaveChangesAsync();
    }
}