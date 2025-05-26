namespace inventrack_backend_demo.DTOs
{
    #region User Management DTOs

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
        public List<UserRoleDto> UserRoles { get; set; }
    }

    public class UserCreateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public UserProfileCreateDto Profile { get; set; }
    }

    public class UserUpdateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public UserProfileUpdateDto Profile { get; set; }
    }

    public class UserProfileDto
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }

    public class UserProfileUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string PreferredLanguage { get; set; }
        public string TimeZone { get; set; }
    }

    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }

    public class RoleCreateDto
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<int> PermissionIds { get; set; }
    }

    public class RoleUpdateDto
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<int> PermissionIds { get; set; }
    }

    public class UserRoleDto
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedDate { get; set; }
        public RoleDto Role { get; set; }
    }

    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
    }

    public class RolePermissionDto
    {
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public PermissionDto Permission { get; set; }
    }

    public class AuditLogDto
    {
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime Timestamp { get; set; }
        public string IPAddress { get; set; }
        public UserDto User { get; set; }
    }

    #endregion
}
