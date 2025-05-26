import React, { useEffect, useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/card';
import { Button } from '../components/ui/button';
import {
  TrendingUp,
  TrendingDown,
  Package,
  Users,
  ShoppingCart,
  DollarSign,
  AlertTriangle,
  RefreshCw,
} from 'lucide-react';

interface DashboardStats {
  totalProducts: number;
  totalCustomers: number;
  totalOrders: number;
  totalRevenue: number;
  lowStockItems: number;
  pendingOrders: number;
  revenueGrowth: number;
  orderGrowth: number;
}

const Dashboard: React.FC = () => {
  const [stats, setStats] = useState<DashboardStats>({
    totalProducts: 1247,
    totalCustomers: 892,
    totalOrders: 156,
    totalRevenue: 45678.90,
    lowStockItems: 23,
    pendingOrders: 12,
    revenueGrowth: 12.5,
    orderGrowth: 8.3,
  });

  const [isLoading, setIsLoading] = useState(false);

  const refreshData = async () => {
    setIsLoading(true);
    // Simulate API call
    setTimeout(() => {
      setIsLoading(false);
    }, 1000);
  };

  const StatCard: React.FC<{
    title: string;
    value: string | number;
    description: string;
    icon: React.ComponentType<{ className?: string }>;
    trend?: number;
    format?: 'currency' | 'number' | 'percentage';
  }> = ({ title, value, description, icon: Icon, trend, format = 'number' }) => {
    const formatValue = (val: string | number) => {
      if (format === 'currency') {
        return `$${Number(val).toLocaleString('en-US', { minimumFractionDigits: 2 })}`;
      }
      if (format === 'percentage') {
        return `${val}%`;
      }
      return Number(val).toLocaleString();
    };

    return (
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">{title}</CardTitle>
          <Icon className="h-4 w-4 text-sage-green" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold text-dark-charcoal">
            {formatValue(value)}
          </div>
          <div className="flex items-center space-x-2 text-xs text-gray-600">
            <span>{description}</span>
            {trend !== undefined && (
              <div className="flex items-center">
                {trend > 0 ? (
                  <TrendingUp className="h-3 w-3 text-green-500" />
                ) : (
                  <TrendingDown className="h-3 w-3 text-red-500" />
                )}
                <span className={trend > 0 ? 'text-green-500' : 'text-red-500'}>
                  {Math.abs(trend)}%
                </span>
              </div>
            )}
          </div>
        </CardContent>
      </Card>
    );
  };

  return (
    <div className="space-y-8">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">
            Dashboard
          </h1>
          <p className="text-stone-600 dark:text-slate-400">
            Welcome back! Here's what's happening with your inventory.
          </p>
        </div>
        <Button
          onClick={refreshData}
          disabled={isLoading}
          variant="outline"
          className="border-stone-200 dark:border-slate-600 text-stone-700 dark:text-slate-300 hover:bg-stone-100 dark:hover:bg-slate-700 rounded-xl"
        >
          <RefreshCw className={`w-4 h-4 mr-2 ${isLoading ? 'animate-spin' : ''}`} />
          Refresh
        </Button>
      </div>

      {/* Stats Grid */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <StatCard
          title="Total Revenue"
          value={stats.totalRevenue}
          description="from last month"
          icon={DollarSign}
          trend={stats.revenueGrowth}
          format="currency"
        />
        <StatCard
          title="Total Orders"
          value={stats.totalOrders}
          description="this month"
          icon={ShoppingCart}
          trend={stats.orderGrowth}
        />
        <StatCard
          title="Total Products"
          value={stats.totalProducts}
          description="in inventory"
          icon={Package}
        />
        <StatCard
          title="Total Customers"
          value={stats.totalCustomers}
          description="registered"
          icon={Users}
        />
      </div>

      {/* Alerts and Quick Actions */}
      <div className="grid gap-4 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <AlertTriangle className="h-5 w-5 text-coral-red" />
              <span>Alerts</span>
            </CardTitle>
            <CardDescription>
              Items that need your attention
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex items-center justify-between p-3 bg-red-50 rounded-lg">
              <div>
                <p className="font-medium text-red-800">Low Stock Items</p>
                <p className="text-sm text-red-600">{stats.lowStockItems} products below minimum level</p>
              </div>
              <Button size="sm" variant="outline">
                View
              </Button>
            </div>
            <div className="flex items-center justify-between p-3 bg-yellow-50 rounded-lg">
              <div>
                <p className="font-medium text-yellow-800">Pending Orders</p>
                <p className="text-sm text-yellow-600">{stats.pendingOrders} orders awaiting processing</p>
              </div>
              <Button size="sm" variant="outline">
                Process
              </Button>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Quick Actions</CardTitle>
            <CardDescription>
              Common tasks and shortcuts
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-3">
            <Button className="w-full justify-start" variant="outline">
              <Package className="w-4 h-4 mr-2" />
              Add New Product
            </Button>
            <Button className="w-full justify-start" variant="outline">
              <ShoppingCart className="w-4 h-4 mr-2" />
              Create Order
            </Button>
            <Button className="w-full justify-start" variant="outline">
              <Users className="w-4 h-4 mr-2" />
              Add Customer
            </Button>
            <Button className="w-full justify-start" variant="outline">
              <TrendingUp className="w-4 h-4 mr-2" />
              View Reports
            </Button>
          </CardContent>
        </Card>
      </div>

      {/* Recent Activity */}
      <Card>
        <CardHeader>
          <CardTitle>Recent Activity</CardTitle>
          <CardDescription>
            Latest updates across your inventory system
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            {[
              { action: 'New order #1234 created', time: '2 minutes ago', type: 'order' },
              { action: 'Product "Wireless Headphones" stock updated', time: '15 minutes ago', type: 'inventory' },
              { action: 'Customer "John Doe" registered', time: '1 hour ago', type: 'customer' },
              { action: 'Supplier "TechCorp" payment processed', time: '2 hours ago', type: 'payment' },
            ].map((activity, index) => (
              <div key={index} className="flex items-center space-x-4 p-3 hover:bg-warm-beige/30 rounded-lg transition-colors">
                <div className="w-2 h-2 bg-coral-red rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm font-medium text-dark-charcoal">{activity.action}</p>
                  <p className="text-xs text-gray-500">{activity.time}</p>
                </div>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default Dashboard;
