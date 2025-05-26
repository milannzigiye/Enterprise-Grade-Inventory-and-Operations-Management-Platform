import api from './api';

export interface User {
  userId: number;
  username: string;
  email: string;
  phoneNumber?: string;
  isActive: boolean;
  createdDate: string;
  lastLoginDate?: string;
  twoFactorEnabled: boolean;
  profile?: UserProfile;
  userRoles?: UserRole[];
}

export interface UserProfile {
  profileId: number;
  firstName?: string;
  lastName?: string;
  profileImage?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  preferredLanguage?: string;
  timeZone?: string;
}

export interface UserRole {
  userRoleId: number;
  userId: number;
  roleId: number;
  role: Role;
}

export interface Role {
  roleId: number;
  roleName: string;
  description?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
  twoFactorCode?: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string;
  profile?: {
    firstName?: string;
    lastName?: string;
    address?: string;
    city?: string;
    state?: string;
    zipCode?: string;
    country?: string;
  };
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
  requiresTwoFactor: boolean;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface TwoFactorSetup {
  secretKey: string;
  qrCodeUrl: string;
  manualEntryKey: string;
}

export const authService = {
  // Authentication
  login: async (credentials: LoginRequest): Promise<AuthResponse> => {
    // Mock authentication for demo purposes
    console.log('Login attempt with:', { email: credentials.email, password: credentials.password });

    if (credentials.email === 'admin@inventrackpro.com' && credentials.password === 'Admin123!') {
      return {
        token: 'mock-jwt-token-admin',
        refreshToken: 'mock-refresh-token',
        expiresAt: new Date(Date.now() + 3600000).toISOString(),
        user: {
          userId: 1,
          username: 'admin',
          email: 'admin@inventrackpro.com',
          phoneNumber: '+1-555-0001',
          isActive: true,
          createdDate: new Date().toISOString(),
          lastLoginDate: new Date().toISOString(),
          twoFactorEnabled: false,
          profile: {
            profileId: 1,
            firstName: 'Admin',
            lastName: 'User',
            address: '123 Admin Street',
            city: 'Admin City',
            state: 'AC',
            zipCode: '12345',
            country: 'USA'
          }
        },
        requiresTwoFactor: false
      };
    } else if (credentials.email === 'demo@inventrackpro.com' && credentials.password === 'Demo123!') {
      return {
        token: 'mock-jwt-token-demo',
        refreshToken: 'mock-refresh-token',
        expiresAt: new Date(Date.now() + 3600000).toISOString(),
        user: {
          userId: 2,
          username: 'demo',
          email: 'demo@inventrackpro.com',
          phoneNumber: '+1-555-0002',
          isActive: true,
          createdDate: new Date().toISOString(),
          lastLoginDate: new Date().toISOString(),
          twoFactorEnabled: false,
          profile: {
            profileId: 2,
            firstName: 'Demo',
            lastName: 'User',
            address: '456 Demo Avenue',
            city: 'Demo City',
            state: 'DC',
            zipCode: '67890',
            country: 'USA'
          }
        },
        requiresTwoFactor: false
      };
    } else {
      console.log('Invalid credentials provided:', {
        email: credentials.email,
        password: credentials.password,
        expectedAdmin: 'admin@inventrackpro.com / Admin123!',
        expectedDemo: 'demo@inventrackpro.com / Demo123!'
      });
      throw new Error('Invalid credentials');
    }

    // Fallback to API call
    // const response = await api.post('/Auth/login', credentials);
    // return response.data;
  },

  register: async (userData: RegisterRequest): Promise<AuthResponse> => {
    const response = await api.post('/Auth/register', userData);
    return response.data;
  },

  logout: async (): Promise<void> => {
    await api.post('/Auth/logout');
  },

  refreshToken: async (refreshToken: string): Promise<AuthResponse> => {
    const response = await api.post('/Auth/refresh-token', { refreshToken });
    return response.data;
  },

  changePassword: async (passwordData: ChangePasswordRequest): Promise<void> => {
    await api.post('/Auth/change-password', passwordData);
  },

  // User Profile
  getCurrentUser: async (): Promise<User> => {
    // Mock user data for demo
    const token = localStorage.getItem('authToken');
    if (token === 'mock-jwt-token-admin') {
      return {
        userId: 1,
        username: 'admin',
        email: 'admin@inventrackpro.com',
        phoneNumber: '+1-555-0001',
        isActive: true,
        createdDate: new Date().toISOString(),
        lastLoginDate: new Date().toISOString(),
        twoFactorEnabled: false,
        profile: {
          profileId: 1,
          firstName: 'Admin',
          lastName: 'User',
          address: '123 Admin Street',
          city: 'Admin City',
          state: 'AC',
          zipCode: '12345',
          country: 'USA'
        }
      };
    } else if (token === 'mock-jwt-token-demo') {
      return {
        userId: 2,
        username: 'demo',
        email: 'demo@inventrackpro.com',
        phoneNumber: '+1-555-0002',
        isActive: true,
        createdDate: new Date().toISOString(),
        lastLoginDate: new Date().toISOString(),
        twoFactorEnabled: false,
        profile: {
          profileId: 2,
          firstName: 'Demo',
          lastName: 'User',
          address: '456 Demo Avenue',
          city: 'Demo City',
          state: 'DC',
          zipCode: '67890',
          country: 'USA'
        }
      };
    }
    throw new Error('User not found');

    // Fallback to API call
    // const response = await api.get('/User/profile');
    // return response.data;
  },

  updateProfile: async (profile: Partial<UserProfile>): Promise<User> => {
    const response = await api.put('/User/profile', profile);
    return response.data;
  },

  // Two-Factor Authentication
  getTwoFactorSetup: async (): Promise<TwoFactorSetup> => {
    const response = await api.get('/TwoFactor/setup');
    return response.data;
  },

  enableTwoFactor: async (code: string): Promise<void> => {
    await api.post('/TwoFactor/enable', { code });
  },

  disableTwoFactor: async (): Promise<void> => {
    await api.post('/TwoFactor/disable');
  },

  verifyTwoFactorCode: async (code: string): Promise<void> => {
    await api.post('/TwoFactor/verify', { code });
  },
};
