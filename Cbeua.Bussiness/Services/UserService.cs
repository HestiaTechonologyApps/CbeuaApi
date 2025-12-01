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
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<User>(
               tableName: AuditTableName,
               action: "create",
               recordId: user.UserId,
               oldEntity: null,
               newEntity: user,
               changedBy: _currentUser.Email.ToString() // Replace with actual user info
           );
            return await ConvertUserToDTO(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            _repo.Delete(user);
            await _auditRepository.LogAuditAsync<User>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: user.UserId,
               oldEntity: user,
               newEntity: user,
               changedBy: _currentUser.Email.ToString() // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserListDTO>> GetAllAsync()
        {


          //  List<UserDTO> userdtos = new List<UserDTO>();

            var users = await _repo.GetUsersAsync();

            //foreach (var user in users)
            //{
            //    UserDTO userDTO = await ConvertUserToDTO(user);
            //    userdtos.Add(userDTO);


            //}

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
            userDTO.LastloginString = user.Lastlogin.HasValue ? user.Lastlogin.Value.ToString("dd MMMM yyyy hh:mm tt") : "";
            userDTO.CreateAtSyring = user.CreateAt.ToString("dd MMMM yyyy hh:mm tt");


            userDTO.IsActive = user.IsActive;
           // userDTO.AuditLogs = await _auditRepository.GetAuditLogsForEntityAsync("User", user.UserId);
            return userDTO;
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetUserDetailsAsync(id);
            //if (q == null) return null;
            //var userdto = await ConvertUserToDTO(q);
           // userdto.AuditLogs = await _auditRepository.GetAuditLogsForEntityAsync("Users", userdto.UserId);
            return q;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            user.CompanyId = int.Parse(_currentUser.CompanyId);
            var oldentity = await _repo.GetByIdAsync(user.UserId);
            _repo.Detach(oldentity);
            _repo.Update(user);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<User>(
               tableName: AuditTableName,
               action: "update",
               recordId: user.UserId,
               oldEntity: oldentity,
               newEntity: user,
               changedBy: _currentUser.Email.ToString()  // Replace with actual user info
           );
            return true;
        }

        public async Task<CustomApiResponse> ChangePassWord(PasswordChangeRequest passwordChangeRequest)
        {
            var response = new CustomApiResponse();
            try
            {
                var user = await _repo.GetByIdAsync(passwordChangeRequest.UserId);
                if (user == null)
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

       

        //public Task<List<UserLoginLog>> GetUserLogsAsync(int userId)
        //{
        //    return _repo.GetLogsByUserAsync(userId);
        //}

        //Task<List<UserDTO>> IUserService.GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
