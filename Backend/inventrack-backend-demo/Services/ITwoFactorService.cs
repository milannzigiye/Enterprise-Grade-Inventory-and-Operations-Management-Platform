using inventrack_backend_demo.DTOs;

namespace inventrack_backend_demo.Services
{
    public interface ITwoFactorService
    {
        Task<TwoFactorSetupDto> GenerateSetupAsync(int userId);
        Task<bool> EnableTwoFactorAsync(int userId, string code);
        Task<bool> DisableTwoFactorAsync(int userId);
        Task<bool> VerifyCodeAsync(int userId, string code);
        string GenerateQrCodeUrl(string email, string secretKey);
    }
}
