using inventrack_backend_demo.Data;
using inventrack_backend_demo.Model;
using Microsoft.EntityFrameworkCore;

namespace inventrack_backend_demo.Services
{
    public class DataSeedingService
    {
        private readonly ApplicationDbContext _context;

        public DataSeedingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedDemoDataAsync()
        {
            // Check if data already exists
            if (await _context.Users.AnyAsync())
            {
                return; // Data already seeded
            }

            // Create default roles
            var adminRole = new Role
            {
                RoleName = "Admin",
                Description = "Administrator with full access",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var userRole = new Role
            {
                RoleName = "User",
                Description = "Standard user with limited access",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var managerRole = new Role
            {
                RoleName = "Manager",
                Description = "Manager with elevated access",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _context.Roles.AddRange(adminRole, userRole, managerRole);
            await _context.SaveChangesAsync();

            // Create demo users
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@inventrackpro.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                PhoneNumber = "+1-555-0001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                TwoFactorEnabled = false,
                Profile = new UserProfile
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Address = "123 Admin Street",
                    City = "Admin City",
                    State = "AC",
                    ZipCode = "12345",
                    Country = "USA",
                    PreferredLanguage = "en-US",
                    TimeZone = "UTC-5"
                }
            };

            var demoUser = new User
            {
                Username = "demo",
                Email = "demo@inventrackpro.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Demo123!"),
                PhoneNumber = "+1-555-0002",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                TwoFactorEnabled = false,
                Profile = new UserProfile
                {
                    FirstName = "Demo",
                    LastName = "User",
                    Address = "456 Demo Avenue",
                    City = "Demo City",
                    State = "DC",
                    ZipCode = "67890",
                    Country = "USA",
                    PreferredLanguage = "en-US",
                    TimeZone = "UTC-5"
                }
            };

            var managerUser = new User
            {
                Username = "manager",
                Email = "manager@inventrackpro.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager123!"),
                PhoneNumber = "+1-555-0003",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                TwoFactorEnabled = false,
                Profile = new UserProfile
                {
                    FirstName = "Manager",
                    LastName = "User",
                    Address = "789 Manager Boulevard",
                    City = "Manager City",
                    State = "MC",
                    ZipCode = "54321",
                    Country = "USA",
                    PreferredLanguage = "en-US",
                    TimeZone = "UTC-5"
                }
            };

            _context.Users.AddRange(adminUser, demoUser, managerUser);
            await _context.SaveChangesAsync();

            // Assign roles to users
            var userRoles = new List<UserRole>
            {
                new UserRole { UserId = adminUser.UserId, RoleId = adminRole.RoleId },
                new UserRole { UserId = demoUser.UserId, RoleId = userRole.RoleId },
                new UserRole { UserId = managerUser.UserId, RoleId = managerRole.RoleId }
            };

            _context.UserRoles.AddRange(userRoles);
            await _context.SaveChangesAsync();

            // Create some demo permissions
            var permissions = new List<Permission>
            {
                new Permission { PermissionName = "ViewDashboard", Description = "View dashboard", Module = "Dashboard", Action = "View" },
                new Permission { PermissionName = "ManageProducts", Description = "Manage products", Module = "Products", Action = "Manage" },
                new Permission { PermissionName = "ManageOrders", Description = "Manage orders", Module = "Orders", Action = "Manage" },
                new Permission { PermissionName = "ManageUsers", Description = "Manage users", Module = "Users", Action = "Manage" },
                new Permission { PermissionName = "ViewReports", Description = "View reports", Module = "Analytics", Action = "View" }
            };

            _context.Permissions.AddRange(permissions);
            await _context.SaveChangesAsync();

            // Assign permissions to roles
            var rolePermissions = new List<RolePermission>();

            // Admin gets all permissions
            foreach (var permission in permissions)
            {
                rolePermissions.Add(new RolePermission { RoleId = adminRole.RoleId, PermissionId = permission.PermissionId });
            }

            // Manager gets most permissions except user management
            var managerPermissions = permissions.Where(p => p.PermissionName != "ManageUsers");
            foreach (var permission in managerPermissions)
            {
                rolePermissions.Add(new RolePermission { RoleId = managerRole.RoleId, PermissionId = permission.PermissionId });
            }

            // User gets basic permissions
            var userPermissions = permissions.Where(p => p.PermissionName == "ViewDashboard" || p.PermissionName == "ViewReports");
            foreach (var permission in userPermissions)
            {
                rolePermissions.Add(new RolePermission { RoleId = userRole.RoleId, PermissionId = permission.PermissionId });
            }

            _context.RolePermissions.AddRange(rolePermissions);
            await _context.SaveChangesAsync();
        }
    }
}
