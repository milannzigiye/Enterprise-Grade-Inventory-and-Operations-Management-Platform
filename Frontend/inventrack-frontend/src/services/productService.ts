import api from './api';
import type { Product, ProductCategory, ProductCreateDto, Inventory, ProductVariant } from '../types/index';

export const productService = {
  // Product operations
  async getAllProducts(): Promise<Product[]> {
    const response = await api.get('/Product');
    return response.data;
  },

  async getProductById(productId: number): Promise<Product> {
    const response = await api.get(`/Product/${productId}`);
    return response.data;
  },

  async getProductsByCategory(categoryId: number): Promise<Product[]> {
    const response = await api.get(`/Product/category/${categoryId}`);
    return response.data;
  },

  async createProduct(product: ProductCreateDto): Promise<Product> {
    const response = await api.post('/Product', product);
    return response.data;
  },

  async updateProduct(productId: number, product: Partial<Product>): Promise<Product> {
    const response = await api.put(`/Product/${productId}`, product);
    return response.data;
  },

  async deleteProduct(productId: number): Promise<void> {
    await api.delete(`/Product/${productId}`);
  },

  async searchProducts(searchTerm: string): Promise<Product[]> {
    const response = await api.get(`/Product/search?term=${encodeURIComponent(searchTerm)}`);
    return response.data;
  },

  // Product Category operations
  async getAllCategories(): Promise<ProductCategory[]> {
    const response = await api.get('/ProductCategory');
    return response.data;
  },

  async getCategoryById(categoryId: number): Promise<ProductCategory> {
    const response = await api.get(`/ProductCategory/${categoryId}`);
    return response.data;
  },

  async createCategory(category: Omit<ProductCategory, 'categoryId'>): Promise<ProductCategory> {
    const response = await api.post('/ProductCategory', category);
    return response.data;
  },

  async updateCategory(categoryId: number, category: Partial<ProductCategory>): Promise<ProductCategory> {
    const response = await api.put(`/ProductCategory/${categoryId}`, category);
    return response.data;
  },

  async deleteCategory(categoryId: number): Promise<void> {
    await api.delete(`/ProductCategory/${categoryId}`);
  },

  // Inventory operations
  async getInventoryByProduct(productId: number): Promise<Inventory[]> {
    const response = await api.get(`/Inventory/product/${productId}`);
    return response.data;
  },

  async getInventoryByWarehouse(warehouseId: number): Promise<Inventory[]> {
    const response = await api.get(`/Inventory/warehouse/${warehouseId}`);
    return response.data;
  },

  async updateInventory(inventoryId: number, inventory: Partial<Inventory>): Promise<Inventory> {
    const response = await api.put(`/Inventory/${inventoryId}`, inventory);
    return response.data;
  },

  async getLowStockItems(): Promise<Inventory[]> {
    const response = await api.get('/Inventory/low-stock');
    return response.data;
  },

  // Product Variant operations
  async getVariantsByProduct(productId: number): Promise<ProductVariant[]> {
    const response = await api.get(`/ProductVariant/product/${productId}`);
    return response.data;
  },

  async getVariantById(variantId: number): Promise<ProductVariant> {
    const response = await api.get(`/ProductVariant/${variantId}`);
    return response.data;
  },

  async createVariant(variant: Omit<ProductVariant, 'variantId'>): Promise<ProductVariant> {
    const response = await api.post('/ProductVariant', variant);
    return response.data;
  },

  async updateVariant(variantId: number, variant: Partial<ProductVariant>): Promise<ProductVariant> {
    const response = await api.put(`/ProductVariant/${variantId}`, variant);
    return response.data;
  },

  async deleteVariant(variantId: number): Promise<void> {
    await api.delete(`/ProductVariant/${variantId}`);
  },
};
