import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { authService, type User, type LoginRequest, type RegisterRequest } from '../services/authService';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (credentials: LoginRequest) => Promise<{ success: boolean; requiresTwoFactor?: boolean; message?: string }>;
  register: (userData: RegisterRequest) => Promise<{ success: boolean; message?: string }>;
  logout: () => Promise<void>;
  refreshToken: () => Promise<boolean>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const isAuthenticated = !!user;

  useEffect(() => {
    const initializeAuth = async () => {
      const token = localStorage.getItem('authToken');
      if (token) {
        try {
          const currentUser = await authService.getCurrentUser();
          setUser(currentUser);
        } catch (error) {
          console.error('Failed to get current user:', error);
          localStorage.removeItem('authToken');
          localStorage.removeItem('refreshToken');
        }
      }
      setIsLoading(false);
    };

    initializeAuth();
  }, []);

  const login = async (credentials: LoginRequest) => {
    try {
      setIsLoading(true);
      const response = await authService.login(credentials);
      
      if (response.requiresTwoFactor) {
        return { success: false, requiresTwoFactor: true };
      }

      if (response.token) {
        localStorage.setItem('authToken', response.token);
        localStorage.setItem('refreshToken', response.refreshToken);
        setUser(response.user);
        return { success: true };
      }

      return { success: false, message: 'Login failed' };
    } catch (error: any) {
      console.error('Login error:', error);
      return { 
        success: false, 
        message: error.response?.data?.message || 'Login failed' 
      };
    } finally {
      setIsLoading(false);
    }
  };

  const register = async (userData: RegisterRequest) => {
    try {
      setIsLoading(true);
      const response = await authService.register(userData);
      
      if (response.token) {
        localStorage.setItem('authToken', response.token);
        localStorage.setItem('refreshToken', response.refreshToken);
        setUser(response.user);
        return { success: true };
      }

      return { success: false, message: 'Registration failed' };
    } catch (error: any) {
      console.error('Registration error:', error);
      return { 
        success: false, 
        message: error.response?.data?.message || 'Registration failed' 
      };
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    try {
      await authService.logout();
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      localStorage.removeItem('authToken');
      localStorage.removeItem('refreshToken');
      setUser(null);
    }
  };

  const refreshToken = async (): Promise<boolean> => {
    try {
      const refreshTokenValue = localStorage.getItem('refreshToken');
      if (!refreshTokenValue) return false;

      const response = await authService.refreshToken(refreshTokenValue);
      if (response.token) {
        localStorage.setItem('authToken', response.token);
        localStorage.setItem('refreshToken', response.refreshToken);
        setUser(response.user);
        return true;
      }
      return false;
    } catch (error) {
      console.error('Token refresh error:', error);
      localStorage.removeItem('authToken');
      localStorage.removeItem('refreshToken');
      setUser(null);
      return false;
    }
  };

  const value: AuthContextType = {
    user,
    isAuthenticated,
    isLoading,
    login,
    register,
    logout,
    refreshToken,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
