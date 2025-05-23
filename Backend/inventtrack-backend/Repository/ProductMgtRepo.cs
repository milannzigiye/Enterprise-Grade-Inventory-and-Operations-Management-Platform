using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using inventtrack_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace inventtrack_backend.Repositories
{
    #region Product Management Repositories

    public interface IProductCategoryRepository
    {
        Task<ProductCategoryDto> GetCategoryByIdAsync(int categoryId);
        Task<List<ProductCategoryDto>> GetAllCategoriesAsync();
        Task<List<ProductCategoryDto>> GetChildCategoriesAsync(int parentCategoryId);
        Task<ProductCategoryDto> CreateCategoryAsync(ProductCategoryCreateDto categoryCreateDto);
        Task<ProductCategoryDto> UpdateCategoryAsync(int categoryId, ProductCategoryDto categoryDto);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }

    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductCategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _context.ProductCategories
                .Include(c => c.ChildCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null) return null;

            return new ProductCategoryDto
            {
                CategoryId = category.CategoryId,
                ParentCategoryId = category.ParentCategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                IsActive = category.IsActive,
                ChildCategories = category.ChildCategories?.Select(cc => new ProductCategoryDto
                {
                    CategoryId = cc.CategoryId,
                    CategoryName = cc.CategoryName,
                    Description = cc.Description,
                    IsActive = cc.IsActive
                }).ToList()
            };
        }

        public async Task<List<ProductCategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.ProductCategories
                .Where(c => c.ParentCategoryId == null)
                .Include(c => c.ChildCategories)
                .Select(c => new ProductCategoryDto
                {
                    CategoryId = c.CategoryId,
                    ParentCategoryId = c.ParentCategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    ChildCategories = c.ChildCategories.Select(cc => new ProductCategoryDto
                    {
                        CategoryId = cc.CategoryId,
                        CategoryName = cc.CategoryName,
                        Description = cc.Description,
                        IsActive = cc.IsActive
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<List<ProductCategoryDto>> GetChildCategoriesAsync(int parentCategoryId)
        {
            return await _context.ProductCategories
                .Where(c => c.ParentCategoryId == parentCategoryId)
                .Select(c => new ProductCategoryDto
                {
                    CategoryId = c.CategoryId,
                    ParentCategoryId = c.ParentCategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    IsActive = c.IsActive
                }).ToListAsync();
        }

        public async Task<ProductCategoryDto> CreateCategoryAsync(ProductCategoryCreateDto categoryCreateDto)
        {
            var category = new ProductCategory
            {
                ParentCategoryId = categoryCreateDto.ParentCategoryId,
                CategoryName = categoryCreateDto.CategoryName,
                Description = categoryCreateDto.Description,
                IsActive = true
            };

            _context.ProductCategories.Add(category);
            await _context.SaveChangesAsync();

            return await GetCategoryByIdAsync(category.CategoryId);
        }

        public async Task<ProductCategoryDto> UpdateCategoryAsync(int categoryId, ProductCategoryDto categoryDto)
        {
            var category = await _context.ProductCategories.FindAsync(categoryId);
            if (category == null) return null;

            category.CategoryName = categoryDto.CategoryName;
            category.Description = categoryDto.Description;
            category.IsActive = categoryDto.IsActive;

            await _context.SaveChangesAsync();
            return await GetCategoryByIdAsync(categoryId);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.ProductCategories.FindAsync(categoryId);
            if (category == null) return false;

            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IProductRepository
    {
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(ProductCreateDto productCreateDto);
        Task<ProductDto> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto);
        Task<bool> DeleteProductAsync(int productId);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.AttributeValues)
                    .ThenInclude(av => av.Attribute)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null) return null;

            return new ProductDto
            {
                ProductId = product.ProductId,
                SKU = product.SKU,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                UnitOfMeasure = product.UnitOfMeasure,
                Weight = product.Weight,
                Dimensions = product.Dimensions,
                UnitCost = product.UnitCost,
                ListPrice = product.ListPrice,
                MinimumStockLevel = product.MinimumStockLevel,
                ReorderQuantity = product.ReorderQuantity,
                LeadTimeInDays = product.LeadTimeInDays,
                IsActive = product.IsActive,
                CreatedDate = product.CreatedDate,
                ModifiedDate = product.ModifiedDate,
                ImageUrl = product.ImageUrl,
                Category = new ProductCategoryDto
                {
                    CategoryId = product.Category.CategoryId,
                    CategoryName = product.Category.CategoryName
                },
                Variants = product.Variants?.Select(v => new ProductVariantDto
                {
                    VariantId = v.VariantId,
                    ProductId = v.ProductId,
                    VariantName = v.VariantName,
                    SKU = v.SKU,
                    AdditionalCost = v.AdditionalCost,
                    IsActive = v.IsActive
                }).ToList(),
                AttributeValues = product.AttributeValues?.Select(av => new ProductAttributeValueDto
                {
                    ValueId = av.ValueId,
                    ProductId = av.ProductId,
                    AttributeId = av.AttributeId,
                    Value = av.Value,
                    Attribute = new ProductAttributeDto
                    {
                        AttributeId = av.Attribute.AttributeId,
                        AttributeName = av.Attribute.AttributeName,
                        AttributeType = av.Attribute.AttributeType
                    }
                }).ToList()
            };
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    SKU = p.SKU,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    UnitOfMeasure = p.UnitOfMeasure,
                    UnitCost = p.UnitCost,
                    ListPrice = p.ListPrice,
                    IsActive = p.IsActive,
                    Category = new ProductCategoryDto
                    {
                        CategoryId = p.Category.CategoryId,
                        CategoryName = p.Category.CategoryName
                    }
                }).ToListAsync();
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    SKU = p.SKU,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    UnitOfMeasure = p.UnitOfMeasure,
                    UnitCost = p.UnitCost,
                    ListPrice = p.ListPrice,
                    IsActive = p.IsActive,
                    Category = new ProductCategoryDto
                    {
                        CategoryId = p.Category.CategoryId,
                        CategoryName = p.Category.CategoryName
                    }
                }).ToListAsync();
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            var product = new Product
            {
                SKU = productCreateDto.SKU,
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                CategoryId = productCreateDto.CategoryId,
                UnitOfMeasure = productCreateDto.UnitOfMeasure,
                Weight = productCreateDto.Weight,
                Dimensions = productCreateDto.Dimensions,
                UnitCost = productCreateDto.UnitCost,
                ListPrice = productCreateDto.ListPrice,
                MinimumStockLevel = productCreateDto.MinimumStockLevel,
                ReorderQuantity = productCreateDto.ReorderQuantity,
                LeadTimeInDays = productCreateDto.LeadTimeInDays,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ImageUrl = productCreateDto.ImageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (productCreateDto.AttributeValues != null && productCreateDto.AttributeValues.Any())
            {
                foreach (var attrValue in productCreateDto.AttributeValues)
                {
                    _context.ProductAttributeValues.Add(new ProductAttributeValue
                    {
                        ProductId = product.ProductId,
                        AttributeId = attrValue.AttributeId,
                        Value = attrValue.Value
                    });
                }
                await _context.SaveChangesAsync();
            }

            return await GetProductByIdAsync(product.ProductId);
        }

        public async Task<ProductDto> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return null;

            product.SKU = productUpdateDto.SKU;
            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.CategoryId = productUpdateDto.CategoryId;
            product.UnitOfMeasure = productUpdateDto.UnitOfMeasure;
            product.Weight = productUpdateDto.Weight;
            product.Dimensions = productUpdateDto.Dimensions;
            product.UnitCost = productUpdateDto.UnitCost;
            product.ListPrice = productUpdateDto.ListPrice;
            product.MinimumStockLevel = productUpdateDto.MinimumStockLevel;
            product.ReorderQuantity = productUpdateDto.ReorderQuantity;
            product.LeadTimeInDays = productUpdateDto.LeadTimeInDays;
            product.IsActive = productUpdateDto.IsActive;
            product.ModifiedDate = DateTime.UtcNow;
            product.ImageUrl = productUpdateDto.ImageUrl;

            await _context.SaveChangesAsync();
            return await GetProductByIdAsync(productId);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            product.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IProductAttributeRepository
    {
        Task<List<ProductAttributeDto>> GetAllAttributesAsync();
        Task<ProductAttributeDto> GetAttributeByIdAsync(int attributeId);
        Task<ProductAttributeDto> CreateAttributeAsync(ProductAttributeDto attributeDto);
        Task<ProductAttributeDto> UpdateAttributeAsync(int attributeId, ProductAttributeDto attributeDto);
        Task<bool> DeleteAttributeAsync(int attributeId);
    }

    public class ProductAttributeRepository : IProductAttributeRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductAttributeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductAttributeDto>> GetAllAttributesAsync()
        {
            return await _context.ProductAttributes
                .Select(a => new ProductAttributeDto
                {
                    AttributeId = a.AttributeId,
                    AttributeName = a.AttributeName,
                    AttributeType = a.AttributeType
                }).ToListAsync();
        }

        public async Task<ProductAttributeDto> GetAttributeByIdAsync(int attributeId)
        {
            var attribute = await _context.ProductAttributes.FindAsync(attributeId);
            if (attribute == null) return null;

            return new ProductAttributeDto
            {
                AttributeId = attribute.AttributeId,
                AttributeName = attribute.AttributeName,
                AttributeType = attribute.AttributeType
            };
        }

        public async Task<ProductAttributeDto> CreateAttributeAsync(ProductAttributeDto attributeDto)
        {
            var attribute = new ProductAttribute
            {
                AttributeName = attributeDto.AttributeName,
                AttributeType = attributeDto.AttributeType
            };

            _context.ProductAttributes.Add(attribute);
            await _context.SaveChangesAsync();

            return new ProductAttributeDto
            {
                AttributeId = attribute.AttributeId,
                AttributeName = attribute.AttributeName,
                AttributeType = attribute.AttributeType
            };
        }

        public async Task<ProductAttributeDto> UpdateAttributeAsync(int attributeId, ProductAttributeDto attributeDto)
        {
            var attribute = await _context.ProductAttributes.FindAsync(attributeId);
            if (attribute == null) return null;

            attribute.AttributeName = attributeDto.AttributeName;
            attribute.AttributeType = attributeDto.AttributeType;

            await _context.SaveChangesAsync();
            return new ProductAttributeDto
            {
                AttributeId = attribute.AttributeId,
                AttributeName = attribute.AttributeName,
                AttributeType = attribute.AttributeType
            };
        }

        public async Task<bool> DeleteAttributeAsync(int attributeId)
        {
            var attribute = await _context.ProductAttributes.FindAsync(attributeId);
            if (attribute == null) return false;

            _context.ProductAttributes.Remove(attribute);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IProductAttributeValueRepository
    {
        Task<List<ProductAttributeValueDto>> GetAttributeValuesByProductAsync(int productId);
        Task<ProductAttributeValueDto> GetAttributeValueByIdAsync(int valueId);
        Task<ProductAttributeValueDto> CreateAttributeValueAsync(ProductAttributeValueCreateDto attributeValueCreateDto);
        Task<ProductAttributeValueDto> UpdateAttributeValueAsync(int valueId, ProductAttributeValueDto attributeValueDto);
        Task<bool> DeleteAttributeValueAsync(int valueId);
    }

    public class ProductAttributeValueRepository : IProductAttributeValueRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductAttributeValueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductAttributeValueDto>> GetAttributeValuesByProductAsync(int productId)
        {
            return await _context.ProductAttributeValues
                .Where(av => av.ProductId == productId)
                .Include(av => av.Attribute)
                .Select(av => new ProductAttributeValueDto
                {
                    ValueId = av.ValueId,
                    ProductId = av.ProductId,
                    AttributeId = av.AttributeId,
                    Value = av.Value,
                    Attribute = new ProductAttributeDto
                    {
                        AttributeId = av.Attribute.AttributeId,
                        AttributeName = av.Attribute.AttributeName,
                        AttributeType = av.Attribute.AttributeType
                    }
                }).ToListAsync();
        }

        public async Task<ProductAttributeValueDto> GetAttributeValueByIdAsync(int valueId)
        {
            var attributeValue = await _context.ProductAttributeValues
                .Include(av => av.Attribute)
                .FirstOrDefaultAsync(av => av.ValueId == valueId);

            if (attributeValue == null) return null;

            return new ProductAttributeValueDto
            {
                ValueId = attributeValue.ValueId,
                ProductId = attributeValue.ProductId,
                AttributeId = attributeValue.AttributeId,
                Value = attributeValue.Value,
                Attribute = new ProductAttributeDto
                {
                    AttributeId = attributeValue.Attribute.AttributeId,
                    AttributeName = attributeValue.Attribute.AttributeName,
                    AttributeType = attributeValue.Attribute.AttributeType
                }
            };
        }

        public async Task<ProductAttributeValueDto> CreateAttributeValueAsync(ProductAttributeValueCreateDto attributeValueCreateDto)
        {
            var attributeValue = new ProductAttributeValue
            {
                ProductId = attributeValueCreateDto.ProductId,
                AttributeId = attributeValueCreateDto.AttributeId,
                Value = attributeValueCreateDto.Value
            };

            _context.ProductAttributeValues.Add(attributeValue);
            await _context.SaveChangesAsync();

            return await GetAttributeValueByIdAsync(attributeValue.ValueId);
        }

        public async Task<ProductAttributeValueDto> UpdateAttributeValueAsync(int valueId, ProductAttributeValueDto attributeValueDto)
        {
            var attributeValue = await _context.ProductAttributeValues.FindAsync(valueId);
            if (attributeValue == null) return null;

            attributeValue.Value = attributeValueDto.Value;

            await _context.SaveChangesAsync();
            return await GetAttributeValueByIdAsync(valueId);
        }

        public async Task<bool> DeleteAttributeValueAsync(int valueId)
        {
            var attributeValue = await _context.ProductAttributeValues.FindAsync(valueId);
            if (attributeValue == null) return false;

            _context.ProductAttributeValues.Remove(attributeValue);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IProductVariantRepository
    {
        Task<List<ProductVariantDto>> GetVariantsByProductAsync(int productId);
        Task<ProductVariantDto> GetVariantByIdAsync(int variantId);
        Task<ProductVariantDto> CreateVariantAsync(ProductVariantCreateDto variantCreateDto);
        Task<ProductVariantDto> UpdateVariantAsync(int variantId, ProductVariantDto variantDto);
        Task<bool> DeleteVariantAsync(int variantId);
    }

    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductVariantDto>> GetVariantsByProductAsync(int productId)
        {
            return await _context.ProductVariants
                .Where(v => v.ProductId == productId)
                .Select(v => new ProductVariantDto
                {
                    VariantId = v.VariantId,
                    ProductId = v.ProductId,
                    VariantName = v.VariantName,
                    SKU = v.SKU,
                    AdditionalCost = v.AdditionalCost,
                    IsActive = v.IsActive
                }).ToListAsync();
        }

        public async Task<ProductVariantDto> GetVariantByIdAsync(int variantId)
        {
            var variant = await _context.ProductVariants.FindAsync(variantId);
            if (variant == null) return null;

            return new ProductVariantDto
            {
                VariantId = variant.VariantId,
                ProductId = variant.ProductId,
                VariantName = variant.VariantName,
                SKU = variant.SKU,
                AdditionalCost = variant.AdditionalCost,
                IsActive = variant.IsActive
            };
        }

        public async Task<ProductVariantDto> CreateVariantAsync(ProductVariantCreateDto variantCreateDto)
        {
            var variant = new ProductVariant
            {
                ProductId = variantCreateDto.ProductId,
                VariantName = variantCreateDto.VariantName,
                SKU = variantCreateDto.SKU,
                AdditionalCost = variantCreateDto.AdditionalCost,
                IsActive = true
            };

            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();

            return await GetVariantByIdAsync(variant.VariantId);
        }

        public async Task<ProductVariantDto> UpdateVariantAsync(int variantId, ProductVariantDto variantDto)
        {
            var variant = await _context.ProductVariants.FindAsync(variantId);
            if (variant == null) return null;

            variant.VariantName = variantDto.VariantName;
            variant.SKU = variantDto.SKU;
            variant.AdditionalCost = variantDto.AdditionalCost;
            variant.IsActive = variantDto.IsActive;

            await _context.SaveChangesAsync();
            return await GetVariantByIdAsync(variantId);
        }

        public async Task<bool> DeleteVariantAsync(int variantId)
        {
            var variant = await _context.ProductVariants.FindAsync(variantId);
            if (variant == null) return false;

            variant.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IInventoryRepository
    {
        Task<InventoryDto> GetInventoryByIdAsync(int inventoryId);
        Task<List<InventoryDto>> GetInventoryByProductAsync(int productId);
        Task<List<InventoryDto>> GetInventoryByWarehouseAsync(int warehouseId);
        Task<List<InventoryDto>> GetInventoryByLocationAsync(int locationId);
        Task<InventoryDto> CreateInventoryAsync(InventoryDto inventoryDto);
        Task<InventoryDto> UpdateInventoryAsync(int inventoryId, InventoryUpdateDto inventoryUpdateDto);
        Task<bool> DeleteInventoryAsync(int inventoryId);
    }

    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InventoryDto> GetInventoryByIdAsync(int inventoryId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Variant)
                .Include(i => i.Warehouse)
                .Include(i => i.Location)
                .FirstOrDefaultAsync(i => i.InventoryId == inventoryId);

            if (inventory == null) return null;

            return new InventoryDto
            {
                InventoryId = inventory.InventoryId,
                ProductId = inventory.ProductId,
                VariantId = inventory.VariantId,
                WarehouseId = inventory.WarehouseId,
                LocationId = inventory.LocationId,
                QuantityOnHand = inventory.QuantityOnHand,
                QuantityReserved = inventory.QuantityReserved,
                QuantityAvailable = inventory.QuantityOnHand - inventory.QuantityReserved,
                LastCountDate = inventory.LastCountDate,
                LastReceivedDate = inventory.LastReceivedDate,
                ExpiryDate = inventory.ExpiryDate,
                LotNumber = inventory.LotNumber,
                SerialNumber = inventory.SerialNumber,
                Product = new ProductDto
                {
                    ProductId = inventory.Product.ProductId,
                    SKU = inventory.Product.SKU,
                    Name = inventory.Product.Name
                },
                Variant = inventory.Variant != null ? new ProductVariantDto
                {
                    VariantId = inventory.Variant.VariantId,
                    VariantName = inventory.Variant.VariantName,
                    SKU = inventory.Variant.SKU
                } : null,
                Warehouse = new WarehouseDto
                {
                    WarehouseId = inventory.Warehouse.WarehouseId,
                    WarehouseName = inventory.Warehouse.WarehouseName
                },
                Location = new StorageLocationDto
                {
                    LocationId = inventory.Location.LocationId,
                    Aisle = inventory.Location.Aisle,
                    Rack = inventory.Location.Rack,
                    Bin = inventory.Location.Bin
                }
            };
        }

        public async Task<List<InventoryDto>> GetInventoryByProductAsync(int productId)
        {
            return await _context.Inventories
                .Where(i => i.ProductId == productId)
                .Include(i => i.Warehouse)
                .Include(i => i.Location)
                .Select(i => new InventoryDto
                {
                    InventoryId = i.InventoryId,
                    ProductId = i.ProductId,
                    VariantId = i.VariantId,
                    WarehouseId = i.WarehouseId,
                    LocationId = i.LocationId,
                    QuantityOnHand = i.QuantityOnHand,
                    QuantityReserved = i.QuantityReserved,
                    QuantityAvailable = i.QuantityOnHand - i.QuantityReserved,
                    Warehouse = new WarehouseDto
                    {
                        WarehouseId = i.Warehouse.WarehouseId,
                        WarehouseName = i.Warehouse.WarehouseName
                    },
                    Location = new StorageLocationDto
                    {
                        LocationId = i.Location.LocationId,
                        Aisle = i.Location.Aisle,
                        Rack = i.Location.Rack,
                        Bin = i.Location.Bin
                    }
                }).ToListAsync();
        }

        public async Task<List<InventoryDto>> GetInventoryByWarehouseAsync(int warehouseId)
        {
            return await _context.Inventories
                .Where(i => i.WarehouseId == warehouseId)
                .Include(i => i.Product)
                .Include(i => i.Variant)
                .Include(i => i.Location)
                .Select(i => new InventoryDto
                {
                    InventoryId = i.InventoryId,
                    ProductId = i.ProductId,
                    VariantId = i.VariantId,
                    WarehouseId = i.WarehouseId,
                    LocationId = i.LocationId,
                    QuantityOnHand = i.QuantityOnHand,
                    QuantityReserved = i.QuantityReserved,
                    QuantityAvailable = i.QuantityOnHand - i.QuantityReserved,
                    Product = new ProductDto
                    {
                        ProductId = i.Product.ProductId,
                        SKU = i.Product.SKU,
                        Name = i.Product.Name
                    },
                    Variant = i.Variant != null ? new ProductVariantDto
                    {
                        VariantId = i.Variant.VariantId,
                        VariantName = i.Variant.VariantName,
                        SKU = i.Variant.SKU
                    } : null,
                    Location = new StorageLocationDto
                    {
                        LocationId = i.Location.LocationId,
                        Aisle = i.Location.Aisle,
                        Rack = i.Location.Rack,
                        Bin = i.Location.Bin
                    }
                }).ToListAsync();
        }

        public async Task<List<InventoryDto>> GetInventoryByLocationAsync(int locationId)
        {
            return await _context.Inventories
                .Where(i => i.LocationId == locationId)
                .Include(i => i.Product)
                .Include(i => i.Variant)
                .Select(i => new InventoryDto
                {
                    InventoryId = i.InventoryId,
                    ProductId = i.ProductId,
                    VariantId = i.VariantId,
                    WarehouseId = i.WarehouseId,
                    LocationId = i.LocationId,
                    QuantityOnHand = i.QuantityOnHand,
                    QuantityReserved = i.QuantityReserved,
                    QuantityAvailable = i.QuantityOnHand - i.QuantityReserved,
                    Product = new ProductDto
                    {
                        ProductId = i.Product.ProductId,
                        SKU = i.Product.SKU,
                        Name = i.Product.Name
                    },
                    Variant = i.Variant != null ? new ProductVariantDto
                    {
                        VariantId = i.Variant.VariantId,
                        VariantName = i.Variant.VariantName,
                        SKU = i.Variant.SKU
                    } : null
                }).ToListAsync();
        }

        public async Task<InventoryDto> CreateInventoryAsync(InventoryDto inventoryDto)
        {
            var inventory = new Inventory
            {
                ProductId = inventoryDto.ProductId,
                VariantId = inventoryDto.VariantId,
                WarehouseId = inventoryDto.WarehouseId,
                LocationId = inventoryDto.LocationId,
                QuantityOnHand = inventoryDto.QuantityOnHand,
                QuantityReserved = inventoryDto.QuantityReserved,
                LastReceivedDate = DateTime.UtcNow,
                ExpiryDate = inventoryDto.ExpiryDate,
                LotNumber = inventoryDto.LotNumber,
                SerialNumber = inventoryDto.SerialNumber
            };

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return await GetInventoryByIdAsync(inventory.InventoryId);
        }

        public async Task<InventoryDto> UpdateInventoryAsync(int inventoryId, InventoryUpdateDto inventoryUpdateDto)
        {
            var inventory = await _context.Inventories.FindAsync(inventoryId);
            if (inventory == null) return null;

            inventory.QuantityOnHand = inventoryUpdateDto.QuantityOnHand;
            inventory.QuantityReserved = inventoryUpdateDto.QuantityReserved;
            inventory.ExpiryDate = inventoryUpdateDto.ExpiryDate;
            inventory.LotNumber = inventoryUpdateDto.LotNumber;
            inventory.SerialNumber = inventoryUpdateDto.SerialNumber;

            await _context.SaveChangesAsync();
            return await GetInventoryByIdAsync(inventoryId);
        }

        public async Task<bool> DeleteInventoryAsync(int inventoryId)
        {
            var inventory = await _context.Inventories.FindAsync(inventoryId);
            if (inventory == null) return false;

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IInventoryTransactionRepository
    {
        Task<List<InventoryTransactionDto>> GetTransactionsByInventoryAsync(int inventoryId);
        Task<List<InventoryTransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<InventoryTransactionDto> CreateTransactionAsync(InventoryTransactionDto transactionDto);
    }

    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryTransactionDto>> GetTransactionsByInventoryAsync(int inventoryId)
        {
            return await _context.InventoryTransactions
                .Where(t => t.InventoryId == inventoryId)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new InventoryTransactionDto
                {
                    TransactionId = t.TransactionId,
                    InventoryId = t.InventoryId,
                    TransactionType = t.TransactionType,
                    Quantity = t.Quantity,
                    PreviousQuantity = t.PreviousQuantity,
                    NewQuantity = t.NewQuantity,
                    SourceLocationId = t.SourceLocationId,
                    DestinationLocationId = t.DestinationLocationId,
                    ReferenceNumber = t.ReferenceNumber,
                    TransactionDate = t.TransactionDate,
                    UserId = t.UserId,
                    Notes = t.Notes
                }).ToListAsync();
        }

        public async Task<List<InventoryTransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.InventoryTransactions
                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new InventoryTransactionDto
                {
                    TransactionId = t.TransactionId,
                    InventoryId = t.InventoryId,
                    TransactionType = t.TransactionType,
                    Quantity = t.Quantity,
                    PreviousQuantity = t.PreviousQuantity,
                    NewQuantity = t.NewQuantity,
                    SourceLocationId = t.SourceLocationId,
                    DestinationLocationId = t.DestinationLocationId,
                    ReferenceNumber = t.ReferenceNumber,
                    TransactionDate = t.TransactionDate,
                    UserId = t.UserId,
                    Notes = t.Notes
                }).ToListAsync();
        }

        public async Task<InventoryTransactionDto> CreateTransactionAsync(InventoryTransactionDto transactionDto)
        {
            var transaction = new InventoryTransaction
            {
                InventoryId = transactionDto.InventoryId,
                TransactionType = transactionDto.TransactionType,
                Quantity = transactionDto.Quantity,
                PreviousQuantity = transactionDto.PreviousQuantity,
                NewQuantity = transactionDto.NewQuantity,
                SourceLocationId = transactionDto.SourceLocationId,
                DestinationLocationId = transactionDto.DestinationLocationId,
                ReferenceNumber = transactionDto.ReferenceNumber,
                TransactionDate = DateTime.UtcNow,
                UserId = transactionDto.UserId,
                Notes = transactionDto.Notes
            };

            _context.InventoryTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return new InventoryTransactionDto
            {
                TransactionId = transaction.TransactionId,
                InventoryId = transaction.InventoryId,
                TransactionType = transaction.TransactionType,
                Quantity = transaction.Quantity,
                PreviousQuantity = transaction.PreviousQuantity,
                NewQuantity = transaction.NewQuantity,
                SourceLocationId = transaction.SourceLocationId,
                DestinationLocationId = transaction.DestinationLocationId,
                ReferenceNumber = transaction.ReferenceNumber,
                TransactionDate = transaction.TransactionDate,
                UserId = transaction.UserId,
                Notes = transaction.Notes
            };
        }
    }

    #endregion
}
