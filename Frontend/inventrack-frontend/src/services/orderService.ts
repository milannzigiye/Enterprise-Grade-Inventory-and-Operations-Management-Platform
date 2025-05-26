import api from './api';
import type { Order, OrderCreateDto, OrderItem } from '../types/index';

export const orderService = {
  // Order operations
  async getAllOrders(): Promise<Order[]> {
    const response = await api.get('/Order');
    return response.data;
  },

  async getOrderById(orderId: number): Promise<Order> {
    const response = await api.get(`/Order/${orderId}`);
    return response.data;
  },

  async getOrdersByCustomer(customerId: number): Promise<Order[]> {
    const response = await api.get(`/Order/customer/${customerId}`);
    return response.data;
  },

  async getOrdersByStatus(status: string): Promise<Order[]> {
    const response = await api.get(`/Order/status/${status}`);
    return response.data;
  },

  async createOrder(order: OrderCreateDto): Promise<Order> {
    const response = await api.post('/Order', order);
    return response.data;
  },

  async updateOrder(orderId: number, order: Partial<Order>): Promise<Order> {
    const response = await api.put(`/Order/${orderId}`, order);
    return response.data;
  },

  async deleteOrder(orderId: number): Promise<void> {
    await api.delete(`/Order/${orderId}`);
  },

  async updateOrderStatus(orderId: number, status: string): Promise<Order> {
    const response = await api.put(`/Order/${orderId}/status`, { status });
    return response.data;
  },

  // Order Item operations
  async getOrderItems(orderId: number): Promise<OrderItem[]> {
    const response = await api.get(`/OrderItem/order/${orderId}`);
    return response.data;
  },

  async addOrderItem(orderItem: Omit<OrderItem, 'orderItemId'>): Promise<OrderItem> {
    const response = await api.post('/OrderItem', orderItem);
    return response.data;
  },

  async updateOrderItem(orderItemId: number, orderItem: Partial<OrderItem>): Promise<OrderItem> {
    const response = await api.put(`/OrderItem/${orderItemId}`, orderItem);
    return response.data;
  },

  async deleteOrderItem(orderItemId: number): Promise<void> {
    await api.delete(`/OrderItem/${orderItemId}`);
  },
};
