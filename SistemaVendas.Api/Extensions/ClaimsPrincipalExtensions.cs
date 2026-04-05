using System.Security.Claims;

namespace SistemaVendas.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetRequiredUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(value, out var userId))
                throw new UnauthorizedAccessException("Usuario autenticado invalido.");

            return userId;
        }

        public static string GetRequiredUserRole(this ClaimsPrincipal user)
        {
            var role = user.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(role))
                throw new UnauthorizedAccessException("Role do usuario autenticado nao encontrada.");

            return role;
        }
    }
}
