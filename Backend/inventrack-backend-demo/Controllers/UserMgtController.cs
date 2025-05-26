using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    #region User Management Controllers

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var user = await _userRepository.CreateUserAsync(userCreateDto);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int userId, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.UpdateUserAsync(userId, userUpdateDto);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var result = await _userRepository.DeleteUserAsync(userId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{userId}/profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(int userId)
        {
            var profile = await _userRepository.GetUserProfileAsync(userId);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPut("{userId}/profile")]
        public async Task<ActionResult<UserProfileDto>> UpdateUserProfile(int userId, UserProfileUpdateDto profileUpdateDto)
        {
            var profile = await _userRepository.UpdateUserProfileAsync(userId, profileUpdateDto);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("{roleId}")]
        public async Task<ActionResult<RoleDto>> GetRoleById(int roleId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole(RoleCreateDto roleCreateDto)
        {
            var role = await _roleRepository.CreateRoleAsync(roleCreateDto);
            return CreatedAtAction(nameof(GetRoleById), new { roleId = role.RoleId }, role);
        }

        [HttpPut("{roleId}")]
        public async Task<ActionResult<RoleDto>> UpdateRole(int roleId, RoleUpdateDto roleUpdateDto)
        {
            var role = await _roleRepository.UpdateRoleAsync(roleId, roleUpdateDto);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpDelete("{roleId}")]
        public async Task<ActionResult> DeleteRole(int roleId)
        {
            var result = await _roleRepository.DeleteRoleAsync(roleId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{roleId}/permissions")]
        public async Task<ActionResult<List<PermissionDto>>> GetRolePermissions(int roleId)
        {
            var permissions = await _roleRepository.GetRolePermissionsAsync(roleId);
            return Ok(permissions);
        }

        [HttpPost("{roleId}/permissions")]
        public async Task<ActionResult> AssignPermissionsToRole(int roleId, [FromBody] List<int> permissionIds)
        {
            var result = await _roleRepository.AssignPermissionsToRoleAsync(roleId, permissionIds);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleController(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        [HttpPost]
        public async Task<ActionResult<UserRoleDto>> AssignRoleToUser([FromBody] UserRoleAssignDto assignDto)
        {
            var userRole = await _userRoleRepository.AssignRoleToUserAsync(assignDto.UserId, assignDto.RoleId);
            return Ok(userRole);
        }

        [HttpDelete("{userRoleId}")]
        public async Task<ActionResult> RemoveRoleFromUser(int userRoleId)
        {
            var result = await _userRoleRepository.RemoveRoleFromUserAsync(userRoleId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<UserRoleDto>>> GetUserRoles(int userId)
        {
            var roles = await _userRoleRepository.GetUserRolesAsync(userId);
            return Ok(roles);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PermissionDto>>> GetAllPermissions()
        {
            var permissions = await _permissionRepository.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("{permissionId}")]
        public async Task<ActionResult<PermissionDto>> GetPermissionById(int permissionId)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);
            if (permission == null)
                return NotFound();

            return Ok(permission);
        }

        [HttpPost]
        public async Task<ActionResult<PermissionDto>> CreatePermission(PermissionDto permissionDto)
        {
            var permission = await _permissionRepository.CreatePermissionAsync(permissionDto);
            return CreatedAtAction(nameof(GetPermissionById), new { permissionId = permission.PermissionId }, permission);
        }

        [HttpPut("{permissionId}")]
        public async Task<ActionResult<PermissionDto>> UpdatePermission(int permissionId, PermissionDto permissionDto)
        {
            var permission = await _permissionRepository.UpdatePermissionAsync(permissionId, permissionDto);
            if (permission == null)
                return NotFound();

            return Ok(permission);
        }

        [HttpDelete("{permissionId}")]
        public async Task<ActionResult> DeletePermission(int permissionId)
        {
            var result = await _permissionRepository.DeletePermissionAsync(permissionId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogController(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuditLogDto>>> GetAuditLogs(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string entityType,
            [FromQuery] int? userId)
        {
            var logs = await _auditLogRepository.GetAuditLogsAsync(fromDate, toDate, entityType, userId);
            return Ok(logs);
        }

        [HttpPost]
        public async Task<ActionResult<AuditLogDto>> CreateAuditLog(AuditLogDto auditLogDto)
        {
            var log = await _auditLogRepository.CreateAuditLogAsync(auditLogDto);
            return Ok(log);
        }
    }

    #endregion

    // Helper class for UserRoleController
    public class UserRoleAssignDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
