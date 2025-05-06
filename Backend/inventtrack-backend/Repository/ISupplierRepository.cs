using Amazon.Runtime;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using System.Linq.Expressions;

namespace inventtrack_backend.Repository
{
    public interface ISupplierRepository
    {
        // Basic CRUD operations
        Task<Supplier?> GetByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<PaginatedResponse<Supplier>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Supplier>> FindAsync(Expression<Func<Supplier, bool>> predicate);
        Task AddAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(Supplier supplier);
        Task<bool> ExistsAsync(int id);

        // Supplier-specific methods
        Task<SupplierWithDetailsDto?> GetSupplierWithDetailsAsync(int id);
        Task<IEnumerable<SupplierProductDto>> GetSupplierProductsAsync(int supplierId);
        Task<IEnumerable<PurchaseOrderDto>> GetSupplierPurchaseOrdersAsync(int supplierId);
        Task<IEnumerable<SupplierPerformanceDto>> GetSupplierPerformanceMetricsAsync(int supplierId);
        Task<SupplierStatsDto> GetSupplierStatsAsync(int supplierId);
        Task AddSupplierProductAsync(SupplierProduct supplierProduct);
        Task RemoveSupplierProductAsync(int supplierId, int productId);
    }
}
