using System.Security.Claims;

namespace Cbeua.Api.Security
{
    /// <summary>
    /// Server-side role checks that mirror the frontend ProtectedRoute: role names are compared
    /// trimmed and case-insensitively, because User.Role is free text in the database.
    /// </summary>
    public static class AuthPolicies
    {
        /// <summary>Admin portal roles (SystemAdmin, Administrator).</summary>
        public const string AdminOnly = "AdminOnly";

        public static readonly string[] AdminRoles = { "SystemAdmin", "Administrator" };
    }

    public static class ClaimsPrincipalExtensions
    {
        /// <summary>The logged-in user's id from the JWT (SerialNumber claim), or 0 if absent.</summary>
        public static int GetUserId(this ClaimsPrincipal user) =>
            int.TryParse(user.FindFirst(ClaimTypes.SerialNumber)?.Value, out var id) ? id : 0;

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            var role = user.FindFirst(ClaimTypes.Role)?.Value?.Trim();
            return !string.IsNullOrEmpty(role) &&
                   AuthPolicies.AdminRoles.Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));
        }
    }
}
