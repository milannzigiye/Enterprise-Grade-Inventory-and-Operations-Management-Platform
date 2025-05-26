import api from './api';
import type { SalesStatistics, InventoryStatistics, DashboardWidget } from '../types/index';

export interface DashboardStats {
  totalProducts: number;
  totalCustomers: number;
  totalOrders: number;
  totalRevenue: number;
  lowStockItems: number;
  pendingOrders: number;
  revenueGrowth: number;
  orderGrowth: number;
}

export interface SalesReport {
  period: string;
  totalSales: number;
  totalOrders: number;
  averageOrderValue: number;
  topProducts: Array<{
    productName: string;
    quantity: number;
    revenue: number;
  }>;
}

export interface InventoryReport {
  totalProducts: number;
  totalValue: number;
  lowStockItems: number;
  outOfStockItems: number;
  topCategories: Array<{
    categoryName: string;
    productCount: number;
    totalValue: number;
  }>;
}

export const analyticsService = {
  // Get dashboard statistics
  getDashboardStats: async (): Promise<DashboardStats> => {
    const response = await api.get('/Analytics/dashboard');
    return response.data;
  },

  // Get sales statistics
  getSalesStatistics: async (period?: string): Promise<SalesStatistics[]> => {
    const params = period ? { period } : {};
    const response = await api.get('/Analytics/sales', { params });
    return response.data;
  },

  // Get inventory statistics
  getInventoryStatistics: async (period?: string): Promise<InventoryStatistics[]> => {
    const params = period ? { period } : {};
    const response = await api.get('/Analytics/inventory', { params });
    return response.data;
  },

  // Get sales report
  getSalesReport: async (startDate: string, endDate: string): Promise<SalesReport> => {
    const response = await api.get('/Analytics/sales-report', {
      params: { startDate, endDate }
    });
    return response.data;
  },

  // Get inventory report
  getInventoryReport: async (): Promise<InventoryReport> => {
    const response = await api.get('/Analytics/inventory-report');
    return response.data;
  },

  // Get revenue trends
  getRevenueTrends: async (period: string = 'monthly'): Promise<Array<{ date: string; revenue: number }>> => {
    const response = await api.get('/Analytics/revenue-trends', {
      params: { period }
    });
    return response.data;
  },

  // Get order trends
  getOrderTrends: async (period: string = 'monthly'): Promise<Array<{ date: string; orders: number }>> => {
    const response = await api.get('/Analytics/order-trends', {
      params: { period }
    });
    return response.data;
  },

  // Get top selling products
  getTopSellingProducts: async (limit: number = 10): Promise<Array<{
    productId: number;
    productName: string;
    quantitySold: number;
    revenue: number;
  }>> => {
    const response = await api.get('/Analytics/top-products', {
      params: { limit }
    });
    return response.data;
  },

  // Get low stock alerts
  getLowStockAlerts: async (): Promise<Array<{
    productId: number;
    productName: string;
    currentStock: number;
    minimumStock: number;
    warehouseName: string;
  }>> => {
    const response = await api.get('/Analytics/low-stock-alerts');
    return response.data;
  },

  // Get dashboard widgets
  getDashboardWidgets: async (): Promise<DashboardWidget[]> => {
    const response = await api.get('/Analytics/widgets');
    return response.data;
  },
};
