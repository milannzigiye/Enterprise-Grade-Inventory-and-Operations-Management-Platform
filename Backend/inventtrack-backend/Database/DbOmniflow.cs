using inventtrack_backend.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace inventtrack_backend.Database
{
    public class DbOmniflow : DbContext
    {
        public DbOmniflow(DbContextOptions<DbOmniflow> options)
            : base(options)
        {
        }

        // User Management Domain
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Warehouse Management Domain
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseZone> WarehouseZones { get; set; }
        public DbSet<StorageLocation> StorageLocations { get; set; }
        public DbSet<WarehouseWorker> WarehouseWorkers { get; set; }
        public DbSet<WorkerShift> WorkerShifts { get; set; }
        public DbSet<WorkerTask> WorkerTasks { get; set; }

        // Product Management Domain
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }

        // Supplier Management Domain
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Shipment> PurchaseShipments { get; set; }
        public DbSet<SupplierPerformance> SupplierPerformances { get; set; }

        // Customer Management Domain
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerMembership> CustomerMemberships { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }

        // Order Management Domain
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Shipment> OrderShipments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<ReturnItem> ReturnItems { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        // Analytics Domain
        public DbSet<SalesStatistics> SalesStatistics { get; set; }
        public DbSet<InventoryStatistics> InventoryStatistics { get; set; }
        public DbSet<DashboardWidget> DashboardWidgets { get; set; }
        public DbSet<Report> Reports { get; set; }

        // Integration Domain
        public DbSet<IntegrationPartner> IntegrationPartners { get; set; }
        public DbSet<IntegrationConfig> IntegrationConfigs { get; set; }
        public DbSet<SyncStatus> SyncStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Inventory configuration
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Variant)
                .WithMany()
                .HasForeignKey(i => i.VariantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Warehouse)
                .WithMany()
                .HasForeignKey(i => i.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Location)
                .WithMany()
                .HasForeignKey(i => i.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Inventory Transaction configuration
            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.Inventory)
                .WithMany()
                .HasForeignKey(t => t.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product Variant configuration
            modelBuilder.Entity<ProductVariant>()
                .HasOne(v => v.Product)
                .WithMany()
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Storage Location configuration
            modelBuilder.Entity<StorageLocation>()
                .HasOne(l => l.Warehouse)
                .WithMany()
                .HasForeignKey(l => l.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


    // Extension for soft delete
    public static class SoftDeleteQueryExtension
    {
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension)
                .GetMethod(nameof(GetSoftDeleteFilter),
                BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityData.ClrType);
            var filter = methodToCall?.Invoke(null, new object[] { });
            entityData.SetQueryFilter((LambdaExpression)filter);
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }

    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
