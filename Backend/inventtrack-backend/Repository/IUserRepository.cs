using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using System.Linq.Expressions;

namespace inventtrack_backend.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<PaginatedResponse<User>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistsAsync(int id);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);

        // User-specific methods
        Task<UserDto?> GetUserWithDetailsAsync(int id);
        Task<UserProfile?> GetUserProfileAsync(int userId);
        Task UpdateUserProfileAsync(UserProfile profile);
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId);
        Task AddUserToRoleAsync(int userId, int roleId);
        Task RemoveUserFromRoleAsync(int userId, int roleId);
        Task<bool> IsUserInRoleAsync(int userId, int roleId);
        Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(int userId);
        Task<bool> HasPermissionAsync(int userId, string permissionName);
        Task<IEnumerable<AuditLogDto>> GetUserAuditLogsAsync(int userId, int count = 10);
        Task ChangePasswordAsync(int userId, string newPasswordHash);
    }
}
