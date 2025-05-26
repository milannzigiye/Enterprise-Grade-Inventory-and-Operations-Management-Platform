namespace inventrack_backend_demo.DTOs
{
    #region Supplier Management DTOs

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
        public decimal? PerformanceRating { get; set; }
        public List<SupplierProductDto> SupplierProducts { get; set; }
    }

    public class SupplierCreateDto
    {
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
    }

    public class SupplierProductDto
    {
        public int SupplierProductId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public string SupplierSKU { get; set; }
        public decimal UnitCost { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public bool IsPreferredSupplier { get; set; }
        public int LeadTimeInDays { get; set; }
        public DateTime? LastPurchaseDate { get; set; }

        // Navigation properties
        public ProductDto Product { get; set; }
    }

    public class SupplierProductCreateDto
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public string SupplierSKU { get; set; }
        public decimal UnitCost { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public bool IsPreferredSupplier { get; set; }
        public int LeadTimeInDays { get; set; }
    }

    public class PurchaseOrderDto
    {
        public int POId { get; set; }
        public string PONumber { get; set; }
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }
        public string ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserId { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public SupplierDto Supplier { get; set; }
        public List<PurchaseOrderItemDto> Items { get; set; }
    }

    public class PurchaseOrderCreateDto
    {
        public int SupplierId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string ShippingMethod { get; set; }
        public string Notes { get; set; }
        public List<PurchaseOrderItemCreateDto> Items { get; set; }
    }

    public class PurchaseOrderItemDto
    {
        public int POItemId { get; set; }
        public int POId { get; set; }
        public int ProductId { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public int? QuantityReceived { get; set; }
        public int WarehouseId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public ProductDto Product { get; set; }
        public WarehouseDto Warehouse { get; set; }
    }

    public class PurchaseOrderItemCreateDto
    {
        public int ProductId { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal UnitCost { get; set; }
        public int WarehouseId { get; set; }
    }

    public class ShipmentDto
    {
        public int ShipmentId { get; set; }
        public int POId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public DateTime? ActualArrivalDate { get; set; }
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public PurchaseOrderDto PurchaseOrder { get; set; }
    }

    public class SupplierPerformanceDto
    {
        public int PerformanceId { get; set; }
        public int SupplierId { get; set; }
        public string MetricType { get; set; }
        public decimal Score { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }

    #endregion
}
