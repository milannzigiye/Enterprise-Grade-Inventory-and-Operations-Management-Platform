using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using System.Security.Claims;

namespace inventrack_backend_demo.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    }
}
