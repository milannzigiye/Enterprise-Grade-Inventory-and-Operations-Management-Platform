using Microsoft.EntityFrameworkCore;
using inventrack_backend_demo.Model;
using System.Reflection;

namespace inventrack_backend_demo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region User Management Domain

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<TwoFactorSecret> TwoFactorSecrets { get; set; }

        #endregion

        #region Warehouse Management Domain

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseZone> WarehouseZones { get; set; }
        public DbSet<StorageLocation> StorageLocations { get; set; }
        public DbSet<WarehouseWorker> WarehouseWorkers { get; set; }
        public DbSet<WorkerShift> WorkerShifts { get; set; }
        public DbSet<WorkerTask> WorkerTasks { get; set; }

        #endregion

        #region Product Management Domain

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }

        #endregion

        #region Supplier Management Domain

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<SupplierPerformance> SupplierPerformances { get; set; }

        #endregion

        #region Customer Management Domain

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerMembership> CustomerMemberships { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }

        #endregion

        #region Order Management Domain

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderShipment> OrderShipments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<ReturnItem> ReturnItems { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        #endregion

        #region Analytics Domain

        public DbSet<SalesStatistics> SalesStatistics { get; set; }
        public DbSet<InventoryStatistics> InventoryStatistics { get; set; }
        public DbSet<DashboardWidget> DashboardWidgets { get; set; }
        public DbSet<Report> Reports { get; set; }

        #endregion

        #region Integration Domain

        public DbSet<IntegrationPartner> IntegrationPartners { get; set; }
        public DbSet<IntegrationConfig> IntegrationConfigs { get; set; }
        public DbSet<SyncStatus> SyncStatuses { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            #region User Management Configurations

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(u => u.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(r => r.RoleName).IsUnique();
                entity.Property(r => r.IsSystemRole).HasDefaultValue(false);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
                entity.Property(ur => ur.AssignedDate).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasOne(up => up.User)
                    .WithOne(u => u.Profile)
                    .HasForeignKey<UserProfile>(up => up.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(al => al.Timestamp).HasDefaultValueSql("GETUTCDATE()");
            });

            #endregion

            #region Warehouse Management Configurations

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.Property(w => w.IsActive).HasDefaultValue(true);
                entity.Property(w => w.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<WarehouseZone>(entity =>
            {
                entity.HasOne(wz => wz.Warehouse)
                    .WithMany(w => w.Zones)
                    .HasForeignKey(wz => wz.WarehouseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(wz => wz.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<StorageLocation>(entity =>
            {
                entity.HasOne(sl => sl.Zone)
                    .WithMany(z => z.Locations)
                    .HasForeignKey(sl => sl.ZoneId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(sl => new { sl.ZoneId, sl.Aisle, sl.Rack, sl.Bin }).IsUnique();
                entity.Property(sl => sl.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<WarehouseWorker>(entity =>
            {
                entity.HasOne(ww => ww.User)
                    .WithMany()
                    .HasForeignKey(ww => ww.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ww => ww.Warehouse)
                    .WithMany(w => w.Workers)
                    .HasForeignKey(ww => ww.WarehouseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ww => ww.Supervisor)
                    .WithMany()
                    .HasForeignKey(ww => ww.SupervisorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ww => ww.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<WorkerShift>(entity =>
            {
                entity.HasOne(ws => ws.Worker)
                    .WithMany(ww => ww.Shifts)
                    .HasForeignKey(ws => ws.WorkerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WorkerTask>(entity =>
            {
                entity.HasOne(wt => wt.Worker)
                    .WithMany(ww => ww.Tasks)
                    .HasForeignKey(wt => wt.WorkerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(wt => wt.Warehouse)
                    .WithMany(w => w.Tasks)
                    .HasForeignKey(wt => wt.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(wt => wt.AssignedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(wt => wt.Status).HasDefaultValue("Assigned");
                entity.Property(wt => wt.Priority).HasDefaultValue(3);
            });

            #endregion

            #region Product Management Configurations

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasOne(pc => pc.ParentCategory)
                    .WithMany(pc => pc.ChildCategories)
                    .HasForeignKey(pc => pc.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(pc => pc.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(p => p.SKU).IsUnique();
                entity.HasOne(p => p.Category)
                    .WithMany(pc => pc.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.IsActive).HasDefaultValue(true);
                entity.Property(p => p.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<ProductAttribute>(entity =>
            {
                entity.HasIndex(pa => pa.AttributeName).IsUnique();
            });

            modelBuilder.Entity<ProductAttributeValue>(entity =>
            {
                entity.HasOne(pav => pav.Product)
                    .WithMany(p => p.AttributeValues)
                    .HasForeignKey(pav => pav.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pav => pav.Attribute)
                    .WithMany(pa => pa.AttributeValues)
                    .HasForeignKey(pav => pav.AttributeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasIndex(pv => pv.SKU).IsUnique();
                entity.HasOne(pv => pv.Product)
                    .WithMany(p => p.Variants)
                    .HasForeignKey(pv => pv.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(pv => pv.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasOne(i => i.Product)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(i => i.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Variant)
                    .WithMany(pv => pv.Inventories)
                    .HasForeignKey(i => i.VariantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Warehouse)
                    .WithMany(w => w.Inventories)
                    .HasForeignKey(i => i.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Location)
                    .WithMany(sl => sl.Inventories)
                    .HasForeignKey(i => i.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.HasOne(it => it.Inventory)
                    .WithMany(i => i.Transactions)
                    .HasForeignKey(it => it.InventoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(it => it.SourceLocation)
                    .WithMany(sl => sl.SourceTransactions)
                    .HasForeignKey(it => it.SourceLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(it => it.DestinationLocation)
                    .WithMany(sl => sl.DestinationTransactions)
                    .HasForeignKey(it => it.DestinationLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(it => it.User)
                    .WithMany()
                    .HasForeignKey(it => it.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(it => it.TransactionDate).HasDefaultValueSql("GETUTCDATE()");
            });

            #endregion

            #region Supplier Management Configurations

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasOne(s => s.User)
                    .WithMany()
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(s => s.IsActive).HasDefaultValue(true);
                entity.Property(s => s.OnboardingDate).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<SupplierProduct>(entity =>
            {
                entity.HasOne(sp => sp.Supplier)
                    .WithMany(s => s.SupplierProducts)
                    .HasForeignKey(sp => sp.SupplierId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sp => sp.Product)
                    .WithMany(p => p.SupplierProducts)
                    .HasForeignKey(sp => sp.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasIndex(po => po.PONumber).IsUnique();
                entity.HasOne(po => po.Supplier)
                    .WithMany(s => s.PurchaseOrders)
                    .HasForeignKey(po => po.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(po => po.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(po => po.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(po => po.ApprovedByUser)
                    .WithMany()
                    .HasForeignKey(po => po.ApprovedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(po => po.OrderDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(po => po.Status).HasDefaultValue("Pending");
                entity.Property(po => po.PaymentStatus).HasDefaultValue("Pending");
            });

            modelBuilder.Entity<PurchaseOrderItem>(entity =>
            {
                entity.HasOne(poi => poi.PurchaseOrder)
                    .WithMany(po => po.Items)
                    .HasForeignKey(poi => poi.POId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(poi => poi.Product)
                    .WithMany(p => p.PurchaseOrderItems)
                    .HasForeignKey(poi => poi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(poi => poi.Warehouse)
                    .WithMany()
                    .HasForeignKey(poi => poi.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(poi => poi.Status).HasDefaultValue("Ordered");
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.HasOne(s => s.PurchaseOrder)
                    .WithMany(po => po.Shipments)
                    .HasForeignKey(s => s.POId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SupplierPerformance>(entity =>
            {
                entity.HasOne(sp => sp.Supplier)
                    .WithMany(s => s.PerformanceRecords)
                    .HasForeignKey(sp => sp.SupplierId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(sp => sp.Date).HasDefaultValueSql("GETUTCDATE()");
            });

            #endregion

            #region Customer Management Configurations

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasOne(c => c.User)
                    .WithMany()
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(c => c.JoinDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(c => c.Status).HasDefaultValue("Active");
            });

            modelBuilder.Entity<CustomerMembership>(entity =>
            {
                entity.HasOne(cm => cm.Customer)
                    .WithOne(c => c.Membership)
                    .HasForeignKey<CustomerMembership>(cm => cm.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(cm => cm.StartDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(cm => cm.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasOne(w => w.Customer)
                    .WithMany(c => c.Wishlists)
                    .HasForeignKey(w => w.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(w => w.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasOne(wi => wi.Wishlist)
                    .WithMany(w => w.Items)
                    .HasForeignKey(wi => wi.WishlistId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(wi => wi.Product)
                    .WithMany(p => p.WishlistItems)
                    .HasForeignKey(wi => wi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(wi => wi.DateAdded).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<CustomerFeedback>(entity =>
            {
                entity.HasOne(cf => cf.Customer)
                    .WithMany(c => c.Feedbacks)
                    .HasForeignKey(cf => cf.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cf => cf.Order)
                    .WithMany(o => o.Feedbacks)
                    .HasForeignKey(cf => cf.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cf => cf.Product)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(cf => cf.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(cf => cf.SubmissionDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(cf => cf.Status).HasDefaultValue("Pending");
            });

            #endregion

            #region Order Management Configurations

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(o => o.OrderNumber).IsUnique();
                entity.HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(o => o.OrderDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(o => o.Status).HasDefaultValue("Pending");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.Items)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(oi => oi.Variant)
                    .WithMany(pv => pv.OrderItems)
                    .HasForeignKey(oi => oi.VariantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(oi => oi.Warehouse)
                    .WithMany()
                    .HasForeignKey(oi => oi.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(oi => oi.FulfillmentLocation)
                    .WithMany()
                    .HasForeignKey(oi => oi.FulfillmentLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(oi => oi.Status).HasDefaultValue("Pending");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasOne(os => os.Order)
                    .WithMany(o => o.StatusHistory)
                    .HasForeignKey(os => os.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(os => os.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(os => os.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(os => os.StatusDate).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<OrderShipment>(entity =>
            {
                entity.HasIndex(os => os.ShipmentNumber).IsUnique();
                entity.HasOne(os => os.Order)
                    .WithMany(o => o.Shipments)
                    .HasForeignKey(os => os.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasOne(p => p.Order)
                    .WithMany(o => o.Payments)
                    .HasForeignKey(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Return>(entity =>
            {
                entity.HasIndex(r => r.ReturnNumber).IsUnique();
                entity.HasOne(r => r.Order)
                    .WithMany(o => o.Returns)
                    .HasForeignKey(r => r.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.ApprovedByUser)
                    .WithMany()
                    .HasForeignKey(r => r.ApprovedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(r => r.RequestDate).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<ReturnItem>(entity =>
            {
                entity.HasOne(ri => ri.Return)
                    .WithMany(r => r.Items)
                    .HasForeignKey(ri => ri.ReturnId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ri => ri.OrderItem)
                    .WithMany(oi => oi.ReturnItems)
                    .HasForeignKey(ri => ri.OrderItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasOne(d => d.Shipment)
                    .WithMany(os => os.Deliveries)
                    .HasForeignKey(d => d.ShipmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.DeliveryPerson)
                    .WithMany()
                    .HasForeignKey(d => d.DeliveryPersonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region Analytics Configurations

            modelBuilder.Entity<SalesStatistics>(entity =>
            {
                entity.HasKey(ss => ss.StatId);
            });

            modelBuilder.Entity<InventoryStatistics>(entity =>
            {
                entity.HasKey(invStats => invStats.StatId);
            });

            modelBuilder.Entity<DashboardWidget>(entity =>
            {
                entity.HasOne(dw => dw.User)
                    .WithMany()
                    .HasForeignKey(dw => dw.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(dw => dw.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasOne(r => r.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(r => r.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(r => r.CreationDate).HasDefaultValueSql("GETUTCDATE()");
            });

            #endregion

            #region Integration Configurations

            modelBuilder.Entity<IntegrationPartner>(entity =>
            {
                entity.Property(ip => ip.Status).HasDefaultValue("Active");
            });

            modelBuilder.Entity<IntegrationConfig>(entity =>
            {
                entity.HasOne(ic => ic.Partner)
                    .WithMany(ip => ip.Configurations)
                    .HasForeignKey(ic => ic.PartnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SyncStatus>(entity =>
            {
                entity.HasOne(ss => ss.Partner)
                    .WithMany(ip => ip.SyncStatuses)
                    .HasForeignKey(ss => ss.PartnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion
        }
    }
}