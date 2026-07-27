using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class UserRegistrationRepository : IUserRegistrationRepository
    {
        private readonly AppDbContext _context;
        public UserRegistrationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserRegistration registration)
        {
            await _context.UserRegistrations.AddAsync(registration);
        }

        public async Task<UserRegistration?> GetByIdAsync(int id)
        {
            return await _context.UserRegistrations
                .FirstOrDefaultAsync(r => r.UserRegistrationId == id);
        }

        public async Task<List<UserRegistration>> GetPendingAsync()
        {
            return await _context.UserRegistrations
                .Where(r => r.RegistrationStatus == "Pending")
                .OrderByDescending(r => r.RequestedDate)
                .ToListAsync();
        }

        public async Task<bool> AnyPendingStaffNoAsync(int staffNo)
        {
            return await _context.UserRegistrations
                .AnyAsync(r => r.StaffNo == staffNo && r.RegistrationStatus == "Pending");
        }

        public void Update(UserRegistration registration)
        {
            _context.UserRegistrations.Update(registration);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}