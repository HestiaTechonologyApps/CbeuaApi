using Cbeua.Domain.DTO;

public interface IAuthService
{
    Task<CustomApiResponse> LoginAsync(LoginRequestDTO request);
    Task<CustomApiResponse> RegisterAsync(RegisterRequestDTO request);
    Task<CustomApiResponse> ForgotPasswordAsync(string email);
    Task<CustomApiResponse> ResetPasswordAsync(ResetPasswordRequestDTO request);
    Task<CustomApiResponse> ChangePasswordAsync(int userId, ChangePasswordRequestDTO request);
    Task<CustomApiResponse> GetCurrentUserAsync(int userId);
}
