using System.ComponentModel.DataAnnotations;

namespace inventrack_backend_demo.DTOs
{
    #region Authentication DTOs

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? TwoFactorCode { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public UserProfileCreateDto? Profile { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; }
        public bool RequiresTwoFactor { get; set; }
    }

    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }

    public class TwoFactorSetupDto
    {
        public string SecretKey { get; set; }
        public string QrCodeUrl { get; set; }
        public string ManualEntryKey { get; set; }
    }

    public class EnableTwoFactorDto
    {
        [Required]
        public string Code { get; set; }
    }

    public class VerifyTwoFactorDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Email { get; set; }
    }

    #endregion
}
