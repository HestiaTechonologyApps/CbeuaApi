using System.Security.Claims;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;

public interface IJwtService
{
    string GenerateToken(UserDTO user);
    ClaimsPrincipal? ValidateToken(string token);

}
