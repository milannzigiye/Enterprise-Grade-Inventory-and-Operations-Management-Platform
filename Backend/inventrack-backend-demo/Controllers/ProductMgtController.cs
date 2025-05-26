using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    #region Product Management Controllers

    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryRepository _categoryRepository;

        public ProductCategoryController(IProductCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<ProductCategoryDto>> GetCategoryById(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductCategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("children/{parentCategoryId}")]
        public async Task<ActionResult<List<ProductCategoryDto>>> GetChildCategories(int parentCategoryId)
        {
            var categories = await _categoryRepository.GetChildCategoriesAsync(parentCategoryId);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<ProductCategoryDto>> CreateCategory(ProductCategoryCreateDto categoryCreateDto)
        {
            var category = await _categoryRepository.CreateCategoryAsync(categoryCreateDto);
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = category.CategoryId }, category);
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult<ProductCategoryDto>> UpdateCategory(int categoryId, ProductCategoryDto categoryDto)
        {
            var category = await _categoryRepository.UpdateCategoryAsync(categoryId, categoryDto);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var result = await _categoryRepository.DeleteCategoryAsync(categoryId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductCreateDto productCreateDto)
        {
            var product = await _productRepository.CreateProductAsync(productCreateDto);
            return CreatedAtAction(nameof(GetProductById), new { productId = product.ProductId }, product);
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int productId, ProductUpdateDto productUpdateDto)
        {
            var product = await _productRepository.UpdateProductAsync(productId, productUpdateDto);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var result = await _productRepository.DeleteProductAsync(productId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ProductAttributeController : ControllerBase
    {
        private readonly IProductAttributeRepository _attributeRepository;

        public ProductAttributeController(IProductAttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductAttributeDto>>> GetAllAttributes()
        {
            var attributes = await _attributeRepository.GetAllAttributesAsync();
            return Ok(attributes);
        }

        [HttpGet("{attributeId}")]
        public async Task<ActionResult<ProductAttributeDto>> GetAttributeById(int attributeId)
        {
            var attribute = await _attributeRepository.GetAttributeByIdAsync(attributeId);
            if (attribute == null)
                return NotFound();

            return Ok(attribute);
        }

        [HttpPost]
        public async Task<ActionResult<ProductAttributeDto>> CreateAttribute(ProductAttributeDto attributeDto)
        {
            var attribute = await _attributeRepository.CreateAttributeAsync(attributeDto);
            return CreatedAtAction(nameof(GetAttributeById), new { attributeId = attribute.AttributeId }, attribute);
        }

        [HttpPut("{attributeId}")]
        public async Task<ActionResult<ProductAttributeDto>> UpdateAttribute(int attributeId, ProductAttributeDto attributeDto)
        {
            var attribute = await _attributeRepository.UpdateAttributeAsync(attributeId, attributeDto);
            if (attribute == null)
                return NotFound();

            return Ok(attribute);
        }

        [HttpDelete("{attributeId}")]
        public async Task<ActionResult> DeleteAttribute(int attributeId)
        {
            var result = await _attributeRepository.DeleteAttributeAsync(attributeId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ProductAttributeValueController : ControllerBase
    {
        private readonly IProductAttributeValueRepository _attributeValueRepository;

        public ProductAttributeValueController(IProductAttributeValueRepository attributeValueRepository)
        {
            _attributeValueRepository = attributeValueRepository;
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<List<ProductAttributeValueDto>>> GetAttributeValuesByProduct(int productId)
        {
            var attributeValues = await _attributeValueRepository.GetAttributeValuesByProductAsync(productId);
            return Ok(attributeValues);
        }

        [HttpGet("{valueId}")]
        public async Task<ActionResult<ProductAttributeValueDto>> GetAttributeValueById(int valueId)
        {
            var attributeValue = await _attributeValueRepository.GetAttributeValueByIdAsync(valueId);
            if (attributeValue == null)
                return NotFound();

            return Ok(attributeValue);
        }

        [HttpPost]
        public async Task<ActionResult<ProductAttributeValueDto>> CreateAttributeValue(ProductAttributeValueCreateDto attributeValueCreateDto)
        {
            var attributeValue = await _attributeValueRepository.CreateAttributeValueAsync(attributeValueCreateDto);
            return CreatedAtAction(nameof(GetAttributeValueById), new { valueId = attributeValue.ValueId }, attributeValue);
        }

        [HttpPut("{valueId}")]
        public async Task<ActionResult<ProductAttributeValueDto>> UpdateAttributeValue(int valueId, ProductAttributeValueDto attributeValueDto)
        {
            var attributeValue = await _attributeValueRepository.UpdateAttributeValueAsync(valueId, attributeValueDto);
            if (attributeValue == null)
                return NotFound();

            return Ok(attributeValue);
        }

        [HttpDelete("{valueId}")]
        public async Task<ActionResult> DeleteAttributeValue(int valueId)
        {
            var result = await _attributeValueRepository.DeleteAttributeValueAsync(valueId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantRepository _variantRepository;

        public ProductVariantController(IProductVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<List<ProductVariantDto>>> GetVariantsByProduct(int productId)
        {
            var variants = await _variantRepository.GetVariantsByProductAsync(productId);
            return Ok(variants);
        }

        [HttpGet("{variantId}")]
        public async Task<ActionResult<ProductVariantDto>> GetVariantById(int variantId)
        {
            var variant = await _variantRepository.GetVariantByIdAsync(variantId);
            if (variant == null)
                return NotFound();

            return Ok(variant);
        }

        [HttpPost]
        public async Task<ActionResult<ProductVariantDto>> CreateVariant(ProductVariantCreateDto variantCreateDto)
        {
            var variant = await _variantRepository.CreateVariantAsync(variantCreateDto);
            return CreatedAtAction(nameof(GetVariantById), new { variantId = variant.VariantId }, variant);
        }

        [HttpPut("{variantId}")]
        public async Task<ActionResult<ProductVariantDto>> UpdateVariant(int variantId, ProductVariantDto variantDto)
        {
            var variant = await _variantRepository.UpdateVariantAsync(variantId, variantDto);
            if (variant == null)
                return NotFound();

            return Ok(variant);
        }

        [HttpDelete("{variantId}")]
        public async Task<ActionResult> DeleteVariant(int variantId)
        {
            var result = await _variantRepository.DeleteVariantAsync(variantId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet("{inventoryId}")]
        public async Task<ActionResult<InventoryDto>> GetInventoryById(int inventoryId)
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
            if (inventory == null)
                return NotFound();

            return Ok(inventory);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<List<InventoryDto>>> GetInventoryByProduct(int productId)
        {
            var inventories = await _inventoryRepository.GetInventoryByProductAsync(productId);
            return Ok(inventories);
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<List<InventoryDto>>> GetInventoryByWarehouse(int warehouseId)
        {
            var inventories = await _inventoryRepository.GetInventoryByWarehouseAsync(warehouseId);
            return Ok(inventories);
        }

        [HttpGet("location/{locationId}")]
        public async Task<ActionResult<List<InventoryDto>>> GetInventoryByLocation(int locationId)
        {
            var inventories = await _inventoryRepository.GetInventoryByLocationAsync(locationId);
            return Ok(inventories);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryDto>> CreateInventory(InventoryDto inventoryDto)
        {
            var inventory = await _inventoryRepository.CreateInventoryAsync(inventoryDto);
            return CreatedAtAction(nameof(GetInventoryById), new { inventoryId = inventory.InventoryId }, inventory);
        }

        [HttpPut("{inventoryId}")]
        public async Task<ActionResult<InventoryDto>> UpdateInventory(int inventoryId, InventoryUpdateDto inventoryUpdateDto)
        {
            var inventory = await _inventoryRepository.UpdateInventoryAsync(inventoryId, inventoryUpdateDto);
            if (inventory == null)
                return NotFound();

            return Ok(inventory);
        }

        [HttpDelete("{inventoryId}")]
        public async Task<ActionResult> DeleteInventory(int inventoryId)
        {
            var result = await _inventoryRepository.DeleteInventoryAsync(inventoryId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class InventoryTransactionController : ControllerBase
    {
        private readonly IInventoryTransactionRepository _transactionRepository;

        public InventoryTransactionController(IInventoryTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet("inventory/{inventoryId}")]
        public async Task<ActionResult<List<InventoryTransactionDto>>> GetTransactionsByInventory(int inventoryId)
        {
            var transactions = await _transactionRepository.GetTransactionsByInventoryAsync(inventoryId);
            return Ok(transactions);
        }

        [HttpGet("daterange")]
        public async Task<ActionResult<List<InventoryTransactionDto>>> GetTransactionsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var transactions = await _transactionRepository.GetTransactionsByDateRangeAsync(startDate, endDate);
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryTransactionDto>> CreateTransaction(InventoryTransactionDto transactionDto)
        {
            var transaction = await _transactionRepository.CreateTransactionAsync(transactionDto);
            return Ok(transaction);
        }
    }

    #endregion
}
