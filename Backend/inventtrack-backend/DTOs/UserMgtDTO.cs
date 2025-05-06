using System.ComponentModel.DataAnnotations;
namespace inventtrack_backend.DTOs
{    

    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public UserProfileDto Profile { get; set; }
        public ICollection<RoleDto> Roles { get; set; }
    }

    public class UserCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public UserProfileCreateDto Profile { get; set; }

        public List<int> RoleIds { get; set; } = new List<int>();
    }

    public class UserUpdateDto
    {
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public bool? IsActive { get; set; }

        public bool? TwoFactorEnabled { get; set; }

        public List<int> RoleIds { get; set; }
    }

    public class UserPasswordChangeDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class UserProfileDto
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string ProfileImage { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string PreferredLanguage { get; set; }
        public string TimeZone { get; set; }
    }

    public class UserProfileCreateDto
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(20)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(20)]
        public string PreferredLanguage { get; set; }

        [StringLength(50)]
        public string TimeZone { get; set; }
    }

    public class UserProfileUpdateDto
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string ProfileImage { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(20)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(20)]
        public string PreferredLanguage { get; set; }

        [StringLength(50)]
        public string TimeZone { get; set; }
    }

    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
        public ICollection<PermissionDto> Permissions { get; set; }
    }

    public class RoleCreateDto
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public List<int> PermissionIds { get; set; } = new List<int>();
    }

    public class RoleUpdateDto
    {
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public List<int> PermissionIds { get; set; }
    }

    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
    }

    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class TokenResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; }
    }

    public class AuditLogDto
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime Timestamp { get; set; }
        public string IPAddress { get; set; }
    }
}
