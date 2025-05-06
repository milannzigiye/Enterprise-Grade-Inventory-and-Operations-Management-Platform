using Amazon.Runtime;
using AutoMapper;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using inventtrack_backend.Repository;

namespace inventtrack_backend.Service
{
    
        public interface IWarehouseService
        {
            Task<PaginatedResponse<WarehouseDto>> GetWarehousesAsync(int pageNumber, int pageSize);
            Task<WarehouseWithDetailsDto?> GetWarehouseWithDetailsAsync(int id);
            Task<WarehouseDto> CreateWarehouseAsync(WarehouseCreateDto warehouseCreateDto);
            Task UpdateWarehouseAsync(int id, WarehouseUpdateDto warehouseUpdateDto);
            Task DeleteWarehouseAsync(int id);
            Task<WarehouseStatsDto> GetWarehouseStatsAsync(int warehouseId);
            Task<IEnumerable<WarehouseZoneDto>> GetWarehouseZonesAsync(int warehouseId);
            Task<IEnumerable<StorageLocationDto>> GetWarehouseLocationsAsync(int warehouseId);
            Task<IEnumerable<WarehouseWorkerDto>> GetWarehouseWorkersAsync(int warehouseId);
        }

        public class WarehouseService : IWarehouseService
        {
            private readonly IWarehouseRepository _warehouseRepository;
            private readonly IMapper _mapper;

            public WarehouseService(
                IWarehouseRepository warehouseRepository,
                IMapper mapper)
            {
                _warehouseRepository = warehouseRepository;
                _mapper = mapper;
            }

            public async Task<PaginatedResponse<WarehouseDto>> GetWarehousesAsync(int pageNumber, int pageSize)
            {
                var paginatedWarehouses = await _warehouseRepository.GetPaginatedAsync(pageNumber, pageSize);
                var warehouseDtos = _mapper.Map<List<WarehouseDto>>(paginatedWarehouses.Items);

                return new PaginatedResponse<WarehouseDto>(
                    warehouseDtos,
                    paginatedWarehouses.TotalCount,
                    pageNumber,
                    pageSize
                );
            }

            public async Task<WarehouseWithDetailsDto?> GetWarehouseWithDetailsAsync(int id)
            {
                return await _warehouseRepository.GetWarehouseWithDetailsAsync(id);
            }

            public async Task<WarehouseDto> CreateWarehouseAsync(WarehouseCreateDto warehouseCreateDto)
            {
                var warehouse = _mapper.Map<Warehouse>(warehouseCreateDto);
                warehouse.CreatedDate = DateTime.UtcNow;
                warehouse.IsActive = true;

                await _warehouseRepository.AddAsync(warehouse);

                return _mapper.Map<WarehouseDto>(warehouse);
            }

            public async Task UpdateWarehouseAsync(int id, WarehouseUpdateDto warehouseUpdateDto)
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(id);
                if (warehouse == null)
                {
                    throw new KeyNotFoundException($"Warehouse with ID {id} not found");
                }

                _mapper.Map(warehouseUpdateDto, warehouse);
                warehouse.LastModifiedDate = DateTime.UtcNow;

                await _warehouseRepository.UpdateAsync(warehouse);
            }

            public async Task DeleteWarehouseAsync(int id)
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(id);
                if (warehouse == null)
                {
                    throw new KeyNotFoundException($"Warehouse with ID {id} not found");
                }

                // Soft delete
                warehouse.IsActive = false;
                await _warehouseRepository.UpdateAsync(warehouse);
            }

            public async Task<WarehouseStatsDto> GetWarehouseStatsAsync(int warehouseId)
            {
                if (!await _warehouseRepository.ExistsAsync(warehouseId))
                {
                    throw new KeyNotFoundException($"Warehouse with ID {warehouseId} not found");
                }

                return await _warehouseRepository.GetWarehouseStatsAsync(warehouseId);
            }

            public async Task<IEnumerable<WarehouseZoneDto>> GetWarehouseZonesAsync(int warehouseId)
            {
                if (!await _warehouseRepository.ExistsAsync(warehouseId))
                {
                    throw new KeyNotFoundException($"Warehouse with ID {warehouseId} not found");
                }

                var zones = await _warehouseRepository.GetWarehouseZonesAsync(warehouseId);
                return _mapper.Map<IEnumerable<WarehouseZoneDto>>(zones);
            }

            public async Task<IEnumerable<StorageLocationDto>> GetWarehouseLocationsAsync(int warehouseId)
            {
                if (!await _warehouseRepository.ExistsAsync(warehouseId))
                {
                    throw new KeyNotFoundException($"Warehouse with ID {warehouseId} not found");
                }

                var locations = await _warehouseRepository.GetWarehouseLocationsAsync(warehouseId);
                return _mapper.Map<IEnumerable<StorageLocationDto>>(locations);
            }

            public async Task<IEnumerable<WarehouseWorkerDto>> GetWarehouseWorkersAsync(int warehouseId)
            {
                if (!await _warehouseRepository.ExistsAsync(warehouseId))
                {
                    throw new KeyNotFoundException($"Warehouse with ID {warehouseId} not found");
                }

                var workers = await _warehouseRepository.GetWarehouseWorkersAsync(warehouseId);
                return _mapper.Map<IEnumerable<WarehouseWorkerDto>>(workers);
            }
        }
    }

