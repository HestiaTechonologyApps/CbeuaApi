using System.ComponentModel.DataAnnotations;
namespace Cbeua.Domain.DTO
{
    // DTOs/Requests/LoginRequest.cs
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
    // DTOs/Requests/RegisterRequest.cs
    public class RegisterRequestDTO
    {
        [Required]
        public int StaffNo { get; set; }
        [Required]
        public string UserName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = "";
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = "";
    }
    // DTOs/Requests/ForgotPasswordRequest.cs
    public class ForgotPasswordRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
    // DTOs/Requests/ResetPasswordRequest.cs
    public class ResetPasswordRequestDTO
    {
        [Required]
        public string Token { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = "";
    }
    // DTOs/Requests/ChangePasswordRequest.cs
    public class ChangePasswordRequestDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = "";
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = "";
    }
}