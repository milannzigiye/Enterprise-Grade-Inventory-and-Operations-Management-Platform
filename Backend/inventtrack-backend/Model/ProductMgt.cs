using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace inventtrack_backend.Model
{
    #region Product Management Domain

    public class ProductCategory
    {
        [Key]
        public int CategoryId { get; set; }

        public int? ParentCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ProductCategory ParentCategory { get; set; }
        public virtual ICollection<ProductCategory> ChildCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(20)]
        public string UnitOfMeasure { get; set; }

        public decimal? Weight { get; set; }

        [StringLength(50)]
        public string Dimensions { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ListPrice { get; set; }

        public int MinimumStockLevel { get; set; }

        public int ReorderQuantity { get; set; }

        public int LeadTimeInDays { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        // Navigation properties
        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<ProductVariant> Variants { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<ProductAttributeValue> AttributeValues { get; set; }
        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public virtual ICollection<CustomerFeedback> Feedbacks { get; set; }
        public virtual ICollection<WishlistItem> WishlistItems { get; set; }
    }

    public class ProductAttribute
    {
        [Key]
        public int AttributeId { get; set; }

        [Required]
        [StringLength(50)]
        public string AttributeName { get; set; }

        [Required]
        [StringLength(20)]
        public string AttributeType { get; set; }

        // Navigation properties
        public virtual ICollection<ProductAttributeValue> AttributeValues { get; set; }
    }

    public class ProductAttributeValue
    {
        [Key]
        public int ValueId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int AttributeId { get; set; }

        [Required]
        public string Value { get; set; }

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual ProductAttribute Attribute { get; set; }
    }

    public class ProductVariant
    {
        [Key]
        public int VariantId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string VariantName { get; set; }

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalCost { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int? VariantId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int LocationId { get; set; }

        public int QuantityOnHand { get; set; }

        public int QuantityReserved { get; set; }

        [NotMapped]
        public int QuantityAvailable => QuantityOnHand - QuantityReserved;

        public DateTime? LastCountDate { get; set; }

        public DateTime? LastReceivedDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(50)]
        public string LotNumber { get; set; }

        [StringLength(100)]
        public string SerialNumber { get; set; }

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual ProductVariant Variant { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual StorageLocation Location { get; set; }
        public virtual ICollection<InventoryTransaction> Transactions { get; set; }
    }

    public class InventoryTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int InventoryId { get; set; }

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int PreviousQuantity { get; set; }

        public int NewQuantity { get; set; }

        public int? SourceLocationId { get; set; }

        public int? DestinationLocationId { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public int? UserId { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual Inventory Inventory { get; set; }
        public virtual StorageLocation SourceLocation { get; set; }
        public virtual StorageLocation DestinationLocation { get; set; }
        public virtual User User { get; set; }
    }

    #endregion
}
