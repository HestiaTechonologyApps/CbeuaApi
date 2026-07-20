using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;

namespace Cbeua.Bussiness.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public String AuditTableName { get; set; } = "USER";

        public UserService(IUserRepository repo, IAuditRepository auditRepository, ICurrentUserService currentUser)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<UserDTO> CreateAsync(User user)
        {
            user.CompanyId = int.Parse(_currentUser.CompanyId);
            user.IsDeleted = false; // ✅ ENSURE NOT DELETED
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<User>(
               tableName: AuditTableName,
               action: "create",
               recordId: user.UserId,
               oldEntity: null,
               newEntity: user,
               changedBy: _currentUser.Email.ToString()
           );
            return await ConvertUserToDTO(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null || user.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            var oldEntity = CloneUser(user); // ✅ CLONE FOR AUDIT

            // ✅ SOFT DELETE
            user.IsDeleted = true;
            _repo.Update(user);

            await _auditRepository.LogAuditAsync<User>(
               tableName: AuditTableName,
               action: "delete",
               recordId: user.UserId,
               oldEntity: oldEntity,
               newEntity: user,
               changedBy: _currentUser.Email.ToString()
           );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserListDTO>> GetAllAsync()
        {
            var users = await _repo.GetUsersAsync();
            return users;
        }

        private async Task<UserDTO> ConvertUserToDTO(User user)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.UserId = user.UserId;
            userDTO.UserName = user.UserName;
            userDTO.UserEmail = user.UserEmail;
            userDTO.Address = user.Address;
            userDTO.PhoneNumber = user.PhoneNumber;
            userDTO.Islocked = user.Islocked;
            userDTO.CreateAt = user.CreateAt;
            userDTO.Lastlogin = user.Lastlogin;
            userDTO.CompanyId = user.CompanyId;
            userDTO.StaffNo = user.StaffNo;
            userDTO.MemberId = user.MemberId;
            userDTO.IsDeleted = user.IsDeleted; // ✅ ADDED
            userDTO.LastloginString = user.Lastlogin.HasValue ? user.Lastlogin.Value.ToString("dd MMMM yyyy hh:mm tt") : "";
            userDTO.CreateAtSyring = user.CreateAt.ToString("dd MMMM yyyy hh:mm tt");
            userDTO.Role = user.Role;
            userDTO.IsActive = user.IsActive;
            return userDTO;
        }

        // ✅ ADDED CLONE METHOD
        private User CloneUser(User user)
        {
            return new User
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                StaffNo = user.StaffNo,
                MemberId = user.MemberId,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                PasswordHash = user.PasswordHash,
                IsActive = user.IsActive,
                Islocked = user.Islocked,
                IsDeleted = user.IsDeleted,
                CreateAt = user.CreateAt,
                Lastlogin = user.Lastlogin,
                Role = user.Role,
                CompanyId = user.CompanyId
            };
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetUserDetailsAsync(id);
            return q;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var oldentity = await _repo.GetByIdAsync(user.UserId);
            if (oldentity == null || oldentity.IsDeleted) return false; // ✅ CHECK IF DELETED

            // Only update allowed fields
            oldentity.UserName = user.UserName;
            oldentity.UserEmail = user.UserEmail;
            oldentity.Address = user.Address;
            oldentity.PhoneNumber = user.PhoneNumber;
            oldentity.IsActive = user.IsActive;
            oldentity.Role = user.Role;
            oldentity.StaffNo = user.StaffNo;
            oldentity.MemberId = user.MemberId;
            oldentity.Islocked = user.Islocked;
            oldentity.CompanyId = int.Parse(_currentUser.CompanyId);

            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<User>(
                tableName: AuditTableName,
                action: "update",
                recordId: oldentity.UserId,
                oldEntity: oldentity,
                newEntity: user,
                changedBy: _currentUser.Email
            );

            return true;
        }

        public async Task<CustomApiResponse> ChangePassWord(PasswordChangeRequest passwordChangeRequest)
        {
            var response = new CustomApiResponse();
            try
            {
                var user = await _repo.GetByIdAsync(passwordChangeRequest.UserId);
                if (user == null || user.IsDeleted) // ✅ CHECK IF DELETED
                {
                    response.IsSucess = false;
                    response.Error = "User not found";
                }
                else
                {
                    if (user.PasswordHash != passwordChangeRequest.OldPassword)
                    {
                        response.IsSucess = false;
                        response.Error = "Old password does not match";
                    }
                    else
                    {
                        user.PasswordHash = passwordChangeRequest.NewPassword;
                        _repo.Update(user);
                        await _repo.SaveChangesAsync();
                        response.IsSucess = true;
                        response.Value = "Password changed successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }
        public async Task<PagedResult<UserListDTO>> GetPagedUsersAsync(UserPaginationParams parameters)
        {
            var query = _repo.QueryableUserList();

            if (parameters.UserId.HasValue && parameters.UserId.Value > 0)
                query = query.Where(u => u.UserId == parameters.UserId.Value);

            if (parameters.IsActive.HasValue)
                query = query.Where(u => u.IsActive == parameters.IsActive.Value);

            var allUsers = query.ToList();

            IEnumerable<UserListDTO> filteredUsers = allUsers;

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();

                filteredUsers = allUsers.Where(u =>
                    u.UserId.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(u.UserName) && u.UserName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(u.UserEmail) && u.UserEmail.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(u.PhoneNumber) && u.PhoneNumber.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(u.Role) && u.Role.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(u.CompanyName) && u.CompanyName.ToLower().Contains(searchLower)) ||
                    u.StaffNo.ToString().Contains(searchLower)
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                var sortBy = parameters.SortBy.ToLower();

                filteredUsers = sortBy switch
                {
                    "userid" => parameters.SortDescending
                        ? filteredUsers.OrderByDescending(u => u.UserId)
                        : filteredUsers.OrderBy(u => u.UserId),
                    "username" => parameters.SortDescending
                        ? filteredUsers.OrderByDescending(u => u.UserName)
                        : filteredUsers.OrderBy(u => u.UserName),
                    "useremail" => parameters.SortDescending
                        ? filteredUsers.OrderByDescending(u => u.UserEmail)
                        : filteredUsers.OrderBy(u => u.UserEmail),
                    "staffno" => parameters.SortDescending
                        ? filteredUsers.OrderByDescending(u => u.StaffNo)
                        : filteredUsers.OrderBy(u => u.StaffNo),
                    "role" => parameters.SortDescending
                        ? filteredUsers.OrderByDescending(u => u.Role)
                        : filteredUsers.OrderBy(u => u.Role),
                    "phonenumber" => parameters.SortDescending
                        ? filteredUsers.OrderByDescending(u => u.PhoneNumber)
                        : filteredUsers.OrderBy(u => u.PhoneNumber),
                    _ => parameters.SortDescending
                        ? filteredUsers.OrderByDescending(u => u.UserId)
                        : filteredUsers.OrderBy(u => u.UserId)
                };
            }
            else
            {
                filteredUsers = filteredUsers.OrderByDescending(u => u.UserId);
            }

            var totalRecords = filteredUsers.Count();

            var pageNumber = parameters.PageNumber;
            var pageSize = parameters.PageSize;

            List<UserListDTO> pagedData;

            if (parameters.GetAll)
            {
                pagedData = filteredUsers.ToList();
            }
            else
            {
                pagedData = filteredUsers
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            return new PagedResult<UserListDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = parameters.GetAll ? totalRecords : pageSize
            };
        }
    }
}
