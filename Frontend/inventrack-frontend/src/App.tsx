import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from './contexts/ThemeContext';
import { AuthProvider } from './contexts/AuthContext';
import { Toaster } from './components/ui/toaster';
import ProtectedRoute from './components/auth/ProtectedRoute';
import Layout from './components/layout/Layout';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import Products from './pages/Products';
import Customers from './pages/Customers';
import Orders from './pages/Orders';
import Suppliers from './pages/Suppliers';
import Warehouse from './pages/Warehouse';
import Users from './pages/Users';
import Analytics from './pages/Analytics';
import Integrations from './pages/Integrations';
import Settings from './pages/Settings';
import Profile from './pages/Profile';
import ApiTest from './pages/ApiTest';

function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <Router>
          <Routes>
            {/* Public routes */}
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />

            {/* Protected routes */}
            <Route path="/*" element={
              <ProtectedRoute>
                <Layout>
                  <Routes>
                    <Route path="/" element={<Dashboard />} />
                    <Route path="/products" element={<Products />} />
                    <Route path="/customers" element={<Customers />} />
                    <Route path="/orders" element={<Orders />} />
                    <Route path="/suppliers" element={<Suppliers />} />
                    <Route path="/warehouse" element={<Warehouse />} />
                    <Route path="/users" element={<Users />} />
                    <Route path="/analytics" element={<Analytics />} />
                    <Route path="/integrations" element={<Integrations />} />
                    <Route path="/settings" element={<Settings />} />
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/api-test" element={<ApiTest />} />
                  </Routes>
                </Layout>
              </ProtectedRoute>
            } />
          </Routes>
          <Toaster />
        </Router>
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;
