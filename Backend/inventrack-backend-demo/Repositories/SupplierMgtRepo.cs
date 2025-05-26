using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using inventrack_backend_demo.Data;
using Microsoft.EntityFrameworkCore;

namespace inventrack_backend_demo.Repositories
{
    #region Supplier Management Repositories

    public interface ISupplierRepository
    {
        Task<SupplierDto> GetSupplierByIdAsync(int supplierId);
        Task<List<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto);
        Task<SupplierDto> UpdateSupplierAsync(int supplierId, SupplierDto supplierDto);
        Task<bool> DeleteSupplierAsync(int supplierId);
    }

    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(int supplierId)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.SupplierProducts)
                    .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.SupplierId == supplierId);

            if (supplier == null) return null;

            return new SupplierDto
            {
                SupplierId = supplier.SupplierId,
                UserId = supplier.UserId,
                CompanyName = supplier.CompanyName,
                ContactName = supplier.ContactName,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address,
                City = supplier.City,
                State = supplier.State,
                ZipCode = supplier.ZipCode,
                Country = supplier.Country,
                TaxIdentificationNumber = supplier.TaxIdentificationNumber,
                PaymentTerms = supplier.PaymentTerms,
                LeadTimeInDays = supplier.LeadTimeInDays,
                IsActive = supplier.IsActive,
                OnboardingDate = supplier.OnboardingDate,
                LastOrderDate = supplier.LastOrderDate,
                PerformanceRating = supplier.PerformanceRating,
                SupplierProducts = supplier.SupplierProducts?.Select(sp => new SupplierProductDto
                {
                    SupplierProductId = sp.SupplierProductId,
                    SupplierId = sp.SupplierId,
                    ProductId = sp.ProductId,
                    SupplierSKU = sp.SupplierSKU,
                    UnitCost = sp.UnitCost,
                    MinimumOrderQuantity = sp.MinimumOrderQuantity,
                    IsPreferredSupplier = sp.IsPreferredSupplier,
                    LeadTimeInDays = sp.LeadTimeInDays,
                    LastPurchaseDate = sp.LastPurchaseDate,
                    Product = new ProductDto
                    {
                        ProductId = sp.Product.ProductId,
                        SKU = sp.Product.SKU,
                        Name = sp.Product.Name
                    }
                }).ToList()
            };
        }

        public async Task<List<SupplierDto>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers
                .Select(s => new SupplierDto
                {
                    SupplierId = s.SupplierId,
                    CompanyName = s.CompanyName,
                    ContactName = s.ContactName,
                    Email = s.Email,
                    Phone = s.Phone,
                    IsActive = s.IsActive,
                    OnboardingDate = s.OnboardingDate
                }).ToListAsync();
        }

        public async Task<SupplierDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto)
        {
            var supplier = new Supplier
            {
                CompanyName = supplierCreateDto.CompanyName,
                ContactName = supplierCreateDto.ContactName,
                Email = supplierCreateDto.Email,
                Phone = supplierCreateDto.Phone,
                Address = supplierCreateDto.Address,
                City = supplierCreateDto.City,
                State = supplierCreateDto.State,
                ZipCode = supplierCreateDto.ZipCode,
                Country = supplierCreateDto.Country,
                TaxIdentificationNumber = supplierCreateDto.TaxIdentificationNumber,
                PaymentTerms = supplierCreateDto.PaymentTerms,
                LeadTimeInDays = supplierCreateDto.LeadTimeInDays,
                IsActive = true,
                OnboardingDate = DateTime.UtcNow
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return await GetSupplierByIdAsync(supplier.SupplierId);
        }

        public async Task<SupplierDto> UpdateSupplierAsync(int supplierId, SupplierDto supplierDto)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null) return null;

            supplier.CompanyName = supplierDto.CompanyName;
            supplier.ContactName = supplierDto.ContactName;
            supplier.Email = supplierDto.Email;
            supplier.Phone = supplierDto.Phone;
            supplier.Address = supplierDto.Address;
            supplier.City = supplierDto.City;
            supplier.State = supplierDto.State;
            supplier.ZipCode = supplierDto.ZipCode;
            supplier.Country = supplierDto.Country;
            supplier.TaxIdentificationNumber = supplierDto.TaxIdentificationNumber;
            supplier.PaymentTerms = supplierDto.PaymentTerms;
            supplier.LeadTimeInDays = supplierDto.LeadTimeInDays;
            supplier.IsActive = supplierDto.IsActive;
            supplier.PerformanceRating = supplierDto.PerformanceRating;

            await _context.SaveChangesAsync();
            return await GetSupplierByIdAsync(supplierId);
        }

        public async Task<bool> DeleteSupplierAsync(int supplierId)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null) return false;

            supplier.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface ISupplierProductRepository
    {
        Task<List<SupplierProductDto>> GetProductsBySupplierAsync(int supplierId);
        Task<SupplierProductDto> GetSupplierProductByIdAsync(int supplierProductId);
        Task<SupplierProductDto> CreateSupplierProductAsync(SupplierProductCreateDto supplierProductCreateDto);
        Task<SupplierProductDto> UpdateSupplierProductAsync(int supplierProductId, SupplierProductDto supplierProductDto);
        Task<bool> DeleteSupplierProductAsync(int supplierProductId);
    }

    public class SupplierProductRepository : ISupplierProductRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplierProductDto>> GetProductsBySupplierAsync(int supplierId)
        {
            return await _context.SupplierProducts
                .Where(sp => sp.SupplierId == supplierId)
                .Include(sp => sp.Product)
                .Select(sp => new SupplierProductDto
                {
                    SupplierProductId = sp.SupplierProductId,
                    SupplierId = sp.SupplierId,
                    ProductId = sp.ProductId,
                    SupplierSKU = sp.SupplierSKU,
                    UnitCost = sp.UnitCost,
                    MinimumOrderQuantity = sp.MinimumOrderQuantity,
                    IsPreferredSupplier = sp.IsPreferredSupplier,
                    LeadTimeInDays = sp.LeadTimeInDays,
                    LastPurchaseDate = sp.LastPurchaseDate,
                    Product = new ProductDto
                    {
                        ProductId = sp.Product.ProductId,
                        SKU = sp.Product.SKU,
                        Name = sp.Product.Name
                    }
                }).ToListAsync();
        }

        public async Task<SupplierProductDto> GetSupplierProductByIdAsync(int supplierProductId)
        {
            var supplierProduct = await _context.SupplierProducts
                .Include(sp => sp.Product)
                .FirstOrDefaultAsync(sp => sp.SupplierProductId == supplierProductId);

            if (supplierProduct == null) return null;

            return new SupplierProductDto
            {
                SupplierProductId = supplierProduct.SupplierProductId,
                SupplierId = supplierProduct.SupplierId,
                ProductId = supplierProduct.ProductId,
                SupplierSKU = supplierProduct.SupplierSKU,
                UnitCost = supplierProduct.UnitCost,
                MinimumOrderQuantity = supplierProduct.MinimumOrderQuantity,
                IsPreferredSupplier = supplierProduct.IsPreferredSupplier,
                LeadTimeInDays = supplierProduct.LeadTimeInDays,
                LastPurchaseDate = supplierProduct.LastPurchaseDate,
                Product = new ProductDto
                {
                    ProductId = supplierProduct.Product.ProductId,
                    SKU = supplierProduct.Product.SKU,
                    Name = supplierProduct.Product.Name
                }
            };
        }

        public async Task<SupplierProductDto> CreateSupplierProductAsync(SupplierProductCreateDto supplierProductCreateDto)
        {
            var supplierProduct = new SupplierProduct
            {
                SupplierId = supplierProductCreateDto.SupplierId,
                ProductId = supplierProductCreateDto.ProductId,
                SupplierSKU = supplierProductCreateDto.SupplierSKU,
                UnitCost = supplierProductCreateDto.UnitCost,
                MinimumOrderQuantity = supplierProductCreateDto.MinimumOrderQuantity,
                IsPreferredSupplier = supplierProductCreateDto.IsPreferredSupplier,
                LeadTimeInDays = supplierProductCreateDto.LeadTimeInDays
            };

            _context.SupplierProducts.Add(supplierProduct);
            await _context.SaveChangesAsync();

            return await GetSupplierProductByIdAsync(supplierProduct.SupplierProductId);
        }

        public async Task<SupplierProductDto> UpdateSupplierProductAsync(int supplierProductId, SupplierProductDto supplierProductDto)
        {
            var supplierProduct = await _context.SupplierProducts.FindAsync(supplierProductId);
            if (supplierProduct == null) return null;

            supplierProduct.SupplierSKU = supplierProductDto.SupplierSKU;
            supplierProduct.UnitCost = supplierProductDto.UnitCost;
            supplierProduct.MinimumOrderQuantity = supplierProductDto.MinimumOrderQuantity;
            supplierProduct.IsPreferredSupplier = supplierProductDto.IsPreferredSupplier;
            supplierProduct.LeadTimeInDays = supplierProductDto.LeadTimeInDays;
            supplierProduct.LastPurchaseDate = supplierProductDto.LastPurchaseDate;

            await _context.SaveChangesAsync();
            return await GetSupplierProductByIdAsync(supplierProductId);
        }

        public async Task<bool> DeleteSupplierProductAsync(int supplierProductId)
        {
            var supplierProduct = await _context.SupplierProducts.FindAsync(supplierProductId);
            if (supplierProduct == null) return false;

            _context.SupplierProducts.Remove(supplierProduct);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int poId);
        Task<List<PurchaseOrderDto>> GetPurchaseOrdersBySupplierAsync(int supplierId);
        Task<List<PurchaseOrderDto>> GetPurchaseOrdersByStatusAsync(string status);
        Task<PurchaseOrderDto> CreatePurchaseOrderAsync(PurchaseOrderCreateDto purchaseOrderCreateDto);
        Task<PurchaseOrderDto> UpdatePurchaseOrderAsync(int poId, PurchaseOrderDto purchaseOrderDto);
        Task<bool> DeletePurchaseOrderAsync(int poId);
    }

    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int poId)
        {
            var purchaseOrder = await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Items)
                    .ThenInclude(item => item.Product)
                .Include(po => po.Items)
                    .ThenInclude(item => item.Warehouse)
                .FirstOrDefaultAsync(po => po.POId == poId);

            if (purchaseOrder == null) return null;

            return new PurchaseOrderDto
            {
                POId = purchaseOrder.POId,
                PONumber = purchaseOrder.PONumber,
                SupplierId = purchaseOrder.SupplierId,
                OrderDate = purchaseOrder.OrderDate,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate,
                Status = purchaseOrder.Status,
                ShippingMethod = purchaseOrder.ShippingMethod,
                ShippingCost = purchaseOrder.ShippingCost,
                SubTotal = purchaseOrder.SubTotal,
                TaxAmount = purchaseOrder.TaxAmount,
                TotalAmount = purchaseOrder.TotalAmount,
                Notes = purchaseOrder.Notes,
                CreatedByUserId = purchaseOrder.CreatedByUserId,
                ApprovedByUserId = purchaseOrder.ApprovedByUserId,
                ApprovalDate = purchaseOrder.ApprovalDate,
                PaymentStatus = purchaseOrder.PaymentStatus,
                PaymentDate = purchaseOrder.PaymentDate,
                ActualDeliveryDate = purchaseOrder.ActualDeliveryDate,
                Supplier = new SupplierDto
                {
                    SupplierId = purchaseOrder.Supplier.SupplierId,
                    CompanyName = purchaseOrder.Supplier.CompanyName,
                    ContactName = purchaseOrder.Supplier.ContactName
                },
                Items = purchaseOrder.Items?.Select(item => new PurchaseOrderItemDto
                {
                    POItemId = item.POItemId,
                    POId = item.POId,
                    ProductId = item.ProductId,
                    QuantityOrdered = item.QuantityOrdered,
                    UnitCost = item.UnitCost,
                    TotalCost = item.TotalCost,
                    QuantityReceived = item.QuantityReceived,
                    WarehouseId = item.WarehouseId,
                    ExpectedDeliveryDate = item.ExpectedDeliveryDate,
                    Status = item.Status,
                    Product = new ProductDto
                    {
                        ProductId = item.Product.ProductId,
                        SKU = item.Product.SKU,
                        Name = item.Product.Name
                    },
                    Warehouse = new WarehouseDto
                    {
                        WarehouseId = item.Warehouse.WarehouseId,
                        WarehouseName = item.Warehouse.WarehouseName
                    }
                }).ToList()
            };
        }

        public async Task<List<PurchaseOrderDto>> GetPurchaseOrdersBySupplierAsync(int supplierId)
        {
            return await _context.PurchaseOrders
                .Where(po => po.SupplierId == supplierId)
                .Include(po => po.Supplier)
                .Select(po => new PurchaseOrderDto
                {
                    POId = po.POId,
                    PONumber = po.PONumber,
                    SupplierId = po.SupplierId,
                    OrderDate = po.OrderDate,
                    ExpectedDeliveryDate = po.ExpectedDeliveryDate,
                    Status = po.Status,
                    TotalAmount = po.TotalAmount,
                    PaymentStatus = po.PaymentStatus,
                    Supplier = new SupplierDto
                    {
                        SupplierId = po.Supplier.SupplierId,
                        CompanyName = po.Supplier.CompanyName
                    }
                }).ToListAsync();
        }

        public async Task<List<PurchaseOrderDto>> GetPurchaseOrdersByStatusAsync(string status)
        {
            return await _context.PurchaseOrders
                .Where(po => po.Status == status)
                .Include(po => po.Supplier)
                .Select(po => new PurchaseOrderDto
                {
                    POId = po.POId,
                    PONumber = po.PONumber,
                    SupplierId = po.SupplierId,
                    OrderDate = po.OrderDate,
                    ExpectedDeliveryDate = po.ExpectedDeliveryDate,
                    Status = po.Status,
                    TotalAmount = po.TotalAmount,
                    Supplier = new SupplierDto
                    {
                        SupplierId = po.Supplier.SupplierId,
                        CompanyName = po.Supplier.CompanyName
                    }
                }).ToListAsync();
        }

        public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(PurchaseOrderCreateDto purchaseOrderCreateDto)
        {
            var purchaseOrder = new PurchaseOrder
            {
                PONumber = GeneratePONumber(),
                SupplierId = purchaseOrderCreateDto.SupplierId,
                OrderDate = DateTime.UtcNow,
                ExpectedDeliveryDate = purchaseOrderCreateDto.ExpectedDeliveryDate,
                Status = "Pending",
                ShippingMethod = purchaseOrderCreateDto.ShippingMethod,
                CreatedByUserId = 1, // TODO: Replace with actual user ID from auth context
                PaymentStatus = "Pending"
            };

            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();

            // Add items
            foreach (var itemDto in purchaseOrderCreateDto.Items)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);
                if (product == null) continue;

                var poItem = new PurchaseOrderItem
                {
                    POId = purchaseOrder.POId,
                    ProductId = itemDto.ProductId,
                    QuantityOrdered = itemDto.QuantityOrdered,
                    UnitCost = itemDto.UnitCost,
                    TotalCost = itemDto.QuantityOrdered * itemDto.UnitCost,
                    WarehouseId = itemDto.WarehouseId,
                    Status = "Ordered"
                };

                _context.PurchaseOrderItems.Add(poItem);
            }

            // Calculate totals
            purchaseOrder.SubTotal = purchaseOrder.Items.Sum(item => item.TotalCost);
            purchaseOrder.TaxAmount = purchaseOrder.SubTotal * 0.1m; // Example 10% tax
            purchaseOrder.TotalAmount = purchaseOrder.SubTotal + purchaseOrder.TaxAmount + purchaseOrder.ShippingCost;

            await _context.SaveChangesAsync();

            return await GetPurchaseOrderByIdAsync(purchaseOrder.POId);
        }

        public async Task<PurchaseOrderDto> UpdatePurchaseOrderAsync(int poId, PurchaseOrderDto purchaseOrderDto)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(poId);
            if (purchaseOrder == null) return null;

            purchaseOrder.Status = purchaseOrderDto.Status;
            purchaseOrder.ShippingMethod = purchaseOrderDto.ShippingMethod;
            purchaseOrder.ShippingCost = purchaseOrderDto.ShippingCost;
            purchaseOrder.Notes = purchaseOrderDto.Notes;
            purchaseOrder.PaymentStatus = purchaseOrderDto.PaymentStatus;
            purchaseOrder.PaymentDate = purchaseOrderDto.PaymentStatus == "Paid" ? DateTime.UtcNow : null;
            purchaseOrder.ActualDeliveryDate = (DateTime)purchaseOrderDto.ActualDeliveryDate;

            await _context.SaveChangesAsync();
            return await GetPurchaseOrderByIdAsync(poId);
        }

        public async Task<bool> DeletePurchaseOrderAsync(int poId)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(poId);
            if (purchaseOrder == null) return false;

            if (purchaseOrder.Status == "Received") return false; // Can't delete received orders

            _context.PurchaseOrders.Remove(purchaseOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        private string GeneratePONumber()
        {
            return "PO-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
        }
    }

    public interface IShipmentRepository
    {
        Task<ShipmentDto> GetShipmentByIdAsync(int shipmentId);
        Task<List<ShipmentDto>> GetShipmentsByPurchaseOrderAsync(int poId);
        Task<ShipmentDto> CreateShipmentAsync(ShipmentDto shipmentDto);
        Task<ShipmentDto> UpdateShipmentAsync(int shipmentId, ShipmentDto shipmentDto);
    }

    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ApplicationDbContext _context;

        public ShipmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShipmentDto> GetShipmentByIdAsync(int shipmentId)
        {
            var shipment = await _context.Shipments
                .Include(s => s.PurchaseOrder)
                .FirstOrDefaultAsync(s => s.ShipmentId == shipmentId);

            if (shipment == null) return null;

            return new ShipmentDto
            {
                ShipmentId = shipment.ShipmentId,
                POId = shipment.POId,
                ShipmentDate = shipment.ShipmentDate,
                EstimatedArrivalDate = (DateTime)shipment.EstimatedArrivalDate,
                ActualArrivalDate = shipment.ActualArrivalDate,
                TrackingNumber = shipment.TrackingNumber,
                Carrier = shipment.Carrier,
                Status = shipment.Status,
                Notes = shipment.Notes,
                PurchaseOrder = new PurchaseOrderDto
                {
                    POId = shipment.PurchaseOrder.POId,
                    PONumber = shipment.PurchaseOrder.PONumber
                }
            };
        }

        public async Task<List<ShipmentDto>> GetShipmentsByPurchaseOrderAsync(int poId)
        {
            return await _context.Shipments
                .Where(s => s.POId == poId)
                .Select(s => new ShipmentDto
                {
                    ShipmentId = s.ShipmentId,
                    POId = s.POId,
                    ShipmentDate = s.ShipmentDate,
                    EstimatedArrivalDate = (DateTime)s.EstimatedArrivalDate,
                    ActualArrivalDate = s.ActualArrivalDate,
                    TrackingNumber = s.TrackingNumber,
                    Carrier = s.Carrier,
                    Status = s.Status,
                    Notes = s.Notes
                }).ToListAsync();
        }

        public async Task<ShipmentDto> CreateShipmentAsync(ShipmentDto shipmentDto)
        {
            var shipment = new Shipment
            {
                POId = shipmentDto.POId,
                ShipmentDate = shipmentDto.ShipmentDate,
                EstimatedArrivalDate = shipmentDto.EstimatedArrivalDate,
                TrackingNumber = shipmentDto.TrackingNumber,
                Carrier = shipmentDto.Carrier,
                Status = "In Transit",
                Notes = shipmentDto.Notes
            };

            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();

            // Update PO status if first shipment
            var po = await _context.PurchaseOrders.FindAsync(shipmentDto.POId);
            if (po != null && po.Status == "Pending")
            {
                po.Status = "Shipped";
                await _context.SaveChangesAsync();
            }

            return await GetShipmentByIdAsync(shipment.ShipmentId);
        }

        public async Task<ShipmentDto> UpdateShipmentAsync(int shipmentId, ShipmentDto shipmentDto)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentId);
            if (shipment == null) return null;

            shipment.EstimatedArrivalDate = shipmentDto.EstimatedArrivalDate;
            shipment.ActualArrivalDate = shipmentDto.ActualArrivalDate;
            shipment.TrackingNumber = shipmentDto.TrackingNumber;
            shipment.Carrier = shipmentDto.Carrier;
            shipment.Status = shipmentDto.Status;
            shipment.Notes = shipmentDto.Notes;

            // Update PO status if all items received
            if (shipment.Status == "Delivered")
            {
                var po = await _context.PurchaseOrders
                    .Include(p => p.Items)
                    .FirstOrDefaultAsync(p => p.POId == shipment.POId);

                if (po != null && po.Items.All(i => i.Status == "Received"))
                {
                    po.Status = "Received";
                    po.ActualDeliveryDate = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            return await GetShipmentByIdAsync(shipmentId);
        }
    }

    public interface ISupplierPerformanceRepository
    {
        Task<List<SupplierPerformanceDto>> GetPerformanceRecordsBySupplierAsync(int supplierId);
        Task<SupplierPerformanceDto> CreatePerformanceRecordAsync(SupplierPerformanceDto performanceDto);
    }

    public class SupplierPerformanceRepository : ISupplierPerformanceRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierPerformanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplierPerformanceDto>> GetPerformanceRecordsBySupplierAsync(int supplierId)
        {
            return await _context.SupplierPerformances
                .Where(sp => sp.SupplierId == supplierId)
                .OrderByDescending(sp => sp.Date)
                .Select(sp => new SupplierPerformanceDto
                {
                    PerformanceId = sp.PerformanceId,
                    SupplierId = sp.SupplierId,
                    MetricType = sp.MetricType,
                    Score = sp.Score,
                    Date = sp.Date,
                    Notes = sp.Notes
                }).ToListAsync();
        }

        public async Task<SupplierPerformanceDto> CreatePerformanceRecordAsync(SupplierPerformanceDto performanceDto)
        {
            var performance = new SupplierPerformance
            {
                SupplierId = performanceDto.SupplierId,
                MetricType = performanceDto.MetricType,
                Score = performanceDto.Score,
                Date = DateTime.UtcNow,
                Notes = performanceDto.Notes
            };

            _context.SupplierPerformances.Add(performance);
            await _context.SaveChangesAsync();

            // Update supplier's overall performance rating
            var supplier = await _context.Suppliers.FindAsync(performanceDto.SupplierId);
            if (supplier != null)
            {
                var avgScore = await _context.SupplierPerformances
                    .Where(sp => sp.SupplierId == supplier.SupplierId)
                    .AverageAsync(sp => sp.Score);
                supplier.PerformanceRating = avgScore;
                await _context.SaveChangesAsync();
            }

            return new SupplierPerformanceDto
            {
                PerformanceId = performance.PerformanceId,
                SupplierId = performance.SupplierId,
                MetricType = performance.MetricType,
                Score = performance.Score,
                Date = performance.Date,
                Notes = performance.Notes
            };
        }
    }

    #endregion
}
