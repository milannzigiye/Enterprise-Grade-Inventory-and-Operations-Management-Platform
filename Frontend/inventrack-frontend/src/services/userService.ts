import api from './api';
import type { User, Role } from '../types/index';

export interface UserCreateDto {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  password: string;
  roleIds?: number[];
}

export interface UserUpdateDto {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  isActive: boolean;
  roleIds?: number[];
}

export interface UserProfileUpdateDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
}

export const userService = {
  // Get all users
  getAllUsers: async (): Promise<User[]> => {
    const response = await api.get('/User');
    return response.data;
  },

  // Get user by ID
  getUserById: async (userId: number): Promise<User> => {
    const response = await api.get(`/User/${userId}`);
    return response.data;
  },

  // Create new user
  createUser: async (user: UserCreateDto): Promise<User> => {
    const response = await api.post('/User', user);
    return response.data;
  },

  // Update user
  updateUser: async (userId: number, user: UserUpdateDto): Promise<User> => {
    const response = await api.put(`/User/${userId}`, user);
    return response.data;
  },

  // Delete user
  deleteUser: async (userId: number): Promise<void> => {
    await api.delete(`/User/${userId}`);
  },

  // Get current user profile
  getCurrentUser: async (): Promise<User> => {
    const response = await api.get('/User/profile');
    return response.data;
  },

  // Update current user profile
  updateProfile: async (profile: UserProfileUpdateDto): Promise<User> => {
    const response = await api.put('/User/profile', profile);
    return response.data;
  },

  // Change password
  changePassword: async (currentPassword: string, newPassword: string): Promise<void> => {
    await api.post('/User/change-password', {
      currentPassword,
      newPassword,
    });
  },

  // Get all roles
  getAllRoles: async (): Promise<Role[]> => {
    const response = await api.get('/Role');
    return response.data;
  },

  // Get user roles
  getUserRoles: async (userId: number): Promise<Role[]> => {
    const response = await api.get(`/User/${userId}/roles`);
    return response.data;
  },

  // Assign roles to user
  assignRoles: async (userId: number, roleIds: number[]): Promise<void> => {
    await api.post(`/User/${userId}/roles`, { roleIds });
  },
};
