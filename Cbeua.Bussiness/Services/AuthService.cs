using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Cbeua.Bussiness.Helpers;
using Cbeua.Core.Helpers; // ApiResponseFactory lives here
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;

namespace Cbeua.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        // In production, replace with persistent storage (DB/Cache)
        private readonly Dictionary<string, string> _resetTokens = new();

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.UserEmail == email);
            return users.FirstOrDefault();
        }

        public async Task<CustomApiResponse> LoginAsync(LoginRequestDTO request)
        {
            try
            {
                var user = await GetUserByEmailAsync(request.Email);
                if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                    return ApiResponseFactory.Fail("Invalid email or password");

                if (user.Islocked)
                    return ApiResponseFactory.Fail("Account is locked. Please contact support.");

                if (!user.IsActive)
                    return ApiResponseFactory.Fail("Account is inactive. Please contact support.");

                user.Lastlogin = DateTime.UtcNow;
                _userRepository.Update(user);

                var token = _jwtService.GenerateToken(MapToUserDto(user));

                var data = new
                {
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = MapToUserDto(user)
                };

                var log = new UserLoginLog
                {
                    UserId = user.UserId,
                    UserLoginLogId = 0,
                    ActionType = "Login",
                    ActionTime = DateTime.UtcNow

                };
                await _userRepository.AddLoginLogAsync(log);

                return ApiResponseFactory.Success(data, "Login successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", request.Email);
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> ChangePasswordAsync(int userId, ChangePasswordRequestDTO request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return ApiResponseFactory.Fail("User not found");

                if (!PasswordHelper.VerifyPassword(request.CurrentPassword, user.PasswordHash))
                    return ApiResponseFactory.Fail("Current password is incorrect");

                user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
                _userRepository.Update(user);

                return ApiResponseFactory.Success(null, "Password changed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);
                if (user == null)
                    return ApiResponseFactory.Success(null, "If the email exists, a reset link has been sent.");

                var resetToken = PasswordHelper.GenerateResetToken();
                _resetTokens[resetToken] = email;

                _logger.LogInformation("Reset token for {Email}: {Token}", email, resetToken);

                return ApiResponseFactory.Success(resetToken, "If the email exists, a reset link has been sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in forgot password for {Email}", email);
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> GetCurrentUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return ApiResponseFactory.Fail("User not found");

                return ApiResponseFactory.Success(MapToUserDto(user), "User retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {UserId}", userId);
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> RegisterAsync(RegisterRequestDTO request)
        {
            try
            {
                if (await _userRepository.AnyAsync(u => u.UserEmail == request.UserEmail))
                    return ApiResponseFactory.Fail("Email already registered");

                if (await _userRepository.AnyAsync(u => u.UserName == request.UserName))
                    return ApiResponseFactory.Fail("Username already taken");

                var user = new User
                {
                    UserName = request.UserName,
                    UserEmail = request.UserEmail,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    PasswordHash = PasswordHelper.HashPassword(request.Password),
                    IsActive = true,
                    Islocked = false,
                    CreateAt = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);

                var createdUser = await GetUserByEmailAsync(request.UserEmail);
                if (createdUser == null)
                    return ApiResponseFactory.Fail("Failed to create user. Please try again.");

                var token = _jwtService.GenerateToken(MapToUserDto(createdUser));

                var data = new
                {
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = MapToUserDto(createdUser)
                };

                return ApiResponseFactory.Success(data, "Registration successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for {Email}", request.UserEmail);
                return ApiResponseFactory.Exception(ex);
            }
        }

        public async Task<CustomApiResponse> ResetPasswordAsync(ResetPasswordRequestDTO request)
        {
            try
            {
                if (!_resetTokens.TryGetValue(request.Token, out var storedEmail) || storedEmail != request.Email)
                    return ApiResponseFactory.Fail("Invalid or expired reset token");

                var user = await GetUserByEmailAsync(request.Email);
                if (user == null)
                    return ApiResponseFactory.Fail("User not found");

                user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
                _userRepository.Update(user);

                _resetTokens.Remove(request.Token);

                return ApiResponseFactory.Success(null, "Password reset successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for {Email}", request.Email);
                return ApiResponseFactory.Exception(ex);
            }
        }

        private UserDTO MapToUserDto(User user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive,
                CreateAt = user.CreateAt,
                Lastlogin = user.Lastlogin,
                CompanyId=user.CompanyId
            };
        }
    }

    
}
