import api from './api';
import type { Customer, CustomerCreateDto, CustomerMembership } from '../types/index';

export const customerService = {
  // Customer operations
  async getAllCustomers(): Promise<Customer[]> {
    const response = await api.get('/Customer');
    return response.data;
  },

  async getCustomerById(customerId: number): Promise<Customer> {
    const response = await api.get(`/Customer/${customerId}`);
    return response.data;
  },

  async createCustomer(customer: CustomerCreateDto): Promise<Customer> {
    const response = await api.post('/Customer', customer);
    return response.data;
  },

  async updateCustomer(customerId: number, customer: Partial<Customer>): Promise<Customer> {
    const response = await api.put(`/Customer/${customerId}`, customer);
    return response.data;
  },

  async deleteCustomer(customerId: number): Promise<void> {
    await api.delete(`/Customer/${customerId}`);
  },

  async searchCustomers(searchTerm: string): Promise<Customer[]> {
    const response = await api.get(`/Customer/search?term=${encodeURIComponent(searchTerm)}`);
    return response.data;
  },

  // Customer Membership operations
  async getMembershipByCustomer(customerId: number): Promise<CustomerMembership> {
    const response = await api.get(`/CustomerMembership/customer/${customerId}`);
    return response.data;
  },

  async createMembership(membership: Omit<CustomerMembership, 'membershipId'>): Promise<CustomerMembership> {
    const response = await api.post('/CustomerMembership', membership);
    return response.data;
  },

  async updateMembership(membershipId: number, membership: Partial<CustomerMembership>): Promise<CustomerMembership> {
    const response = await api.put(`/CustomerMembership/${membershipId}`, membership);
    return response.data;
  },

  async deleteMembership(membershipId: number): Promise<void> {
    await api.delete(`/CustomerMembership/${membershipId}`);
  },
};
