import React, { useEffect, useState } from 'react';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/card';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '../components/ui/table';
import OrderForm from '../components/forms/OrderForm';
import { useToast } from '../hooks/use-toast';
import type { Order } from '../types/index';
import { orderService } from '../services/orderService';
import { Plus, Search, Filter, Edit, Eye, Package, Clock, CheckCircle, XCircle, Trash2, ShoppingCart } from 'lucide-react';

const Orders: React.FC = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredOrders, setFilteredOrders] = useState<Order[]>([]);
  const [showOrderForm, setShowOrderForm] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);
  const [formMode, setFormMode] = useState<'create' | 'edit'>('create');
  const { toast } = useToast();

  useEffect(() => {
    loadOrders();
  }, []);

  useEffect(() => {
    const filtered = orders.filter(
      (order) =>
        order.orderId.toString().includes(searchTerm) ||
        order.status.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredOrders(filtered);
  }, [orders, searchTerm]);

  const loadOrders = async () => {
    try {
      setLoading(true);
      const data = await orderService.getAllOrders();
      setOrders(data);
    } catch (error) {
      console.error('Error loading orders:', error);
      // Mock data for demo
      setOrders([
        {
          orderId: 1001,
          customerId: 1,
          orderDate: '2024-01-20T10:00:00Z',
          totalAmount: 159.98,
          status: 'Processing',
          shippingAddress: '123 Main St, New York, NY 10001',
          billingAddress: '123 Main St, New York, NY 10001',
          paymentMethod: 'Credit Card',
          customer: {
            customerId: 1,
            firstName: 'John',
            lastName: 'Doe',
            email: 'john.doe@email.com',
            registrationDate: '2024-01-15T10:00:00Z',
            isActive: true,
          },
        },
        {
          orderId: 1002,
          customerId: 2,
          orderDate: '2024-01-19T14:30:00Z',
          totalAmount: 89.99,
          status: 'Shipped',
          shippingAddress: '456 Oak Ave, Los Angeles, CA 90210',
          billingAddress: '456 Oak Ave, Los Angeles, CA 90210',
          paymentMethod: 'PayPal',
          customer: {
            customerId: 2,
            firstName: 'Jane',
            lastName: 'Smith',
            email: 'jane.smith@email.com',
            registrationDate: '2024-01-10T14:30:00Z',
            isActive: true,
          },
        },
        {
          orderId: 1003,
          customerId: 3,
          orderDate: '2024-01-18T09:15:00Z',
          totalAmount: 199.99,
          status: 'Delivered',
          shippingAddress: '789 Pine Rd, Chicago, IL 60601',
          billingAddress: '789 Pine Rd, Chicago, IL 60601',
          paymentMethod: 'Credit Card',
          customer: {
            customerId: 3,
            firstName: 'Bob',
            lastName: 'Johnson',
            email: 'bob.johnson@email.com',
            registrationDate: '2024-01-08T09:15:00Z',
            isActive: true,
          },
        },
        {
          orderId: 1004,
          customerId: 1,
          orderDate: '2024-01-17T16:45:00Z',
          totalAmount: 45.50,
          status: 'Cancelled',
          shippingAddress: '123 Main St, New York, NY 10001',
          billingAddress: '123 Main St, New York, NY 10001',
          paymentMethod: 'Credit Card',
          customer: {
            customerId: 1,
            firstName: 'John',
            lastName: 'Doe',
            email: 'john.doe@email.com',
            registrationDate: '2024-01-15T10:00:00Z',
            isActive: true,
          },
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteOrder = async (orderId: number) => {
    if (window.confirm('Are you sure you want to delete this order?')) {
      try {
        await orderService.deleteOrder(orderId);
        await loadOrders();
        toast({
          title: "Success",
          description: "Order deleted successfully",
          variant: "success",
        });
      } catch (error) {
        console.error('Error deleting order:', error);
        toast({
          title: "Error",
          description: "Failed to delete order",
          variant: "destructive",
        });
      }
    }
  };

  const handleCreateOrder = () => {
    setSelectedOrder(null);
    setFormMode('create');
    setShowOrderForm(true);
  };

  const handleEditOrder = (order: Order) => {
    setSelectedOrder(order);
    setFormMode('edit');
    setShowOrderForm(true);
  };

  const handleFormSuccess = () => {
    loadOrders();
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(amount);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const getStatusIcon = (status: string) => {
    switch (status.toLowerCase()) {
      case 'processing':
        return <Clock className="w-4 h-4" />;
      case 'shipped':
        return <Package className="w-4 h-4" />;
      case 'delivered':
        return <CheckCircle className="w-4 h-4" />;
      case 'cancelled':
        return <XCircle className="w-4 h-4" />;
      default:
        return <Clock className="w-4 h-4" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case 'processing':
        return 'badge-warning';
      case 'shipped':
        return 'badge-info';
      case 'delivered':
        return 'badge-success';
      case 'cancelled':
        return 'badge-error';
      default:
        return 'badge-info';
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64 fade-in">
        <div className="text-lg text-neutral-medium">Loading orders...</div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Orders</h1>
          <p className="text-stone-600 dark:text-slate-400">Track and manage customer orders and fulfillment</p>
        </div>
        <Button
          onClick={handleCreateOrder}
          className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105"
        >
          <Plus className="w-4 h-4 mr-2" />
          Create Order
        </Button>
      </div>

      {/* Search and Filters */}
      <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
        <CardContent className="p-6">
          <div className="flex items-center space-x-4">
            <div className="flex-1 relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-stone-500 dark:text-slate-400 w-4 h-4" />
              <Input
                type="text"
                placeholder="Search orders by ID or status..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-10 bg-white/50 dark:bg-slate-700/50 border-stone-200 dark:border-slate-600 focus:border-red-500 dark:focus:border-red-400 rounded-xl"
              />
            </div>
            <Button
              variant="outline"
              className="border-stone-200 dark:border-slate-600 text-stone-700 dark:text-slate-300 hover:bg-stone-100 dark:hover:bg-slate-700 rounded-xl"
            >
              <Filter className="w-4 h-4 mr-2" />
              Filters
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Orders Table */}
      <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
        <CardHeader className="bg-gradient-to-r from-stone-100/80 to-gray-100/80 dark:from-slate-700/80 dark:to-slate-600/80 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
          <div className="flex items-center justify-between">
            <div>
              <CardTitle className="text-stone-800 dark:text-slate-100 flex items-center space-x-2">
                <ShoppingCart className="w-5 h-5 text-red-500" />
                <span>Orders ({filteredOrders.length})</span>
              </CardTitle>
              <CardDescription className="text-stone-600 dark:text-slate-400">
                Track and manage all customer orders and their status
              </CardDescription>
            </div>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="hover:bg-transparent">
                <TableHead className="text-neutral-dark">Order ID</TableHead>
                <TableHead className="text-neutral-dark">Customer</TableHead>
                <TableHead className="text-neutral-dark">Order Date</TableHead>
                <TableHead className="text-neutral-dark">Total Amount</TableHead>
                <TableHead className="text-neutral-dark">Status</TableHead>
                <TableHead className="text-neutral-dark">Payment Method</TableHead>
                <TableHead className="text-right text-neutral-dark">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredOrders.map((order) => (
                <TableRow key={order.orderId} className="hover:bg-neutral-light/50 transition-colors">
                  <TableCell className="font-mono font-medium text-neutral-dark">
                    #{order.orderId}
                  </TableCell>
                  <TableCell>
                    <div>
                      <div className="font-medium text-neutral-dark">
                        {order.customer?.firstName} {order.customer?.lastName}
                      </div>
                      <div className="text-sm text-neutral-medium">{order.customer?.email}</div>
                    </div>
                  </TableCell>
                  <TableCell className="text-neutral-dark">{formatDate(order.orderDate)}</TableCell>
                  <TableCell className="font-medium text-neutral-dark">{formatCurrency(order.totalAmount)}</TableCell>
                  <TableCell>
                    <span className={`badge ${getStatusColor(order.status)} inline-flex items-center space-x-1`}>
                      {getStatusIcon(order.status)}
                      <span>{order.status}</span>
                    </span>
                  </TableCell>
                  <TableCell className="text-neutral-dark">{order.paymentMethod}</TableCell>
                  <TableCell className="text-right">
                    <div className="flex items-center justify-end space-x-2">
                      <Button variant="ghost" size="sm" className="text-neutral-medium hover:text-primary">
                        <Eye className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-neutral-medium hover:text-primary"
                        onClick={() => handleEditOrder(order)}
                      >
                        <Edit className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-neutral-medium hover:text-error"
                        onClick={() => handleDeleteOrder(order.orderId)}
                      >
                        <Trash2 className="w-4 h-4" />
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      {/* Order Form Modal */}
      <OrderForm
        isOpen={showOrderForm}
        onClose={() => setShowOrderForm(false)}
        onSuccess={handleFormSuccess}
        order={selectedOrder}
        mode={formMode}
      />
    </div>
  );
};

export default Orders;
