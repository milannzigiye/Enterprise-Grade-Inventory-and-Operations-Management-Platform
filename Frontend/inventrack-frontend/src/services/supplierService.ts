import api from './api';
import type { Supplier, PurchaseOrder } from '../types/index';

export interface SupplierCreateDto {
  supplierName: string;
  contactPerson?: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
}

export interface SupplierUpdateDto extends SupplierCreateDto {
  isActive: boolean;
}

export const supplierService = {
  // Get all suppliers
  getAllSuppliers: async (): Promise<Supplier[]> => {
    const response = await api.get('/Supplier');
    return response.data;
  },

  // Get supplier by ID
  getSupplierById: async (supplierId: number): Promise<Supplier> => {
    const response = await api.get(`/Supplier/${supplierId}`);
    return response.data;
  },

  // Create new supplier
  createSupplier: async (supplier: SupplierCreateDto): Promise<Supplier> => {
    const response = await api.post('/Supplier', supplier);
    return response.data;
  },

  // Update supplier
  updateSupplier: async (supplierId: number, supplier: SupplierUpdateDto): Promise<Supplier> => {
    const response = await api.put(`/Supplier/${supplierId}`, supplier);
    return response.data;
  },

  // Delete supplier
  deleteSupplier: async (supplierId: number): Promise<void> => {
    await api.delete(`/Supplier/${supplierId}`);
  },

  // Get supplier purchase orders
  getSupplierPurchaseOrders: async (supplierId: number): Promise<PurchaseOrder[]> => {
    const response = await api.get(`/PurchaseOrder/supplier/${supplierId}`);
    return response.data;
  },

  // Get supplier performance metrics
  getSupplierPerformance: async (supplierId: number): Promise<any> => {
    const response = await api.get(`/Supplier/${supplierId}/performance`);
    return response.data;
  },
};
