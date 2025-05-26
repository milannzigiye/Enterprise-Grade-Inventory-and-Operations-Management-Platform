using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventrack_backend_demo.Model
{
    #region Order Management Domain

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderNumber { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string OrderSource { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; }

        [StringLength(500)]
        public string BillingAddress { get; set; }

        [StringLength(500)]
        public string ShippingAddress { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
        public virtual ICollection<OrderStatus> StatusHistory { get; set; }
        public virtual ICollection<OrderShipment> Shipments { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Return> Returns { get; set; }
        public virtual ICollection<CustomerFeedback> Feedbacks { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int? VariantId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public int WarehouseId { get; set; }

        public int? FulfillmentLocationId { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductVariant Variant { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual StorageLocation FulfillmentLocation { get; set; }
        public virtual ICollection<ReturnItem> ReturnItems { get; set; }
    }

    public class OrderStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime StatusDate { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string Notes { get; set; }

        public int? UpdatedByUserId { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual User UpdatedByUser { get; set; }
    }

    public class OrderShipment
    {
        [Key]
        public int ShipmentId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string ShipmentNumber { get; set; }

        public DateTime ShipmentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Carrier { get; set; }

        [StringLength(100)]
        public string TrackingNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        [StringLength(100)]
        public string DeliveredTo { get; set; }

        public bool SignatureRequired { get; set; }

        [StringLength(255)]
        public string ProofOfDeliveryUrl { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
    }

    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string TransactionId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime PaymentDate { get; set; }

        [StringLength(100)]
        public string AuthorizationCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; }

        public DateTime? RefundDate { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
    }

    public class Return
    {
        [Key]
        public int ReturnId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(20)]
        public string ReturnNumber { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(255)]
        public string ReturnReason { get; set; }

        [Required]
        [StringLength(20)]
        public string ReturnType { get; set; }

        public int? ApprovedByUserId { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public DateTime? ReceivedDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; }

        public DateTime? RefundDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual User ApprovedByUser { get; set; }
        public virtual ICollection<ReturnItem> Items { get; set; }
    }

    public class ReturnItem
    {
        [Key]
        public int ReturnItemId { get; set; }

        [Required]
        public int ReturnId { get; set; }

        [Required]
        public int OrderItemId { get; set; }

        public int Quantity { get; set; }

        [StringLength(255)]
        public string ReturnReason { get; set; }

        [StringLength(50)]
        public string Condition { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        // Navigation properties
        public virtual Return Return { get; set; }
        public virtual OrderItem OrderItem { get; set; }
    }

    public class Delivery
    {
        [Key]
        public int DeliveryId { get; set; }

        [Required]
        public int ShipmentId { get; set; }

        [Required]
        public int DeliveryPersonId { get; set; }

        public DateTime ScheduledDate { get; set; }

        [StringLength(50)]
        public string EstimatedTimeWindow { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime? ActualDeliveryTime { get; set; }

        [StringLength(100)]
        public string GeoLocation { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual OrderShipment Shipment { get; set; }
        public virtual User DeliveryPerson { get; set; }
    }

    #endregion
}
