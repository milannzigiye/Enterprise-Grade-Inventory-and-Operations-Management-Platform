using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    #region Supplier Management Controllers

    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet("{supplierId}")]
        public async Task<ActionResult<SupplierDto>> GetSupplierById(int supplierId)
        {
            var supplier = await _supplierRepository.GetSupplierByIdAsync(supplierId);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplierDto>>> GetAllSuppliers()
        {
            var suppliers = await _supplierRepository.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDto>> CreateSupplier(SupplierCreateDto supplierCreateDto)
        {
            var supplier = await _supplierRepository.CreateSupplierAsync(supplierCreateDto);
            return CreatedAtAction(nameof(GetSupplierById), new { supplierId = supplier.SupplierId }, supplier);
        }

        [HttpPut("{supplierId}")]
        public async Task<ActionResult<SupplierDto>> UpdateSupplier(int supplierId, SupplierDto supplierDto)
        {
            var supplier = await _supplierRepository.UpdateSupplierAsync(supplierId, supplierDto);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpDelete("{supplierId}")]
        public async Task<ActionResult> DeleteSupplier(int supplierId)
        {
            var result = await _supplierRepository.DeleteSupplierAsync(supplierId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SupplierProductController : ControllerBase
    {
        private readonly ISupplierProductRepository _supplierProductRepository;

        public SupplierProductController(ISupplierProductRepository supplierProductRepository)
        {
            _supplierProductRepository = supplierProductRepository;
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<List<SupplierProductDto>>> GetProductsBySupplier(int supplierId)
        {
            var products = await _supplierProductRepository.GetProductsBySupplierAsync(supplierId);
            return Ok(products);
        }

        [HttpGet("{supplierProductId}")]
        public async Task<ActionResult<SupplierProductDto>> GetSupplierProductById(int supplierProductId)
        {
            var product = await _supplierProductRepository.GetSupplierProductByIdAsync(supplierProductId);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierProductDto>> CreateSupplierProduct(SupplierProductCreateDto supplierProductCreateDto)
        {
            var product = await _supplierProductRepository.CreateSupplierProductAsync(supplierProductCreateDto);
            return CreatedAtAction(nameof(GetSupplierProductById), new { supplierProductId = product.SupplierProductId }, product);
        }

        [HttpPut("{supplierProductId}")]
        public async Task<ActionResult<SupplierProductDto>> UpdateSupplierProduct(int supplierProductId, SupplierProductDto supplierProductDto)
        {
            var product = await _supplierProductRepository.UpdateSupplierProductAsync(supplierProductId, supplierProductDto);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpDelete("{supplierProductId}")]
        public async Task<ActionResult> DeleteSupplierProduct(int supplierProductId)
        {
            var result = await _supplierProductRepository.DeleteSupplierProductAsync(supplierProductId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public PurchaseOrderController(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        [HttpGet("{poId}")]
        public async Task<ActionResult<PurchaseOrderDto>> GetPurchaseOrderById(int poId)
        {
            var order = await _purchaseOrderRepository.GetPurchaseOrderByIdAsync(poId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<List<PurchaseOrderDto>>> GetPurchaseOrdersBySupplier(int supplierId)
        {
            var orders = await _purchaseOrderRepository.GetPurchaseOrdersBySupplierAsync(supplierId);
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<PurchaseOrderDto>>> GetPurchaseOrdersByStatus(string status)
        {
            var orders = await _purchaseOrderRepository.GetPurchaseOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDto>> CreatePurchaseOrder(PurchaseOrderCreateDto purchaseOrderCreateDto)
        {
            var order = await _purchaseOrderRepository.CreatePurchaseOrderAsync(purchaseOrderCreateDto);
            return CreatedAtAction(nameof(GetPurchaseOrderById), new { poId = order.POId }, order);
        }

        [HttpPut("{poId}")]
        public async Task<ActionResult<PurchaseOrderDto>> UpdatePurchaseOrder(int poId, PurchaseOrderDto purchaseOrderDto)
        {
            var order = await _purchaseOrderRepository.UpdatePurchaseOrderAsync(poId, purchaseOrderDto);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpDelete("{poId}")]
        public async Task<ActionResult> DeletePurchaseOrder(int poId)
        {
            var result = await _purchaseOrderRepository.DeletePurchaseOrderAsync(poId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentController(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        [HttpGet("{shipmentId}")]
        public async Task<ActionResult<ShipmentDto>> GetShipmentById(int shipmentId)
        {
            var shipment = await _shipmentRepository.GetShipmentByIdAsync(shipmentId);
            if (shipment == null)
                return NotFound();

            return Ok(shipment);
        }

        [HttpGet("purchaseorder/{poId}")]
        public async Task<ActionResult<List<ShipmentDto>>> GetShipmentsByPurchaseOrder(int poId)
        {
            var shipments = await _shipmentRepository.GetShipmentsByPurchaseOrderAsync(poId);
            return Ok(shipments);
        }

        [HttpPost]
        public async Task<ActionResult<ShipmentDto>> CreateShipment(ShipmentDto shipmentDto)
        {
            var shipment = await _shipmentRepository.CreateShipmentAsync(shipmentDto);
            return CreatedAtAction(nameof(GetShipmentById), new { shipmentId = shipment.ShipmentId }, shipment);
        }

        [HttpPut("{shipmentId}")]
        public async Task<ActionResult<ShipmentDto>> UpdateShipment(int shipmentId, ShipmentDto shipmentDto)
        {
            var shipment = await _shipmentRepository.UpdateShipmentAsync(shipmentId, shipmentDto);
            if (shipment == null)
                return NotFound();

            return Ok(shipment);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SupplierPerformanceController : ControllerBase
    {
        private readonly ISupplierPerformanceRepository _performanceRepository;

        public SupplierPerformanceController(ISupplierPerformanceRepository performanceRepository)
        {
            _performanceRepository = performanceRepository;
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<List<SupplierPerformanceDto>>> GetPerformanceRecordsBySupplier(int supplierId)
        {
            var records = await _performanceRepository.GetPerformanceRecordsBySupplierAsync(supplierId);
            return Ok(records);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierPerformanceDto>> CreatePerformanceRecord(SupplierPerformanceDto performanceDto)
        {
            var record = await _performanceRepository.CreatePerformanceRecordAsync(performanceDto);
            return Ok(record);
        }
    }

    #endregion
}
