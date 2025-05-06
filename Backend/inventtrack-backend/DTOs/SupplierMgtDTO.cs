using System.ComponentModel.DataAnnotations;

namespace inventtrack_backend.DTOs
{

    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public int? UserId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public string PaymentTerms { get; set; }
        public int LeadTimeInDays { get; set; }
        public bool IsActive { get; set; }
        public DateTime OnboardingDate { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public int PerformanceRating { get; set; }
        public int ProductCount { get; set; }
        public int OpenOrdersCount { get; set; }
        public decimal TotalPurchased { get; set; }
    }

    public class SupplierCreateDto
    {
        public int? UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
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

        [StringLength(100)]
        public string PaymentTerms { get; set; }

        [Range(1, 365)]
        public int LeadTimeInDays { get; set; }
    }

    public class SupplierUpdateDto
    {
        public int? UserId { get; set; }

        [StringLength(200)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string ContactName { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
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

        [StringLength(100)]
        public string PaymentTerms { get; set; }

        [Range(1, 365)]
        public int? LeadTimeInDays { get; set; }

        public bool? IsActive { get; set; }

        [Range(1, 5)]
        public int? PerformanceRating { get; set; }
    }

    public class SupplierProductDto
    {
        public int SupplierProductId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public string SupplierSKU { get; set; }
        public decimal UnitCost { get; set; }
        public decimal MinimumOrderQuantity { get; set; }
        public bool IsPreferredSupplier { get; set; }
        public int LeadTimeInDays { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
    }

    public class SupplierProductCreateDto
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [StringLength(50)]
        public string SupplierSKU { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitCost { get; set; }

        [Range(1, double.MaxValue)]
        public decimal MinimumOrderQuantity { get; set; } = 1;

        public bool IsPreferredSupplier { get; set; }

        [Range(1, 365)]
        public int LeadTimeInDays { get; set; }
    }

    public class SupplierProductUpdateDto
    {
        [StringLength(50)]
        public string SupplierSKU { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? UnitCost { get; set; }

        [Range(1, double.MaxValue)]
        public decimal? MinimumOrderQuantity { get; set; }

        public bool? IsPreferredSupplier { get; set; }

        [Range(1, 365)]
        public int? LeadTimeInDays { get; set; }
    }

    public class PurchaseOrderDto
    {
        public int POId { get; set; }
        public string PONumber { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }
        public string ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public int? ApprovedByUserId { get; set; }
        public string ApprovedByUserName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public ICollection<PurchaseOrderItemDto> Items { get; set; }
        public ICollection<ShipmentDto> Shipments { get; set; }
    }

    public class PurchaseOrderCreateDto
    {
        [Required]
        public int SupplierId { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        [StringLength(50)]
        public string ShippingMethod { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ShippingCost { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TaxAmount { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [Required]
        public List<PurchaseOrderItemCreateDto> Items { get; set; }
    }

    public class PurchaseOrderUpdateDto
    {
        public DateTime? ExpectedDeliveryDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string ShippingMethod { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ShippingCost { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TaxAmount { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public int? ApprovedByUserId { get; set; }

        public DateTime? ApprovalDate { get; set; }

        [StringLength(50)]
        public string PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; }
    }

    public class PurchaseOrderItemDto
    {
        public int POItemId { get; set; }
        public int POId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal QuantityReceived { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }
    }

    public class PurchaseOrderItemCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal QuantityOrdered { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitCost { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }
    }

    public class PurchaseOrderItemUpdateDto
    {
        [Range(0.01, double.MaxValue)]
        public decimal? QuantityOrdered { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? UnitCost { get; set; }

        public int? WarehouseId { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? QuantityReceived { get; set; }
    }

    public class ShipmentDto
    {
        public int ShipmentId { get; set; }
        public int POId { get; set; }
        public string PONumber { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public DateTime? ActualArrivalDate { get; set; }
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public ICollection<ShipmentItemDto> Items { get; set; }
    }

    public class ShipmentCreateDto
    {
        [Required]
        public int POId { get; set; }

        [Required]
        public DateTime ShipmentDate { get; set; }

        [Required]
        public DateTime EstimatedArrivalDate { get; set; }

        [StringLength(100)]
        public string TrackingNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Carrier { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [Required]
        public List<ShipmentItemCreateDto> Items { get; set; }
    }

    public class ShipmentUpdateDto
    {
        public DateTime? ActualArrivalDate { get; set; }

        [StringLength(100)]
        public string TrackingNumber { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }

    public class ShipmentItemDto
    {
        public int ShipmentItemId { get; set; }
        public int ShipmentId { get; set; }
        public int POItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public decimal QuantityShipped { get; set; }
        public decimal QuantityReceived { get; set; }
        public int? LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LotNumber { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ReceiptStatus { get; set; }
        public string QualityCheckStatus { get; set; }
        public string Notes { get; set; }
    }

    public class ShipmentItemCreateDto
    {
        [Required]
        public int POItemId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal QuantityShipped { get; set; }

        [StringLength(50)]
        public string LotNumber { get; set; }

        [StringLength(100)]
        public string SerialNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }

    public class ShipmentItemReceiveDto
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal QuantityReceived { get; set; }

        [Required]
        public int LocationId { get; set; }

        [StringLength(50)]
        public string QualityCheckStatus { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }

    public class SupplierPerformanceDto
    {
        public int PerformanceId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string MetricType { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }

    public class SupplierPerformanceCreateDto
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(50)]
        public string MetricType { get; set; }

        [Required]
        [Range(1, 5)]
        public int Score { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }

    public class SupplierPerformanceUpdateDto
    {
        [Range(1, 5)]
        public int? Score { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }

    public class SupplierWithDetailsDto : SupplierDto
    {
        public ICollection<SupplierProductDto> Products { get; set; }
        public ICollection<PurchaseOrderDto> PurchaseOrders { get; set; }
        public ICollection<SupplierPerformanceDto> PerformanceMetrics { get; set; }
    }

    public class SupplierStatsDto
    {
        public int TotalProducts { get; set; }
        public int TotalPurchaseOrders { get; set; }
        public decimal TotalSpend { get; set; }
        public double AvgDeliveryTimeDays { get; set; }
        public double OnTimeDeliveryPercentage { get; set; }
        public double AvgQualityScore { get; set; }
    }

}
