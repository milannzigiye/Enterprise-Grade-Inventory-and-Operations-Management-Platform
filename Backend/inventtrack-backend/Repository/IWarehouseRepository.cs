using System.Linq.Expressions;
using Amazon.Runtime;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;

namespace inventtrack_backend.Repository
{
        public interface IWarehouseRepository
        {
            Task<Warehouse?> GetByIdAsync(int id);
            Task<IEnumerable<Warehouse>> GetAllAsync();
            Task<PaginatedResponse<Warehouse>> GetPaginatedAsync(int pageNumber, int pageSize);
            Task<IEnumerable<Warehouse>> FindAsync(Expression<Func<Warehouse, bool>> predicate);
            Task AddAsync(Warehouse warehouse);
            Task UpdateAsync(Warehouse warehouse);
            Task DeleteAsync(Warehouse warehouse);
            Task<bool> ExistsAsync(int id);

            // Warehouse-specific methods
            Task<WarehouseWithDetailsDto?> GetWarehouseWithDetailsAsync(int id);
            Task<IEnumerable<WarehouseZone>> GetWarehouseZonesAsync(int warehouseId);
            Task<IEnumerable<StorageLocation>> GetWarehouseLocationsAsync(int warehouseId);
            Task<IEnumerable<WarehouseWorker>> GetWarehouseWorkersAsync(int warehouseId);
            Task<WarehouseStatsDto> GetWarehouseStatsAsync(int warehouseId);
        }
    }