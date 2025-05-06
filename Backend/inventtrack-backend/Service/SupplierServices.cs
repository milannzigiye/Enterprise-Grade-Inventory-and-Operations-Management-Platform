using Amazon.Runtime;
using AutoMapper;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;
using inventtrack_backend.Repository;

namespace inventtrack_backend.Service
{
        public interface ISupplierService
        {
            Task<PaginatedResponse<SupplierDto>> GetSuppliersAsync(int pageNumber, int pageSize);
            Task<SupplierWithDetailsDto?> GetSupplierWithDetailsAsync(int id);
            Task<SupplierDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto);
            Task UpdateSupplierAsync(int id, SupplierUpdateDto supplierUpdateDto);
            Task DeleteSupplierAsync(int id);
            Task<SupplierStatsDto> GetSupplierStatsAsync(int supplierId);
            Task<IEnumerable<SupplierProductDto>> GetSupplierProductsAsync(int supplierId);
            Task<SupplierProductDto> AddSupplierProductAsync(int supplierId, SupplierProductCreateDto supplierProductCreateDto);
            Task RemoveSupplierProductAsync(int supplierId, int productId);
            Task<IEnumerable<PurchaseOrderDto>> GetSupplierPurchaseOrdersAsync(int supplierId);
            Task<IEnumerable<SupplierPerformanceDto>> GetSupplierPerformanceMetricsAsync(int supplierId);
        }

        public class SupplierService : ISupplierService
        {
            private readonly ISupplierRepository _supplierRepository;
            private readonly IMapper _mapper;

            public SupplierService(
                ISupplierRepository supplierRepository,
                IMapper mapper)
            {
                _supplierRepository = supplierRepository;
                _mapper = mapper;
            }

            public async Task<PaginatedResponse<SupplierDto>> GetSuppliersAsync(int pageNumber, int pageSize)
            {
                var paginatedSuppliers = await _supplierRepository.GetPaginatedAsync(pageNumber, pageSize);
                var supplierDtos = _mapper.Map<List<SupplierDto>>(paginatedSuppliers.Items);

                return new PaginatedResponse<SupplierDto>(
                    supplierDtos,
                    paginatedSuppliers.TotalCount,
                    pageNumber,
                    pageSize
                );
            }

            public async Task<SupplierWithDetailsDto?> GetSupplierWithDetailsAsync(int id)
            {
                return await _supplierRepository.GetSupplierWithDetailsAsync(id);
            }

            public async Task<SupplierDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto)
            {
                var supplier = _mapper.Map<Supplier>(supplierCreateDto);
                supplier.OnboardingDate = DateTime.UtcNow;
                supplier.IsActive = true;

                await _supplierRepository.AddAsync(supplier);

                return _mapper.Map<SupplierDto>(supplier);
            }

            public async Task UpdateSupplierAsync(int id, SupplierUpdateDto supplierUpdateDto)
            {
                var supplier = await _supplierRepository.GetByIdAsync(id);
                if (supplier == null)
                {
                    throw new KeyNotFoundException($"Supplier with ID {id} not found");
                }

                _mapper.Map(supplierUpdateDto, supplier);

                await _supplierRepository.UpdateAsync(supplier);
            }

            public async Task DeleteSupplierAsync(int id)
            {
                var supplier = await _supplierRepository.GetByIdAsync(id);
                if (supplier == null)
                {
                    throw new KeyNotFoundException($"Supplier with ID {id} not found");
                }

                // Soft delete
                supplier.IsActive = false;
                await _supplierRepository.UpdateAsync(supplier);
            }

            public async Task<SupplierStatsDto> GetSupplierStatsAsync(int supplierId)
            {
                if (!await _supplierRepository.ExistsAsync(supplierId))
                {
                    throw new KeyNotFoundException($"Supplier with ID {supplierId} not found");
                }

                return await _supplierRepository.GetSupplierStatsAsync(supplierId);
            }

            public async Task<IEnumerable<SupplierProductDto>> GetSupplierProductsAsync(int supplierId)
            {
                if (!await _supplierRepository.ExistsAsync(supplierId))
                {
                    throw new KeyNotFoundException($"Supplier with ID {supplierId} not found");
                }

                return await _supplierRepository.GetSupplierProductsAsync(supplierId);
            }

            public async Task<SupplierProductDto> AddSupplierProductAsync(
                int supplierId,
                SupplierProductCreateDto supplierProductCreateDto)
            {
                if (!await _supplierRepository.ExistsAsync(supplierId))
                {
                    throw new KeyNotFoundException($"Supplier with ID {supplierId} not found");
                }

                var supplierProduct = _mapper.Map<SupplierProduct>(supplierProductCreateDto);
                supplierProduct.SupplierId = supplierId;

                await _supplierRepository.AddSupplierProductAsync(supplierProduct);

                return _mapper.Map<SupplierProductDto>(supplierProduct);
            }

            public async Task RemoveSupplierProductAsync(int supplierId, int productId)
            {
                if (!await _supplierRepository.ExistsAsync(supplierId))
                {
                    throw new KeyNotFoundException($"Supplier with ID {supplierId} not found");
                }

                await _supplierRepository.RemoveSupplierProductAsync(supplierId, productId);
            }

            public async Task<IEnumerable<PurchaseOrderDto>> GetSupplierPurchaseOrdersAsync(int supplierId)
            {
                if (!await _supplierRepository.ExistsAsync(supplierId))
                {
                    throw new KeyNotFoundException($"Supplier with ID {supplierId} not found");
                }

                return await _supplierRepository.GetSupplierPurchaseOrdersAsync(supplierId);
            }

            public async Task<IEnumerable<SupplierPerformanceDto>> GetSupplierPerformanceMetricsAsync(int supplierId)
            {
                if (!await _supplierRepository.ExistsAsync(supplierId))
                {
                    throw new KeyNotFoundException($"Supplier with ID {supplierId} not found");
                }

                return await _supplierRepository.GetSupplierPerformanceMetricsAsync(supplierId);
            }
        }
    }
