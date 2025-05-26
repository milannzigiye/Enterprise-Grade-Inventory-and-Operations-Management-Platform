using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace inventrack_backend_demo.Model
{
    #region Supplier Management Domain

    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        public int? UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string ContactName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(20)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string TaxIdentificationNumber { get; set; }

        [StringLength(50)]
        public string PaymentTerms { get; set; }

        public int LeadTimeInDays { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime OnboardingDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastOrderDate { get; set; }

        public decimal? PerformanceRating { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<SupplierPerformance> PerformanceRecords { get; set; }
    }

    public class SupplierProduct
    {
        [Key]
        public int SupplierProductId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [StringLength(50)]
        public string SupplierSKU { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        public int MinimumOrderQuantity { get; set; }

        public bool IsPreferredSupplier { get; set; }

        public int LeadTimeInDays { get; set; }

        public DateTime? LastPurchaseDate { get; set; }

        // Navigation properties
        public virtual Supplier Supplier { get; set; }
        public virtual Product Product { get; set; }
    }

    public class PurchaseOrder
    {
        [Key]
        public int POId { get; set; }

        [Required]
        [StringLength(20)]
        public string PONumber { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpectedDeliveryDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(50)]
        public string ShippingMethod { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        public int? ApprovedByUserId { get; set; }

        public DateTime? ApprovalDate { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; }

        // Navigation properties
        public virtual Supplier Supplier { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ApprovedByUser { get; set; }
        public virtual ICollection<PurchaseOrderItem> Items { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
        public DateTime ActualDeliveryDate { get; internal set; }
    }

    public class PurchaseOrderItem
    {
        [Key]
        public int POItemId { get; set; }

        [Required]
        public int POId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int QuantityOrdered { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }

        public int QuantityReceived { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        // Navigation properties
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Product Product { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }

    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }

        [Required]
        public int POId { get; set; }

        public DateTime ShipmentDate { get; set; }

        public DateTime? EstimatedArrivalDate { get; set; }

        public DateTime? ActualArrivalDate { get; set; }

        [StringLength(50)]
        public string TrackingNumber { get; set; }

        [StringLength(50)]
        public string Carrier { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }

    public class SupplierPerformance
    {
        [Key]
        public int PerformanceId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(20)]
        public string MetricType { get; set; }

        public decimal Score { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual Supplier Supplier { get; set; }
    }

    #endregion
}
