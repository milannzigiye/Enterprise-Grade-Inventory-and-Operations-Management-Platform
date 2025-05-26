namespace inventrack_backend_demo.DTOs
{
    #region Product Management DTOs

    public class ProductCategoryDto
    {
        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<ProductCategoryDto> ChildCategories { get; set; }
    }

    public class ProductCategoryCreateDto
    {
        public int? ParentCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    public class ProductDto
    {
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Weight { get; set; }
        public string Dimensions { get; set; }
        public decimal UnitCost { get; set; }
        public decimal ListPrice { get; set; }
        public int MinimumStockLevel { get; set; }
        public int ReorderQuantity { get; set; }
        public int LeadTimeInDays { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ImageUrl { get; set; }
        public ProductCategoryDto Category { get; set; }
        public List<ProductVariantDto> Variants { get; set; }
        public List<ProductAttributeValueDto> AttributeValues { get; set; }
    }

    public class ProductCreateDto
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Weight { get; set; }
        public string Dimensions { get; set; }
        public decimal UnitCost { get; set; }
        public decimal ListPrice { get; set; }
        public int MinimumStockLevel { get; set; }
        public int ReorderQuantity { get; set; }
        public int LeadTimeInDays { get; set; }
        public string ImageUrl { get; set; }
        public List<ProductAttributeValueCreateDto> AttributeValues { get; set; }
    }

    public class ProductUpdateDto
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Weight { get; set; }
        public string Dimensions { get; set; }
        public decimal UnitCost { get; set; }
        public decimal ListPrice { get; set; }
        public int MinimumStockLevel { get; set; }
        public int ReorderQuantity { get; set; }
        public int LeadTimeInDays { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ProductAttributeDto
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeType { get; set; }
    }

    public class ProductAttributeValueDto
    {
        public int ValueId { get; set; }
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }

        // Navigation properties
        public ProductAttributeDto Attribute { get; set; }
    }

    public class ProductAttributeValueCreateDto
    {
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }
    }

    public class ProductVariantDto
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string VariantName { get; set; }
        public string SKU { get; set; }
        public decimal AdditionalCost { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductVariantCreateDto
    {
        public int ProductId { get; set; }
        public string VariantName { get; set; }
        public string SKU { get; set; }
        public decimal AdditionalCost { get; set; }
    }

    public class InventoryDto
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public int WarehouseId { get; set; }
        public int LocationId { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public int QuantityAvailable { get; set; }
        public DateTime? LastCountDate { get; set; }
        public DateTime? LastReceivedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string LotNumber { get; set; }
        public string SerialNumber { get; set; }
        public ProductDto Product { get; set; }
        public ProductVariantDto Variant { get; set; }
        public WarehouseDto Warehouse { get; set; }
        public StorageLocationDto Location { get; set; }
    }

    public class InventoryUpdateDto
    {
        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string LotNumber { get; set; }
        public string SerialNumber { get; set; }
    }

    public class InventoryTransactionDto
    {
        public int TransactionId { get; set; }
        public int InventoryId { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public int PreviousQuantity { get; set; }
        public int NewQuantity { get; set; }
        public int? SourceLocationId { get; set; }
        public int? DestinationLocationId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? UserId { get; set; }
        public string Notes { get; set; }
    }

    #endregion
}
