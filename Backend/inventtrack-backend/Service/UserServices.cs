using AutoMapper;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using inventtrack_backend.Repository;
using Microsoft.AspNetCore.Identity;

namespace inventtrack_backend.Service
{
    public interface IUserService
    {
        Task<UserDto?> GetUserAsync(int id);
        Task<PaginatedResponse<UserDto>> GetUsersAsync(int pageNumber, int pageSize);
        Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto);
        Task UpdateUserAsync(int id, UserUpdateDto userUpdateDto);
        Task DeleteUserAsync(int id);
        Task ChangePasswordAsync(int userId, UserPasswordChangeDto passwordChangeDto);
        Task UpdateUserProfileAsync(int userId, UserProfileUpdateDto profileUpdateDto);
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId);
        Task AddUserToRoleAsync(int userId, int roleId);
        Task RemoveUserFromRoleAsync(int userId, int roleId);
        Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(int userId);
        Task<IEnumerable<AuditLogDto>> GetUserAuditLogsAsync(int userId, int count = 10);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto?> GetUserAsync(int id)
        {
            var user = await _userRepository.GetUserWithDetailsAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }
            return user;
        }

        public async Task<PaginatedResponse<UserDto>> GetUsersAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and size must be greater than 0");
            }

            var paginatedUsers = await _userRepository.GetPaginatedAsync(pageNumber, pageSize);
            var userDtos = _mapper.Map<List<UserDto>>(paginatedUsers.Items);

            return new PaginatedResponse<UserDto>
            {
                Items = userDtos,
                TotalCount = paginatedUsers.TotalCount,
                PageNumber = paginatedUsers.PageNumber,
                PageSize = paginatedUsers.PageSize
            };
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto)
        {
            if (await _userRepository.UsernameExistsAsync(userCreateDto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            if (await _userRepository.EmailExistsAsync(userCreateDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = _mapper.Map<User>(userCreateDto);
            user.PasswordHash = _passwordHasher.HashPassword(user, userCreateDto.Password);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;

            await _userRepository.AddAsync(user);

            // Add roles if specified
            if (userCreateDto.RoleIds != null && userCreateDto.RoleIds.Any())
            {
                foreach (var roleId in userCreateDto.RoleIds)
                {
                    await _userRepository.AddUserToRoleAsync(user.UserId, roleId);
                }
            }

            // Create profile if specified
            if (userCreateDto.Profile != null)
            {
                var profile = _mapper.Map<UserProfile>(userCreateDto.Profile);
                profile.UserId = user.UserId;
                await _userRepository.UpdateUserProfileAsync(profile);
            }

            return await GetUserAsync(user.UserId);
        }

        public async Task UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            if (!string.IsNullOrEmpty(userUpdateDto.Email) &&
                userUpdateDto.Email != user.Email &&
                await _userRepository.EmailExistsAsync(userUpdateDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            _mapper.Map(userUpdateDto, user);

            if (userUpdateDto.RoleIds != null)
            {
                // Get current roles
                var currentRoles = await _userRepository.GetUserRolesAsync(id);
                var currentRoleIds = currentRoles.Select(r => r.RoleId).ToList();

                // Roles to add
                var rolesToAdd = userUpdateDto.RoleIds.Except(currentRoleIds);
                foreach (var roleId in rolesToAdd)
                {
                    await _userRepository.AddUserToRoleAsync(id, roleId);
                }

                // Roles to remove
                var rolesToRemove = currentRoleIds.Except(userUpdateDto.RoleIds);
                foreach (var roleId in rolesToRemove)
                {
                    await _userRepository.RemoveUserFromRoleAsync(id, roleId);
                }
            }

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            // Soft delete implementation
            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(int userId, UserPasswordChangeDto passwordChangeDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            // Verify current password
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                passwordChangeDto.CurrentPassword);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Current password is incorrect");
            }

            // Update password
            user.PasswordHash = _passwordHasher.HashPassword(user, passwordChangeDto.NewPassword);
            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdateUserProfileAsync(int userId, UserProfileUpdateDto profileUpdateDto)
        {
            var profile = await _userRepository.GetUserProfileAsync(userId);
            if (profile == null)
            {
                // Create new profile if it doesn't exist
                profile = _mapper.Map<UserProfile>(profileUpdateDto);
                profile.UserId = userId;
                await _userRepository.UpdateUserProfileAsync(profile);
            }
            else
            {
                _mapper.Map(profileUpdateDto, profile);
                await _userRepository.UpdateUserProfileAsync(profile);
            }
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            return await _userRepository.GetUserRolesAsync(userId);
        }

        public async Task AddUserToRoleAsync(int userId, int roleId)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            if (await _userRepository.IsUserInRoleAsync(userId, roleId))
            {
                throw new InvalidOperationException("User already has this role");
            }

            await _userRepository.AddUserToRoleAsync(userId, roleId);
        }

        public async Task RemoveUserFromRoleAsync(int userId, int roleId)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            if (!await _userRepository.IsUserInRoleAsync(userId, roleId))
            {
                throw new InvalidOperationException("User doesn't have this role");
            }

            await _userRepository.RemoveUserFromRoleAsync(userId, roleId);
        }

        public async Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(int userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            return await _userRepository.GetUserPermissionsAsync(userId);
        }

        public async Task<IEnumerable<AuditLogDto>> GetUserAuditLogsAsync(int userId, int count = 10)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            if (count < 1 || count > 100)
            {
                throw new ArgumentException("Count must be between 1 and 100");
            }

            return await _userRepository.GetUserAuditLogsAsync(userId, count);
        }
    }
}
