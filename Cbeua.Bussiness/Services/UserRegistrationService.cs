using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRegistrationRepository _regRepo;
        private readonly IUserRepository _userRepo;

        public UserRegistrationService(IUserRegistrationRepository regRepo, IUserRepository userRepo)
        {
            _regRepo = regRepo;
            _userRepo = userRepo;
        }

        public async Task<CustomApiResponse> GetAllPendingAsync()
        {
            try
            {
                var pending = await _regRepo.GetPendingAsync();
                var dto = pending.Select(r => new UserRegistrationListDTO
                {
                    UserRegistrationId = r.UserRegistrationId,
                    UserName = r.UserName,
                    UserEmail = r.UserEmail,
                    StaffNo = r.StaffNo,
                    PhoneNumber = r.PhoneNumber,
                    RegistrationStatus = r.RegistrationStatus,
                    RequestedDate = r.RequestedDate
                }).ToList();

                return new CustomApiResponse { IsSucess = true, StatusCode = 200, Value = dto };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse { IsSucess = false, Error = ex.Message, StatusCode = 500 };
            }
        }

        public async Task<CustomApiResponse> GetAllAsync()
        {
            try
            {
                var all = await _regRepo.GetAllAsync();
                var dto = all.Select(r => new UserRegistrationListDTO
                {
                    UserRegistrationId = r.UserRegistrationId,
                    UserName = r.UserName,
                    UserEmail = r.UserEmail,
                    StaffNo = r.StaffNo,
                    PhoneNumber = r.PhoneNumber,
                    RegistrationStatus = r.RegistrationStatus,
                    RequestedDate = r.RequestedDate
                }).ToList();

                return new CustomApiResponse { IsSucess = true, StatusCode = 200, Value = dto };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse { IsSucess = false, Error = ex.Message, StatusCode = 500 };
            }
        }

        public async Task<CustomApiResponse> GetByIdAsync(int id)
        {
            try
            {
                var reg = await _regRepo.GetByIdAsync(id);
                if (reg == null)
                    return new CustomApiResponse { IsSucess = false, Error = "Registration not found", StatusCode = 404 };

                var dto = new UserRegistrationDTO
                {
                    UserRegistrationId = reg.UserRegistrationId,
                    UserName = reg.UserName,
                    UserEmail = reg.UserEmail,
                    StaffNo = reg.StaffNo,
                    MemberId = reg.MemberId,
                    PhoneNumber = reg.PhoneNumber,
                    Address = reg.Address,
                    Role = reg.Role,
                    RegistrationStatus = reg.RegistrationStatus,
                    RequestedDate = reg.RequestedDate,
                    ApprovedBy = reg.ApprovedBy,
                    ApprovedDate = reg.ApprovedDate,
                    RejectReason = reg.RejectReason
                };

                return new CustomApiResponse { IsSucess = true, StatusCode = 200, Value = dto };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse { IsSucess = false, Error = ex.Message, StatusCode = 500 };
            }
        }

        public async Task<CustomApiResponse> ApproveAsync(int id, int currentUserId, bool approve, string? rejectReason)
        {
            try
            {
                var reg = await _regRepo.GetByIdAsync(id);
                if (reg == null)
                    return new CustomApiResponse { IsSucess = false, Error = "Registration not found", StatusCode = 404 };

                if (reg.RegistrationStatus != "Pending")
                    return new CustomApiResponse { IsSucess = false, Error = "Only pending registrations can be approved or rejected", StatusCode = 400 };

                if (approve)
                {
                   
                    if (await _userRepo.AnyAsync(u => u.StaffNo == reg.StaffNo && !u.IsDeleted))
                        return new CustomApiResponse { IsSucess = false, Error = "StaffNo already registered to an active user", StatusCode = 400 };

                    var user = new User
                    {
                        UserName = reg.UserName,
                        UserEmail = reg.UserEmail,
                        PhoneNumber = reg.PhoneNumber,
                        Address = reg.Address,
                        PasswordHash = reg.PasswordHash,
                        IsActive = true,
                        Islocked = false,
                        CreateAt = DateTime.UtcNow,
                        CompanyId = reg.CompanyId,
                        StaffNo = reg.StaffNo,
                        MemberId = reg.MemberId,
                        Role = reg.Role
                    };

                    await _userRepo.AddAsync(user);

                    reg.RegistrationStatus = "Approved";
                    reg.ApprovedBy = currentUserId.ToString();
                    reg.ApprovedDate = DateTime.UtcNow;
                }
                else
                {
                    reg.RegistrationStatus = "Rejected";
                    reg.ApprovedBy = currentUserId.ToString();
                    reg.ApprovedDate = DateTime.UtcNow;
                    reg.RejectReason = rejectReason ?? "";
                }

                _regRepo.Update(reg);
                await _regRepo.SaveChangesAsync();
                await _userRepo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = approve ? "User approved and account created successfully" : "Registration rejected"
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse { IsSucess = false, Error = ex.Message, StatusCode = 500 };
            }
        }
    }
}