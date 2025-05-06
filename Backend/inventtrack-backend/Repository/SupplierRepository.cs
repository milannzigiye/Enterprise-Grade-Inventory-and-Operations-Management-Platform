using Amazon.Runtime;
using inventtrack_backend.Database;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace inventtrack_backend.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DbOmniflow _context;
        private readonly DbSet<Supplier> _dbSet;

        public SupplierRepository(DbOmniflow context)
        {
            _context = context;
            _dbSet = context.Set<Supplier>();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<PaginatedResponse<Supplier>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<Supplier>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Supplier>> FindAsync(Expression<Func<Supplier, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(Supplier supplier)
        {
            await _dbSet.AddAsync(supplier);
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _dbSet.Update(supplier);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Supplier supplier)
        {
            _dbSet.Remove(supplier);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(s => s.SupplierId == id);
        }

        public async Task<SupplierWithDetailsDto?> GetSupplierWithDetailsAsync(int id)
        {
            return await _dbSet
                .Where(s => s.SupplierId == id)
                .Select(s => new SupplierWithDetailsDto
                {
                    SupplierId = s.SupplierId,
                    CompanyName = s.CompanyName,
                    ContactName = s.ContactName,
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    City = s.City,
                    State = s.State,
                    ZipCode = s.ZipCode,
                    Country = s.Country,
                    TaxIdentificationNumber = s.TaxIdentificationNumber,
                    PaymentTerms = s.PaymentTerms,
                    LeadTimeInDays = s.LeadTimeInDays,
                    IsActive = s.IsActive,
                    OnboardingDate = s.OnboardingDate,
                    LastOrderDate = s.LastOrderDate,
                    PerformanceRating = (int)s.PerformanceRating,
                    Products = s.SupplierProducts.Select(sp => new SupplierProductDto
                    {
                        SupplierProductId = sp.SupplierProductId,
                        ProductId = sp.ProductId,
                        ProductName = sp.Product.Name,
                        ProductSKU = sp.Product.SKU,
                        SupplierSKU = sp.SupplierSKU,
                        UnitCost = sp.UnitCost,
                        MinimumOrderQuantity = sp.MinimumOrderQuantity,
                        IsPreferredSupplier = sp.IsPreferredSupplier,
                        LeadTimeInDays = sp.LeadTimeInDays,
                        LastPurchaseDate = sp.LastPurchaseDate
                    }).ToList(),
                    PurchaseOrders = s.PurchaseOrders.Select(po => new PurchaseOrderDto
                    {
                        POId = po.POId,
                        PONumber = po.PONumber,
                        OrderDate = po.OrderDate,
                        ExpectedDeliveryDate = (DateTime)po.ExpectedDeliveryDate,
                        Status = po.Status,
                        TotalAmount = po.TotalAmount,
                        PaymentStatus = po.PaymentStatus
                    }).ToList(),
                    PerformanceMetrics = s.PerformanceRecords.Select(pr => new SupplierPerformanceDto
                    {
                        PerformanceId = pr.PerformanceId,
                        MetricType = pr.MetricType,
                        Score = (int)pr.Score,
                        Date = pr.Date,
                        Notes = pr.Notes
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SupplierProductDto>> GetSupplierProductsAsync(int supplierId)
        {
            return await _context.SupplierProducts
                .Where(sp => sp.SupplierId == supplierId)
                .Select(sp => new SupplierProductDto
                {
                    SupplierProductId = sp.SupplierProductId,
                    SupplierId = sp.SupplierId,
                    ProductId = sp.ProductId,
                    ProductName = sp.Product.Name,
                    ProductSKU = sp.Product.SKU,
                    SupplierSKU = sp.SupplierSKU,
                    UnitCost = sp.UnitCost,
                    MinimumOrderQuantity = sp.MinimumOrderQuantity,
                    IsPreferredSupplier = sp.IsPreferredSupplier,
                    LeadTimeInDays = sp.LeadTimeInDays,
                    LastPurchaseDate = sp.LastPurchaseDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetSupplierPurchaseOrdersAsync(int supplierId)
        {
            return await _context.PurchaseOrders
                .Where(po => po.SupplierId == supplierId)
                .OrderByDescending(po => po.OrderDate)
                .Select(po => new PurchaseOrderDto
                {
                    POId = po.POId,
                    PONumber = po.PONumber,
                    OrderDate = po.OrderDate,
                    ExpectedDeliveryDate = (DateTime)po.ExpectedDeliveryDate,
                    Status = po.Status,
                    TotalAmount = po.TotalAmount,
                    PaymentStatus = po.PaymentStatus,
                    Items = po.Items.Select(poi => new PurchaseOrderItemDto
                    {
                        POItemId = poi.POItemId,
                        ProductId = poi.ProductId,
                        ProductName = poi.Product.Name,
                        QuantityOrdered = poi.QuantityOrdered,
                        UnitCost = poi.UnitCost,
                        TotalCost = poi.TotalCost,
                        QuantityReceived = poi.QuantityReceived,
                        Status = poi.Status
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SupplierPerformanceDto>> GetSupplierPerformanceMetricsAsync(int supplierId)
        {
            return await _context.SupplierPerformances
                .Where(sp => sp.SupplierId == supplierId)
                .OrderByDescending(sp => sp.Date)
                .Select(sp => new SupplierPerformanceDto
                {
                    PerformanceId = sp.PerformanceId,
                    MetricType = sp.MetricType,
                    Score = (int)sp.Score,
                    Date = sp.Date,
                    Notes = sp.Notes
                })
                .ToListAsync();
        }

        public async Task<SupplierStatsDto> GetSupplierStatsAsync(int supplierId)
        {
            var stats = new SupplierStatsDto
            {
                TotalProducts = await _context.SupplierProducts
                    .CountAsync(sp => sp.SupplierId == supplierId),
                TotalPurchaseOrders = await _context.PurchaseOrders
                    .CountAsync(po => po.SupplierId == supplierId),
                TotalSpend = await _context.PurchaseOrders
                    .Where(po => po.SupplierId == supplierId)
                    .SumAsync(po => po.TotalAmount),
                AvgDeliveryTimeDays = await _context.PurchaseOrders
                    .Where(po => po.SupplierId == supplierId && po.Status == "Complete")
                    .AverageAsync(po => (po.ActualDeliveryDate - po.OrderDate).Value.TotalDays),
                OnTimeDeliveryPercentage = await _context.PurchaseOrders
                    .Where(po => po.SupplierId == supplierId && po.Status == "Complete")
                    .AverageAsync(po => po.ActualDeliveryDate <= po.ExpectedDeliveryDate ? 100 : 0),
                AvgQualityScore = await _context.SupplierPerformances
                    .Where(sp => sp.SupplierId == supplierId && sp.MetricType == "Quality")
                    .AverageAsync(sp => sp.Score)
            };

            return stats;
        }

        public async Task AddSupplierProductAsync(SupplierProduct supplierProduct)
        {
            await _context.SupplierProducts.AddAsync(supplierProduct);
        }

        public async Task RemoveSupplierProductAsync(int supplierId, int productId)
        {
            var supplierProduct = await _context.SupplierProducts
                .FirstOrDefaultAsync(sp => sp.SupplierId == supplierId && sp.ProductId == productId);

            if (supplierProduct != null)
            {
                _context.SupplierProducts.Remove(supplierProduct);
            }
        }
    }
}
