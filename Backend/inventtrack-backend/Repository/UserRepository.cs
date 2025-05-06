using inventtrack_backend.Database;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace inventtrack_backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbOmniflow _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(DbOmniflow context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }


        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<PaginatedResponse<User>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<User>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _dbSet.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            _dbSet.Update(user);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(User user)
        {
            _dbSet.Remove(user);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(u => u.UserId == id);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<UserDto?> GetUserWithDetailsAsync(int id)
        {
            return await _dbSet
                .Where(u => u.UserId == id)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    IsActive = u.IsActive,
                    CreatedDate = u.CreatedDate,
                    LastLoginDate = u.LastLoginDate,
                    TwoFactorEnabled = u.TwoFactorEnabled,
                    Profile = u.Profile != null ? new UserProfileDto
                    {
                        ProfileId = u.Profile.ProfileId,
                        FirstName = u.Profile.FirstName,
                        LastName = u.Profile.LastName,
                        ProfileImage = u.Profile.ProfileImage,
                        Address = u.Profile.Address,
                        City = u.Profile.City,
                        State = u.Profile.State,
                        ZipCode = u.Profile.ZipCode,
                        Country = u.Profile.Country,
                        PreferredLanguage = u.Profile.PreferredLanguage,
                        TimeZone = u.Profile.TimeZone
                    } : null,
                    Roles = u.UserRoles.Select(ur => new RoleDto
                    {
                        RoleId = ur.Role.RoleId,
                        RoleName = ur.Role.RoleName,
                        Description = ur.Role.Description,
                        IsSystemRole = ur.Role.IsSystemRole,
                        Permissions = ur.Role.RolePermissions.Select(rp => new PermissionDto
                        {
                            PermissionId = rp.Permission.PermissionId,
                            PermissionName = rp.Permission.PermissionName,
                            Description = rp.Permission.Description,
                            Module = rp.Permission.Module,
                            Action = rp.Permission.Action
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserProfile?> GetUserProfileAsync(int userId)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == userId);
        }

        public async Task UpdateUserProfileAsync(UserProfile profile)
        {
            _context.UserProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => new RoleDto
                {
                    RoleId = ur.Role.RoleId,
                    RoleName = ur.Role.RoleName,
                    Description = ur.Role.Description,
                    IsSystemRole = ur.Role.IsSystemRole,
                    Permissions = ur.Role.RolePermissions.Select(rp => new PermissionDto
                    {
                        PermissionId = rp.Permission.PermissionId,
                        PermissionName = rp.Permission.PermissionName,
                        Description = rp.Permission.Description,
                        Module = rp.Permission.Module,
                        Action = rp.Permission.Action
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task AddUserToRoleAsync(int userId, int roleId)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedDate = DateTime.UtcNow
            };
            await _context.UserRoles.AddAsync(userRole);
        }

        public async Task RemoveUserFromRoleAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
            }
        }

        public async Task<bool> IsUserInRoleAsync(int userId, int roleId)
        {
            return await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => new PermissionDto
                {
                    PermissionId = rp.Permission.PermissionId,
                    PermissionName = rp.Permission.PermissionName,
                    Description = rp.Permission.Description,
                    Module = rp.Permission.Module,
                    Action = rp.Permission.Action
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .AnyAsync(rp => rp.Permission.PermissionName == permissionName);
        }

        public async Task<IEnumerable<AuditLogDto>> GetUserAuditLogsAsync(int userId, int count = 10)
        {
            return await _context.AuditLogs
                .Where(al => al.UserId == userId)
                .OrderByDescending(al => al.Timestamp)
                .Take(count)
                .Select(al => new AuditLogDto
                {
                    LogId = al.LogId,
                    UserId = (int)al.UserId,
                    Username = al.User.Username,
                    Action = al.Action,
                    EntityName = al.EntityName,
                    EntityId = al.EntityId,
                    OldValues = al.OldValues,
                    NewValues = al.NewValues,
                    Timestamp = al.Timestamp,
                    IPAddress = al.IPAddress
                })
                .ToListAsync();
        }

        public async Task ChangePasswordAsync(int userId, string newPasswordHash)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                user.PasswordHash = newPasswordHash;
                _dbSet.Update(user);
            }
        }
    }
}
