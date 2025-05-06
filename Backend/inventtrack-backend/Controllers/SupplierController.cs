using Amazon.Runtime;
using inventtrack_backend.DTOs;
using inventtrack_backend.Service;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventtrack_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SuppliersController> _logger;

        public SuppliersController(
            ISupplierService supplierService,
            ILogger<SuppliersController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedResponse<SupplierDto>>> GetSuppliers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0");
                }

                var suppliers = await _supplierService.GetSuppliersAsync(pageNumber, pageSize);
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting suppliers");
                return StatusCode(500, "Error retrieving suppliers");
            }
        }

        /// <summary>
        /// Get a specific supplier by ID with details
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierWithDetailsDto>> GetSupplier(int id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierWithDetailsAsync(id);
                if (supplier == null)
                {
                    return NotFound();
                }
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting supplier with ID {id}");
                return StatusCode(500, "Error retrieving supplier");
            }
        }

        /// <summary>
        /// Create a new supplier
        /// </summary>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SupplierDto>> CreateSupplier(
            [FromBody] SupplierCreateDto supplierCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdSupplier = await _supplierService.CreateSupplierAsync(supplierCreateDto);
                return CreatedAtAction(
                    nameof(GetSupplier),
                    new { id = createdSupplier.SupplierId },
                    createdSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating supplier");
                return StatusCode(500, "Error creating supplier");
            }
        }

        /// <summary>
        /// Update an existing supplier
        /// </summary>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSupplier(
            int id,
            [FromBody] SupplierUpdateDto supplierUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _supplierService.UpdateSupplierAsync(id, supplierUpdateDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating supplier with ID {id}");
                return StatusCode(500, "Error updating supplier");
            }
        }

        /// <summary>
        /// Delete a supplier
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                await _supplierService.DeleteSupplierAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting supplier with ID {id}");
                return StatusCode(500, "Error deleting supplier");
            }
        }

        /// <summary>
        /// Get statistics for a supplier
        /// </summary>
        [HttpGet("{id}/stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierStatsDto>> GetSupplierStats(int id)
        {
            try
            {
                var stats = await _supplierService.GetSupplierStatsAsync(id);
                return Ok(stats);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting stats for supplier with ID {id}");
                return StatusCode(500, "Error getting supplier stats");
            }
        }

        /// <summary>
        /// Get all products for a supplier
        /// </summary>
        [HttpGet("{id}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SupplierProductDto>>> GetSupplierProducts(int id)
        {
            try
            {
                var products = await _supplierService.GetSupplierProductsAsync(id);
                return Ok(products);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting products for supplier with ID {id}");
                return StatusCode(500, "Error getting supplier products");
            }
        }

        /// <summary>
        /// Add a product to a supplier
        /// </summary>
        [HttpPost("{id}/products")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierProductDto>> AddSupplierProduct(
            int id,
            [FromBody] SupplierProductCreateDto supplierProductCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdProduct = await _supplierService.AddSupplierProductAsync(id, supplierProductCreateDto);
                return CreatedAtAction(
                    nameof(GetSupplierProducts),
                    new { id },
                    createdProduct);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding product to supplier with ID {id}");
                return StatusCode(500, "Error adding supplier product");
            }
        }

        /// <summary>
        /// Remove a product from a supplier
        /// </summary>
        [HttpDelete("{supplierId}/products/{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveSupplierProduct(int supplierId, int productId)
        {
            try
            {
                await _supplierService.RemoveSupplierProductAsync(supplierId, productId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing product from supplier with ID {supplierId}");
                return StatusCode(500, "Error removing supplier product");
            }
        }

        /// <summary>
        /// Get all purchase orders for a supplier
        /// </summary>
        [HttpGet("{id}/purchase-orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetSupplierPurchaseOrders(int id)
        {
            try
            {
                var purchaseOrders = await _supplierService.GetSupplierPurchaseOrdersAsync(id);
                return Ok(purchaseOrders);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting purchase orders for supplier with ID {id}");
                return StatusCode(500, "Error getting supplier purchase orders");
            }
        }

        /// <summary>
        /// Get performance metrics for a supplier
        /// </summary>
        [HttpGet("{id}/performance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SupplierPerformanceDto>>> GetSupplierPerformanceMetrics(int id)
        {
            try
            {
                var metrics = await _supplierService.GetSupplierPerformanceMetricsAsync(id);
                return Ok(metrics);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting performance metrics for supplier with ID {id}");
                return StatusCode(500, "Error getting supplier performance metrics");
            }
        }
    }
}
