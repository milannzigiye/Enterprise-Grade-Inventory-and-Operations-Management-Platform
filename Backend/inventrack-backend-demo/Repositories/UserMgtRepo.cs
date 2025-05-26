using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using inventrack_backend_demo.Data;
using Microsoft.EntityFrameworkCore;

namespace inventrack_backend_demo.Repositories
{
    #region User Management Repositories

    public interface IUserRepository
    {
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto);
        Task<UserDto> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserProfileDto> GetUserProfileAsync(int userId);
        Task<UserProfileDto> UpdateUserProfileAsync(int userId, UserProfileUpdateDto profileUpdateDto);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                TwoFactorEnabled = user.TwoFactorEnabled,
                Profile = user.Profile != null ? new UserProfileDto
                {
                    ProfileId = user.Profile.ProfileId,
                    FirstName = user.Profile.FirstName,
                    LastName = user.Profile.LastName,
                    ProfileImage = user.Profile.ProfileImage,
                    Address = user.Profile.Address,
                    City = user.Profile.City,
                    State = user.Profile.State,
                    ZipCode = user.Profile.ZipCode,
                    Country = user.Profile.Country,
                    PreferredLanguage = user.Profile.PreferredLanguage,
                    TimeZone = user.Profile.TimeZone
                } : null,
                UserRoles = user.UserRoles?.Select(ur => new UserRoleDto
                {
                    UserRoleId = ur.UserRoleId,
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    AssignedDate = ur.AssignedDate,
                    Role = new RoleDto
                    {
                        RoleId = ur.Role.RoleId,
                        RoleName = ur.Role.RoleName,
                        Description = ur.Role.Description,
                        IsSystemRole = ur.Role.IsSystemRole
                    }
                }).ToList()
            };
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
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
                    UserRoles = u.UserRoles.Select(ur => new UserRoleDto
                    {
                        UserRoleId = ur.UserRoleId,
                        UserId = ur.UserId,
                        RoleId = ur.RoleId,
                        AssignedDate = ur.AssignedDate,
                        Role = new RoleDto
                        {
                            RoleId = ur.Role.RoleId,
                            RoleName = ur.Role.RoleName,
                            Description = ur.Role.Description,
                            IsSystemRole = ur.Role.IsSystemRole
                        }
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto)
        {
            var user = new User
            {
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password),
                PhoneNumber = userCreateDto.PhoneNumber,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            if (userCreateDto.Profile != null)
            {
                user.Profile = new UserProfile
                {
                    FirstName = userCreateDto.Profile.FirstName,
                    LastName = userCreateDto.Profile.LastName,
                    Address = userCreateDto.Profile.Address,
                    City = userCreateDto.Profile.City,
                    State = userCreateDto.Profile.State,
                    ZipCode = userCreateDto.Profile.ZipCode,
                    Country = userCreateDto.Profile.Country
                };
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await GetUserByIdAsync(user.UserId);
        }

        public async Task<UserDto> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) return null;

            user.Username = userUpdateDto.Username;
            user.Email = userUpdateDto.Email;
            user.PhoneNumber = userUpdateDto.PhoneNumber;
            user.IsActive = userUpdateDto.IsActive;

            if (userUpdateDto.Profile != null)
            {
                if (user.Profile == null)
                {
                    user.Profile = new UserProfile();
                }

                user.Profile.FirstName = userUpdateDto.Profile.FirstName;
                user.Profile.LastName = userUpdateDto.Profile.LastName;
                user.Profile.ProfileImage = userUpdateDto.Profile.ProfileImage;
                user.Profile.Address = userUpdateDto.Profile.Address;
                user.Profile.City = userUpdateDto.Profile.City;
                user.Profile.State = userUpdateDto.Profile.State;
                user.Profile.ZipCode = userUpdateDto.Profile.ZipCode;
                user.Profile.Country = userUpdateDto.Profile.Country;
                user.Profile.PreferredLanguage = userUpdateDto.Profile.PreferredLanguage;
                user.Profile.TimeZone = userUpdateDto.Profile.TimeZone;
            }

            await _context.SaveChangesAsync();

            return await GetUserByIdAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return null;

            return new UserProfileDto
            {
                ProfileId = profile.ProfileId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ProfileImage = profile.ProfileImage,
                Address = profile.Address,
                City = profile.City,
                State = profile.State,
                ZipCode = profile.ZipCode,
                Country = profile.Country,
                PreferredLanguage = profile.PreferredLanguage,
                TimeZone = profile.TimeZone
            };
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(int userId, UserProfileUpdateDto profileUpdateDto)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                profile = new UserProfile { UserId = userId };
                _context.UserProfiles.Add(profile);
            }

            profile.FirstName = profileUpdateDto.FirstName;
            profile.LastName = profileUpdateDto.LastName;
            profile.ProfileImage = profileUpdateDto.ProfileImage;
            profile.Address = profileUpdateDto.Address;
            profile.City = profileUpdateDto.City;
            profile.State = profileUpdateDto.State;
            profile.ZipCode = profileUpdateDto.ZipCode;
            profile.Country = profileUpdateDto.Country;
            profile.PreferredLanguage = profileUpdateDto.PreferredLanguage;
            profile.TimeZone = profileUpdateDto.TimeZone;

            await _context.SaveChangesAsync();

            return new UserProfileDto
            {
                ProfileId = profile.ProfileId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ProfileImage = profile.ProfileImage,
                Address = profile.Address,
                City = profile.City,
                State = profile.State,
                ZipCode = profile.ZipCode,
                Country = profile.Country,
                PreferredLanguage = profile.PreferredLanguage,
                TimeZone = profile.TimeZone
            };
        }
    }

    public interface IRoleRepository
    {
        Task<RoleDto> GetRoleByIdAsync(int roleId);
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> CreateRoleAsync(RoleCreateDto roleCreateDto);
        Task<RoleDto> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto);
        Task<bool> DeleteRoleAsync(int roleId);
        Task<List<PermissionDto>> GetRolePermissionsAsync(int roleId);
        Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoleDto> GetRoleByIdAsync(int roleId)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (role == null) return null;

            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Description = role.Description,
                IsSystemRole = role.IsSystemRole,
                Permissions = role.RolePermissions?.Select(rp => new PermissionDto
                {
                    PermissionId = rp.Permission.PermissionId,
                    PermissionName = rp.Permission.PermissionName,
                    Description = rp.Permission.Description,
                    Module = rp.Permission.Module,
                    Action = rp.Permission.Action
                }).ToList()
            };
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .Select(r => new RoleDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Description = r.Description,
                    IsSystemRole = r.IsSystemRole,
                    Permissions = r.RolePermissions.Select(rp => new PermissionDto
                    {
                        PermissionId = rp.Permission.PermissionId,
                        PermissionName = rp.Permission.PermissionName,
                        Description = rp.Permission.Description,
                        Module = rp.Permission.Module,
                        Action = rp.Permission.Action
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<RoleDto> CreateRoleAsync(RoleCreateDto roleCreateDto)
        {
            var role = new Role
            {
                RoleName = roleCreateDto.RoleName,
                Description = roleCreateDto.Description,
                IsSystemRole = false
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            if (roleCreateDto.PermissionIds != null && roleCreateDto.PermissionIds.Any())
            {
                await AssignPermissionsToRoleAsync(role.RoleId, roleCreateDto.PermissionIds);
            }

            return await GetRoleByIdAsync(role.RoleId);
        }

        public async Task<RoleDto> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return null;

            role.RoleName = roleUpdateDto.RoleName;
            role.Description = roleUpdateDto.Description;

            if (roleUpdateDto.PermissionIds != null)
            {
                // Remove existing permissions
                var existingPermissions = await _context.RolePermissions
                    .Where(rp => rp.RoleId == roleId)
                    .ToListAsync();
                _context.RolePermissions.RemoveRange(existingPermissions);

                // Add new permissions
                await AssignPermissionsToRoleAsync(roleId, roleUpdateDto.PermissionIds);
            }

            await _context.SaveChangesAsync();
            return await GetRoleByIdAsync(roleId);
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            if (role.IsSystemRole) return false; // Prevent deletion of system roles

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PermissionDto>> GetRolePermissionsAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => new PermissionDto
                {
                    PermissionId = rp.Permission.PermissionId,
                    PermissionName = rp.Permission.PermissionName,
                    Description = rp.Permission.Description,
                    Module = rp.Permission.Module,
                    Action = rp.Permission.Action
                }).ToListAsync();
        }

        public async Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            var permissions = await _context.Permissions
                .Where(p => permissionIds.Contains(p.PermissionId))
                .ToListAsync();

            foreach (var permission in permissions)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permission.PermissionId
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IUserRoleRepository
    {
        Task<UserRoleDto> AssignRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userRoleId);
        Task<List<UserRoleDto>> GetUserRolesAsync(int userId);
    }

    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserRoleDto> AssignRoleToUserAsync(int userId, int roleId)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedDate = DateTime.UtcNow
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return await _context.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserRoleId == userRole.UserRoleId)
                .Select(ur => new UserRoleDto
                {
                    UserRoleId = ur.UserRoleId,
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    AssignedDate = ur.AssignedDate,
                    Role = new RoleDto
                    {
                        RoleId = ur.Role.RoleId,
                        RoleName = ur.Role.RoleName,
                        Description = ur.Role.Description,
                        IsSystemRole = ur.Role.IsSystemRole
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userRoleId)
        {
            var userRole = await _context.UserRoles.FindAsync(userRoleId);
            if (userRole == null) return false;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserRoleDto>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .Select(ur => new UserRoleDto
                {
                    UserRoleId = ur.UserRoleId,
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    AssignedDate = ur.AssignedDate,
                    Role = new RoleDto
                    {
                        RoleId = ur.Role.RoleId,
                        RoleName = ur.Role.RoleName,
                        Description = ur.Role.Description,
                        IsSystemRole = ur.Role.IsSystemRole
                    }
                }).ToListAsync();
        }
    }

    public interface IPermissionRepository
    {
        Task<List<PermissionDto>> GetAllPermissionsAsync();
        Task<PermissionDto> GetPermissionByIdAsync(int permissionId);
        Task<PermissionDto> CreatePermissionAsync(PermissionDto permissionDto);
        Task<PermissionDto> UpdatePermissionAsync(int permissionId, PermissionDto permissionDto);
        Task<bool> DeletePermissionAsync(int permissionId);
    }

    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            return await _context.Permissions
                .Select(p => new PermissionDto
                {
                    PermissionId = p.PermissionId,
                    PermissionName = p.PermissionName,
                    Description = p.Description,
                    Module = p.Module,
                    Action = p.Action
                }).ToListAsync();
        }

        public async Task<PermissionDto> GetPermissionByIdAsync(int permissionId)
        {
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null) return null;

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                Description = permission.Description,
                Module = permission.Module,
                Action = permission.Action
            };
        }

        public async Task<PermissionDto> CreatePermissionAsync(PermissionDto permissionDto)
        {
            var permission = new Permission
            {
                PermissionName = permissionDto.PermissionName,
                Description = permissionDto.Description,
                Module = permissionDto.Module,
                Action = permissionDto.Action
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                Description = permission.Description,
                Module = permission.Module,
                Action = permission.Action
            };
        }

        public async Task<PermissionDto> UpdatePermissionAsync(int permissionId, PermissionDto permissionDto)
        {
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null) return null;

            permission.PermissionName = permissionDto.PermissionName;
            permission.Description = permissionDto.Description;
            permission.Module = permissionDto.Module;
            permission.Action = permissionDto.Action;

            await _context.SaveChangesAsync();

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                Description = permission.Description,
                Module = permission.Module,
                Action = permission.Action
            };
        }

        public async Task<bool> DeletePermissionAsync(int permissionId)
        {
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null) return false;

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IAuditLogRepository
    {
        Task<List<AuditLogDto>> GetAuditLogsAsync(DateTime? fromDate, DateTime? toDate, string entityType, int? userId);
        Task<AuditLogDto> CreateAuditLogAsync(AuditLogDto auditLogDto);
    }

    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLogDto>> GetAuditLogsAsync(DateTime? fromDate, DateTime? toDate, string entityType, int? userId)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(log => log.Timestamp >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(log => log.Timestamp <= toDate.Value);
            }

            if (!string.IsNullOrEmpty(entityType))
            {
                query = query.Where(log => log.EntityName == entityType);
            }

            if (userId.HasValue)
            {
                query = query.Where(log => log.UserId == userId.Value);
            }

            return await query
                .Include(log => log.User)
                .OrderByDescending(log => log.Timestamp)
                .Select(log => new AuditLogDto
                {
                    LogId = log.LogId,
                    UserId = log.UserId,
                    Action = log.Action,
                    EntityName = log.EntityName,
                    EntityId = log.EntityId,
                    OldValues = log.OldValues,
                    NewValues = log.NewValues,
                    Timestamp = log.Timestamp,
                    IPAddress = log.IPAddress,
                    User = log.User != null ? new UserDto
                    {
                        UserId = log.User.UserId,
                        Username = log.User.Username,
                        Email = log.User.Email
                    } : null
                }).ToListAsync();
        }

        public async Task<AuditLogDto> CreateAuditLogAsync(AuditLogDto auditLogDto)
        {
            var auditLog = new AuditLog
            {
                UserId = auditLogDto.UserId,
                Action = auditLogDto.Action,
                EntityName = auditLogDto.EntityName,
                EntityId = auditLogDto.EntityId,
                OldValues = auditLogDto.OldValues,
                NewValues = auditLogDto.NewValues,
                Timestamp = DateTime.UtcNow,
                IPAddress = auditLogDto.IPAddress
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return new AuditLogDto
            {
                LogId = auditLog.LogId,
                UserId = auditLog.UserId,
                Action = auditLog.Action,
                EntityName = auditLog.EntityName,
                EntityId = auditLog.EntityId,
                OldValues = auditLog.OldValues,
                NewValues = auditLog.NewValues,
                Timestamp = auditLog.Timestamp,
                IPAddress = auditLog.IPAddress
            };
        }
    }

    #endregion
}
