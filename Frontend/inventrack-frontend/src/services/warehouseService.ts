import api from './api';
import type { Warehouse, WarehouseZone } from '../types/index';

export interface WarehouseCreateDto {
  warehouseName: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  capacity?: number;
}

export interface WarehouseUpdateDto extends WarehouseCreateDto {
  isActive: boolean;
}

export const warehouseService = {
  // Get all warehouses
  getAllWarehouses: async (): Promise<Warehouse[]> => {
    const response = await api.get('/Warehouse');
    return response.data;
  },

  // Get warehouse by ID
  getWarehouseById: async (warehouseId: number): Promise<Warehouse> => {
    const response = await api.get(`/Warehouse/${warehouseId}`);
    return response.data;
  },

  // Create new warehouse
  createWarehouse: async (warehouse: WarehouseCreateDto): Promise<Warehouse> => {
    const response = await api.post('/Warehouse', warehouse);
    return response.data;
  },

  // Update warehouse
  updateWarehouse: async (warehouseId: number, warehouse: WarehouseUpdateDto): Promise<Warehouse> => {
    const response = await api.put(`/Warehouse/${warehouseId}`, warehouse);
    return response.data;
  },

  // Delete warehouse
  deleteWarehouse: async (warehouseId: number): Promise<void> => {
    await api.delete(`/Warehouse/${warehouseId}`);
  },

  // Get warehouse zones
  getWarehouseZones: async (warehouseId: number): Promise<WarehouseZone[]> => {
    const response = await api.get(`/WarehouseZone/warehouse/${warehouseId}`);
    return response.data;
  },

  // Get warehouse utilization
  getWarehouseUtilization: async (warehouseId: number): Promise<{ utilization: number; capacity: number }> => {
    const response = await api.get(`/Warehouse/${warehouseId}/utilization`);
    return response.data;
  },
};
