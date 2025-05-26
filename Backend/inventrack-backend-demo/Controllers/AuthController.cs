using inventrack_backend_demo.Data;
using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using inventrack_backend_demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace inventrack_backend_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ITwoFactorService _twoFactorService;

        public AuthController(
            ApplicationDbContext context,
            IJwtService jwtService,
            ITwoFactorService twoFactorService)
        {
            _context = context;
            _jwtService = jwtService;
            _twoFactorService = twoFactorService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("User with this email already exists");
            }

            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return BadRequest("Username already taken");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                PhoneNumber = registerDto.PhoneNumber,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            if (registerDto.Profile != null)
            {
                user.Profile = new UserProfile
                {
                    FirstName = registerDto.Profile.FirstName,
                    LastName = registerDto.Profile.LastName,
                    Address = registerDto.Profile.Address,
                    City = registerDto.Profile.City,
                    State = registerDto.Profile.State,
                    ZipCode = registerDto.Profile.ZipCode,
                    Country = registerDto.Profile.Country
                };
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Assign default role (if exists)
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
            if (defaultRole != null)
            {
                var userRole = new UserRole
                {
                    UserId = user.UserId,
                    RoleId = defaultRole.RoleId
                };
                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();
            }

            // Load user with roles for token generation
            var userWithRoles = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);

            var token = _jwtService.GenerateToken(userWithRoles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Store refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.UserId
            };
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = new UserDto
                {
                    UserId = userWithRoles.UserId,
                    Username = userWithRoles.Username,
                    Email = userWithRoles.Email,
                    PhoneNumber = userWithRoles.PhoneNumber,
                    IsActive = userWithRoles.IsActive,
                    CreatedDate = userWithRoles.CreatedDate,
                    TwoFactorEnabled = userWithRoles.TwoFactorEnabled
                },
                RequiresTwoFactor = false
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }

            if (!user.IsActive)
            {
                return Unauthorized("Account is deactivated");
            }

            // Check if 2FA is enabled
            if (user.TwoFactorEnabled)
            {
                if (string.IsNullOrEmpty(loginDto.TwoFactorCode))
                {
                    return Ok(new AuthResponseDto
                    {
                        RequiresTwoFactor = true,
                        User = new UserDto
                        {
                            UserId = user.UserId,
                            Email = user.Email
                        }
                    });
                }

                var isValidCode = await _twoFactorService.VerifyCodeAsync(user.UserId, loginDto.TwoFactorCode);
                if (!isValidCode)
                {
                    return Unauthorized("Invalid two-factor authentication code");
                }
            }

            // Update last login date
            user.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Store refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.UserId
            };
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    CreatedDate = user.CreatedDate,
                    LastLoginDate = user.LastLoginDate,
                    TwoFactorEnabled = user.TwoFactorEnabled
                },
                RequiresTwoFactor = false
            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var authResponse = await _jwtService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId != null)
            {
                // Revoke all refresh tokens for this user
                var refreshTokens = await _context.RefreshTokens
                    .Where(rt => rt.UserId == int.Parse(userId) && !rt.IsRevoked)
                    .ToListAsync();

                foreach (var token in refreshTokens)
                {
                    token.IsRevoked = true;
                }

                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
                return Unauthorized();

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                return BadRequest("Current password is incorrect");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully" });
        }
    }
}
