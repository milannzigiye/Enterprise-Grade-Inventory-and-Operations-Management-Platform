namespace inventrack_backend_demo.DTOs
{
    #region Order Management DTOs

    public class OrderDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderSource { get; set; }
        public string Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public CustomerDto Customer { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public List<OrderStatusDto> StatusHistory { get; set; }
    }

    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public string OrderSource { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        public List<OrderItemCreateDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public int WarehouseId { get; set; }

        // Navigation properties
        public ProductDto Product { get; set; }
        public ProductVariantDto Variant { get; set; }
        public WarehouseDto Warehouse { get; set; }
    }

    public class OrderItemCreateDto
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int WarehouseId { get; set; }
    }

    public class OrderStatusDto
    {
        public int StatusId { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string Notes { get; set; }
        public int? UpdatedByUserId { get; set; }
    }

    public class OrderShipmentDto
    {
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public string Status { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public string DeliveredTo { get; set; }
        public bool SignatureRequired { get; set; }
        public string ProofOfDeliveryUrl { get; set; }

        // Navigation properties
        public OrderDto Order { get; set; }
    }

    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string AuthorizationCode { get; set; }
        public decimal? RefundAmount { get; set; }
        public DateTime? RefundDate { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public OrderDto Order { get; set; }
    }

    public class ReturnDto
    {
        public int ReturnId { get; set; }
        public int OrderId { get; set; }
        public string ReturnNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string ReturnReason { get; set; }
        public string ReturnType { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal? RefundAmount { get; set; }
        public DateTime? RefundDate { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public OrderDto Order { get; set; }
        public UserDto ApprovedByUser { get; set; }
        public List<ReturnItemDto> Items { get; set; }
    }

    public class ReturnItemDto
    {
        public int ReturnItemId { get; set; }
        public int ReturnId { get; set; }
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public string ReturnReason { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public OrderItemDto OrderItem { get; set; }
    }

    public class DeliveryDto
    {
        public int DeliveryId { get; set; }
        public int ShipmentId { get; set; }
        public int DeliveryPersonId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string EstimatedTimeWindow { get; set; }
        public string Status { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public string GeoLocation { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public OrderShipmentDto Shipment { get; set; }
        public UserDto DeliveryPerson { get; set; }
    }

    #endregion
}
