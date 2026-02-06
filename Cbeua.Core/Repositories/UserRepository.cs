using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<UserListDTO> QueryableUserList()
        {
            return from usr in _context.Users.AsNoTracking()
                   join cmp in _context.Companies.AsNoTracking()
                   on usr.CompanyId equals cmp.CompanyId
                   where !usr.IsDeleted 
                   select new UserListDTO
                   {
                       UserId = usr.UserId,
                       UserName = usr.UserName,
                       UserEmail = usr.UserEmail,
                       StaffNo = usr.StaffNo,
                       MemberId = usr.MemberId,
                       PhoneNumber = usr.PhoneNumber,
                       CompanyName = cmp.ComapanyName,
                       Role = usr.Role,
                       IsActive = usr.IsActive
                   };
        }

        public async Task<UserDTO> GetUserDetailsAsync(int userId)
        {
            var q = (from user in _context.Users
                     join comp in _context.Companies
                         on user.CompanyId equals comp.CompanyId
                     join mem in _context.Members
                         on user.MemberId equals mem.MemberId into memberGroup
                     from member in memberGroup.DefaultIfEmpty()
                     where user.UserId == userId && !user.IsDeleted 
                     select new UserDTO
                     {
                         UserId = user.UserId,
                         UserName = user.UserName,
                         UserEmail = user.UserEmail,
                         StaffNo = user.StaffNo,
                         MemberId = user.MemberId,
                         PhoneNumber = user.PhoneNumber,
                         Address = user.Address,
                         IsActive = user.IsActive,
                         Islocked = user.Islocked,
                         Lastlogin = user.Lastlogin,
                         CreateAt = user.CreateAt,
                         CompanyId = user.CompanyId,
                         ComapanyName = comp.ComapanyName,
                         Role = user.Role,
                         IsDeleted = user.IsDeleted, // ✅ ADDED
                         ProfileImageSrc = member != null ? member.ProfileImageSrc : "",
                         CreateAtString = user.CreateAt.ToString("dd MMMM yyyy hh:mm tt"),
                         LastloginString = user.Lastlogin.HasValue
                             ? user.Lastlogin.Value.ToString("dd MMMM yyyy hh:mm tt")
                             : ""
                     }).FirstAsync();
            return await q;
        }

        public async Task<List<UserListDTO>> GetUsersAsync()
        {
            IQueryable<UserListDTO> q = QueryableUserList();
            return await q.ToListAsync();
        }

        public async Task AddLoginLogAsync(UserLoginLog log)
        {
            await _context.UserLoginLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserLoginLogDTO>> GetUserLogsAsync(int userId)
        {
            return await _context.UserLoginLogs
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ActionTime)
                .Select(x => new UserLoginLogDTO
                {
                    UserLoginLogId = x.UserLoginLogId,
                    UserId = x.UserId,
                    ActionType = x.ActionType,
                    ActionTimeString = x.ActionTime.ToString("dd MMM yyyy HH:mm:ss")
                })
                .ToListAsync();
        }
    }
}