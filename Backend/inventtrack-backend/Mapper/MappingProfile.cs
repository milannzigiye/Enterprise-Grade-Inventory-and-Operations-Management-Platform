using AutoMapper;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using System.Collections.Generic;
using System.Linq;

namespace inventtrack_backend.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Management
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles ?? new List<UserRole>()))
                .ReverseMap();
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions != null ? src.RolePermissions.Select(rp => rp.Permission) : new List<Permission>()))
                .ReverseMap();
            CreateMap<UserRole, UserRoleDto>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<AuditLog, AuditLogDto>().ReverseMap();
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();

            // Warehouse Management
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones ?? new List<WarehouseZone>()))
                .ForMember(dest => dest.Workers, opt => opt.MapFrom(src => src.Workers ?? new List<WarehouseWorker>()))
                .ReverseMap();
            CreateMap<WarehouseZone, WarehouseZoneDto>()
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations ?? new List<StorageLocation>()))
                .ReverseMap();
            CreateMap<StorageLocation, StorageLocationDto>().ReverseMap();
            CreateMap<WarehouseWorker, WarehouseWorkerDto>().ReverseMap();
            CreateMap<WorkerShift, WorkerShiftDto>().ReverseMap();
            CreateMap<WorkerTask, WorkerTaskDto>().ReverseMap();

            // Product Management
            CreateMap<ProductCategory, ProductCategoryDto>()
                .ForMember(dest => dest.ChildCategories, opt => opt.MapFrom(src => src.ChildCategories ?? new List<ProductCategory>()))
                .ReverseMap();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants ?? new List<ProductVariant>()))
                .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues ?? new List<ProductAttributeValue>()))
                .ReverseMap();
            CreateMap<ProductAttribute, ProductAttributeDto>().ReverseMap();
            CreateMap<ProductAttributeValue, ProductAttributeValueDto>().ReverseMap();
            CreateMap<ProductVariant, ProductVariantDto>().ReverseMap();
            CreateMap<Inventory, InventoryDto>()
                .ForMember(dest => dest.QuantityAvailable, opt => opt.MapFrom(src => src.QuantityOnHand - src.QuantityReserved))
                .ReverseMap();
            CreateMap<InventoryTransaction, InventoryTransactionDto>().ReverseMap();

            // Supplier Management
            CreateMap<Supplier, SupplierDto>()
                .ForMember(dest => dest.SupplierProducts, opt => opt.MapFrom(src => src.SupplierProducts ?? new List<SupplierProduct>()))
                .ReverseMap();
            CreateMap<SupplierProduct, SupplierProductDto>().ReverseMap();
            CreateMap<PurchaseOrder, PurchaseOrderDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items ?? new List<PurchaseOrderItem>()))
                .ReverseMap();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>().ReverseMap();
            CreateMap<Shipment, ShipmentDto>()
                .ForMember(dest => dest.PurchaseOrder, opt => opt.MapFrom(src => src.PurchaseOrder))
                .ReverseMap();
            CreateMap<SupplierPerformance, SupplierPerformanceDto>().ReverseMap();

            // Customer Management
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CustomerMembership, CustomerMembershipDto>().ReverseMap();
            CreateMap<Wishlist, WishlistDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items ?? new List<WishlistItem>()))
                .ReverseMap();
            CreateMap<WishlistItem, WishlistItemDto>().ReverseMap();
            CreateMap<CustomerFeedback, CustomerFeedbackDto>().ReverseMap();

            // Order Management
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items ?? new List<OrderItem>()))
                .ForMember(dest => dest.StatusHistory, opt => opt.MapFrom(src => src.StatusHistory ?? new List<OrderStatus>()))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderStatus, OrderStatusDto>().ReverseMap();
            CreateMap<OrderShipment, OrderShipmentDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Return, ReturnDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items ?? new List<ReturnItem>()))
                .ReverseMap();
            CreateMap<ReturnItem, ReturnItemDto>()
                .ForMember(dest => dest.OrderItem, opt => opt.MapFrom(src => src.OrderItem))
                .ReverseMap();
            CreateMap<Delivery, DeliveryDto>().ReverseMap();

            // Analytics
            CreateMap<SalesStatistics, SalesStatisticsDto>().ReverseMap();
            CreateMap<InventoryStatistics, InventoryStatisticsDto>().ReverseMap();
            CreateMap<DashboardWidget, DashboardWidgetDto>().ReverseMap();
            CreateMap<Report, ReportDto>().ReverseMap();

            // Integration
            CreateMap<IntegrationPartner, IntegrationPartnerDto>()
                .ForMember(dest => dest.Configurations, opt => opt.MapFrom(src => src.Configurations ?? new List<IntegrationConfig>()))
                .ReverseMap();
            CreateMap<IntegrationConfig, IntegrationConfigDto>().ReverseMap();
            CreateMap<SyncStatus, SyncStatusDto>().ReverseMap();

            // Create DTOs
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserProfileCreateDto, UserProfile>();
            CreateMap<UserProfileUpdateDto, UserProfile>();
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>();
            CreateMap<WarehouseCreateDto, Warehouse>();
            CreateMap<WarehouseUpdateDto, Warehouse>();
            CreateMap<WarehouseZoneCreateDto, WarehouseZone>();
            CreateMap<StorageLocationCreateDto, StorageLocation>();
            CreateMap<WarehouseWorkerCreateDto, WarehouseWorker>();
            CreateMap<WorkerShiftCreateDto, WorkerShift>();
            CreateMap<WorkerTaskCreateDto, WorkerTask>();
            CreateMap<ProductCategoryCreateDto, ProductCategory>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<ProductAttributeValueCreateDto, ProductAttributeValue>();
            CreateMap<ProductVariantCreateDto, ProductVariant>();
            CreateMap<SupplierCreateDto, Supplier>();
            CreateMap<SupplierProductCreateDto, SupplierProduct>();
            CreateMap<PurchaseOrderCreateDto, PurchaseOrder>();
            CreateMap<PurchaseOrderItemCreateDto, PurchaseOrderItem>();
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<WishlistCreateDto, Wishlist>();
            CreateMap<WishlistItemCreateDto, WishlistItem>();
            CreateMap<OrderCreateDto, Order>();
            CreateMap<OrderItemCreateDto, OrderItem>();
            CreateMap<InventoryUpdateDto, Inventory>();
        }
    }
}