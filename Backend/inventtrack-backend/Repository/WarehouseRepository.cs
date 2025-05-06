using System.Linq.Expressions;
using inventtrack_backend.Database;
using inventtrack_backend.Model;
using Microsoft.EntityFrameworkCore;
using inventtrack_backend.Database;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using Amazon.Runtime;

namespace omniflow_backend.Repository.Implementation
{
   
        public class WarehouseRepository : IWarehouseRepository
        {
            private readonly DbOmniflow _context;
            private readonly DbSet<Warehouse> _dbSet;

            public WarehouseRepository(DbOmniflow context)
            {
                _context = context;
                _dbSet = context.Set<Warehouse>();
            }

            public async Task<Warehouse?> GetByIdAsync(int id)
            {
                return await _dbSet.FindAsync(id);
            }

            public async Task<IEnumerable<Warehouse>> GetAllAsync()
            {
                return await _dbSet.ToListAsync();
            }

            public async Task<PaginatedResponse<Warehouse>> GetPaginatedAsync(int pageNumber, int pageSize)
            {
                var query = _dbSet.AsQueryable();
                var totalCount = await query.CountAsync();
                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginatedResponse<Warehouse>(items, totalCount, pageNumber, pageSize);
            }

            public async Task<IEnumerable<Warehouse>> FindAsync(Expression<Func<Warehouse, bool>> predicate)
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }

            public async Task AddAsync(Warehouse warehouse)
            {
                await _dbSet.AddAsync(warehouse);
            }

            public async Task UpdateAsync(Warehouse warehouse)
            {
                _dbSet.Update(warehouse);
                await Task.CompletedTask;
            }

            public async Task DeleteAsync(Warehouse warehouse)
            {
                _dbSet.Remove(warehouse);
                await Task.CompletedTask;
            }

            public async Task<bool> ExistsAsync(int id)
            {
                return await _dbSet.AnyAsync(w => w.WarehouseId == id);
            }

            public async Task<WarehouseWithDetailsDto?> GetWarehouseWithDetailsAsync(int id)
            {
                return await _dbSet
                    .Where(w => w.WarehouseId == id)
                    .Select(w => new WarehouseWithDetailsDto
                    {
                        WarehouseId = w.WarehouseId,
                        WarehouseName = w.WarehouseName,
                        Address = w.Address,
                        City = w.City,
                        State = w.State,
                        ZipCode = w.ZipCode,
                        Country = w.Country,
                        PhoneNumber = w.PhoneNumber,
                        Email = w.Email,
                        CapacitySquareFeet = w.CapacitySquareFeet,
                        IsActive = w.IsActive,
                        CreatedDate = w.CreatedDate,
                        LastModifiedDate = (DateTime)w.LastModifiedDate,
                        Zones = w.Zones.Select(z => new WarehouseZoneDto
                        {
                            ZoneId = z.ZoneId,
                            ZoneName = z.ZoneName,
                            ZoneType = z.ZoneType,
                            FloorLevel = z.FloorLevel,
                            Description = z.Description,
                            IsActive = z.IsActive
                        }).ToList(),
                        Workers = w.Workers.Select(ww => new WarehouseWorkerDto
                        {
                            WorkerId = ww.WorkerId,
                            UserId = ww.UserId,
                            UserName = ww.User.Username,
                            FullName = $"{ww.User.Profile.FirstName} {ww.User.Profile.LastName}",
                            Position = ww.Position,
                            HireDate = ww.HireDate,
                            SupervisorId = ww.SupervisorId,
                            SupervisorName = ww.Supervisor != null ?
                                $"{ww.Supervisor.Profile.FirstName} {ww.Supervisor.Profile.LastName}" : null,
                            IsActive = ww.IsActive
                        }).ToList(),
                        UtilizationPercent = w.Inventories.Sum(i => i.Location.CurrentUtilization) /
                                            w.Inventories.Sum(i => i.Location.MaxCapacity) * 100
                    })
                    .FirstOrDefaultAsync();
            }

            public async Task<IEnumerable<WarehouseZone>> GetWarehouseZonesAsync(int warehouseId)
            {
                return await _context.WarehouseZones
                    .Where(z => z.WarehouseId == warehouseId)
                    .ToListAsync();
            }

            public async Task<IEnumerable<StorageLocation>> GetWarehouseLocationsAsync(int warehouseId)
            {
                return await _context.StorageLocations
                    .Include(l => l.Zone)
                    .Where(l => l.Zone.WarehouseId == warehouseId)
                    .ToListAsync();
            }

            public async Task<IEnumerable<WarehouseWorker>> GetWarehouseWorkersAsync(int warehouseId)
            {
                return await _context.WarehouseWorkers
                    .Include(w => w.User)
                    .ThenInclude(u => u.Profile)
                    .Where(w => w.WarehouseId == warehouseId)
                    .ToListAsync();
            }

            public async Task<WarehouseStatsDto> GetWarehouseStatsAsync(int warehouseId)
            {
                var stats = new WarehouseStatsDto
                {
                    TotalZones = await _context.WarehouseZones
                        .CountAsync(z => z.WarehouseId == warehouseId),
                    TotalLocations = await _context.StorageLocations
                        .Include(l => l.Zone)
                        .CountAsync(l => l.Zone.WarehouseId == warehouseId),
                    TotalWorkers = await _context.WarehouseWorkers
                        .CountAsync(w => w.WarehouseId == warehouseId && w.IsActive),
                    TotalProducts = await _context.Inventories
                        .Where(i => i.WarehouseId == warehouseId)
                        .Select(i => i.ProductId)
                        .Distinct()
                        .CountAsync(),
                    TotalInventoryValue = await _context.Inventories
                        .Include(i => i.Product)
                        .Where(i => i.WarehouseId == warehouseId)
                        .SumAsync(i => i.QuantityOnHand * i.Product.UnitCost),
                    UtilizationPercent = await _context.StorageLocations
                        .Include(l => l.Zone)
                        .Where(l => l.Zone.WarehouseId == warehouseId)
                        .AverageAsync(l => (l.CurrentUtilization / l.MaxCapacity) * 100)
                };

                return stats;
            }
        }
    }

