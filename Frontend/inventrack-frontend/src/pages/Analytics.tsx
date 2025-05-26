import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/card';
import { Button } from '../components/ui/button';
import {
  TrendingUp,
  TrendingDown,
  BarChart3,
  PieChart,
  Calendar,
  Download,
  Filter,
  RefreshCw,
  DollarSign,
  Package,
  Users,
  ShoppingCart,
  AlertTriangle,
  Target,
  LineChart,
} from 'lucide-react';

interface AnalyticsData {
  salesTrend: { month: string; sales: number; orders: number }[];
  topProducts: { name: string; sales: number; units: number }[];
  customerSegments: { segment: string; count: number; revenue: number }[];
  inventoryMetrics: {
    totalValue: number;
    turnoverRate: number;
    stockAccuracy: number;
    lowStockItems: number;
  };
  kpis: {
    grossMargin: number;
    customerAcquisitionCost: number;
    averageOrderValue: number;
    customerLifetimeValue: number;
  };
}

const Analytics: React.FC = () => {
  const [data, setData] = useState<AnalyticsData>({
    salesTrend: [
      { month: 'Jan', sales: 45000, orders: 120 },
      { month: 'Feb', sales: 52000, orders: 135 },
      { month: 'Mar', sales: 48000, orders: 128 },
      { month: 'Apr', sales: 61000, orders: 156 },
      { month: 'May', sales: 55000, orders: 142 },
      { month: 'Jun', sales: 67000, orders: 178 },
    ],
    topProducts: [
      { name: 'Wireless Headphones', sales: 15420, units: 234 },
      { name: 'Smartphone Case', sales: 8950, units: 445 },
      { name: 'Bluetooth Speaker', sales: 12300, units: 189 },
      { name: 'USB Cable', sales: 5670, units: 567 },
      { name: 'Power Bank', sales: 9800, units: 156 },
    ],
    customerSegments: [
      { segment: 'Premium', count: 156, revenue: 89500 },
      { segment: 'Regular', count: 423, revenue: 156700 },
      { segment: 'New', count: 234, revenue: 45600 },
      { segment: 'Inactive', count: 89, revenue: 12300 },
    ],
    inventoryMetrics: {
      totalValue: 456789,
      turnoverRate: 4.2,
      stockAccuracy: 98.5,
      lowStockItems: 23,
    },
    kpis: {
      grossMargin: 42.5,
      customerAcquisitionCost: 45.60,
      averageOrderValue: 156.78,
      customerLifetimeValue: 1245.50,
    },
  });

  const [isLoading, setIsLoading] = useState(false);

  const refreshData = async () => {
    setIsLoading(true);
    // Simulate API call
    setTimeout(() => {
      setIsLoading(false);
    }, 1000);
  };

  const exportReport = () => {
    // Simulate report export
    const csvContent = "data:text/csv;charset=utf-8,"
      + "Month,Sales,Orders\n"
      + data.salesTrend.map(row => `${row.month},${row.sales},${row.orders}`).join("\n");

    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "analytics_report.csv");
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  const MetricCard: React.FC<{
    title: string;
    value: string | number;
    change?: number;
    icon: React.ComponentType<{ className?: string }>;
    format?: 'currency' | 'number' | 'percentage';
  }> = ({ title, value, change, icon: Icon, format = 'number' }) => {
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
      <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
        <CardContent className="p-6">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-stone-600 dark:text-slate-400">{title}</p>
              <p className="text-2xl font-bold text-stone-800 dark:text-slate-100">
                {formatValue(value)}
              </p>
              {change !== undefined && (
                <div className="flex items-center mt-1">
                  {change > 0 ? (
                    <TrendingUp className="w-4 h-4 text-green-500 mr-1" />
                  ) : (
                    <TrendingDown className="w-4 h-4 text-red-500 mr-1" />
                  )}
                  <span className={`text-sm ${change > 0 ? 'text-green-500' : 'text-red-500'}`}>
                    {Math.abs(change)}%
                  </span>
                </div>
              )}
            </div>
            <div className="p-3 bg-gradient-to-br from-red-500/10 to-red-600/10 rounded-xl">
              <Icon className="w-6 h-6 text-red-500" />
            </div>
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
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Analytics</h1>
          <p className="text-stone-600 dark:text-slate-400">Business intelligence and performance insights</p>
        </div>
        <div className="flex items-center space-x-3">
          <Button
            variant="outline"
            onClick={exportReport}
            className="border-stone-200 dark:border-slate-600 text-stone-700 dark:text-slate-300 hover:bg-stone-100 dark:hover:bg-slate-700 rounded-xl"
          >
            <Download className="w-4 h-4 mr-2" />
            Export
          </Button>
          <Button
            variant="outline"
            className="border-stone-200 dark:border-slate-600 text-stone-700 dark:text-slate-300 hover:bg-stone-100 dark:hover:bg-slate-700 rounded-xl"
          >
            <Filter className="w-4 h-4 mr-2" />
            Filter
          </Button>
          <Button
            onClick={refreshData}
            disabled={isLoading}
            className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105"
          >
            <RefreshCw className={`w-4 h-4 mr-2 ${isLoading ? 'animate-spin' : ''}`} />
            Refresh
          </Button>
        </div>
      </div>

      {/* KPI Cards */}
      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
        <MetricCard
          title="Gross Margin"
          value={data.kpis.grossMargin}
          change={5.2}
          icon={Target}
          format="percentage"
        />
        <MetricCard
          title="Avg Order Value"
          value={data.kpis.averageOrderValue}
          change={8.1}
          icon={DollarSign}
          format="currency"
        />
        <MetricCard
          title="Customer LTV"
          value={data.kpis.customerLifetimeValue}
          change={12.3}
          icon={Users}
          format="currency"
        />
        <MetricCard
          title="Inventory Turnover"
          value={data.inventoryMetrics.turnoverRate}
          change={-2.1}
          icon={Package}
          format="number"
        />
      </div>

      {/* Charts and Analytics */}
      <div className="grid gap-6 lg:grid-cols-2">
        {/* Sales Trend Chart */}
        <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
          <CardHeader className="bg-gradient-to-r from-green-50/80 to-emerald-50/80 dark:from-green-900/20 dark:to-emerald-900/20 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
            <CardTitle className="flex items-center space-x-2 text-stone-800 dark:text-slate-100">
              <TrendingUp className="w-5 h-5 text-green-500" />
              <span>Sales Trend</span>
            </CardTitle>
            <CardDescription className="text-stone-600 dark:text-slate-400">
              Monthly sales and order performance
            </CardDescription>
          </CardHeader>
          <CardContent className="p-6">
            <div className="space-y-4">
              {data.salesTrend.map((item, index) => (
                <div key={index} className="flex items-center justify-between p-3 bg-stone-50 dark:bg-slate-700/50 rounded-lg">
                  <div className="flex items-center space-x-3">
                    <div className="w-3 h-3 bg-green-500 rounded-full"></div>
                    <span className="font-medium text-stone-800 dark:text-slate-200">{item.month}</span>
                  </div>
                  <div className="text-right">
                    <div className="font-bold text-stone-800 dark:text-slate-200">
                      ${item.sales.toLocaleString()}
                    </div>
                    <div className="text-sm text-stone-600 dark:text-slate-400">
                      {item.orders} orders
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Top Products */}
        <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
          <CardHeader className="bg-gradient-to-r from-blue-50/80 to-cyan-50/80 dark:from-blue-900/20 dark:to-cyan-900/20 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
            <CardTitle className="flex items-center space-x-2 text-stone-800 dark:text-slate-100">
              <Package className="w-5 h-5 text-blue-500" />
              <span>Top Products</span>
            </CardTitle>
            <CardDescription className="text-stone-600 dark:text-slate-400">
              Best performing products by revenue
            </CardDescription>
          </CardHeader>
          <CardContent className="p-6">
            <div className="space-y-4">
              {data.topProducts.map((product, index) => (
                <div key={index} className="flex items-center justify-between p-3 bg-stone-50 dark:bg-slate-700/50 rounded-lg">
                  <div className="flex items-center space-x-3">
                    <div className="w-8 h-8 bg-blue-500 text-white rounded-lg flex items-center justify-center text-sm font-bold">
                      {index + 1}
                    </div>
                    <div>
                      <div className="font-medium text-stone-800 dark:text-slate-200">{product.name}</div>
                      <div className="text-sm text-stone-600 dark:text-slate-400">{product.units} units sold</div>
                    </div>
                  </div>
                  <div className="font-bold text-stone-800 dark:text-slate-200">
                    ${product.sales.toLocaleString()}
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Customer Segments and Inventory Metrics */}
      <div className="grid gap-6 lg:grid-cols-2">
        {/* Customer Segments */}
        <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
          <CardHeader className="bg-gradient-to-r from-purple-50/80 to-violet-50/80 dark:from-purple-900/20 dark:to-violet-900/20 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
            <CardTitle className="flex items-center space-x-2 text-stone-800 dark:text-slate-100">
              <Users className="w-5 h-5 text-purple-500" />
              <span>Customer Segments</span>
            </CardTitle>
            <CardDescription className="text-stone-600 dark:text-slate-400">
              Customer distribution and revenue by segment
            </CardDescription>
          </CardHeader>
          <CardContent className="p-6">
            <div className="space-y-4">
              {data.customerSegments.map((segment, index) => (
                <div key={index} className="flex items-center justify-between p-3 bg-stone-50 dark:bg-slate-700/50 rounded-lg">
                  <div className="flex items-center space-x-3">
                    <div className={`w-3 h-3 rounded-full ${
                      segment.segment === 'Premium' ? 'bg-yellow-500' :
                      segment.segment === 'Regular' ? 'bg-green-500' :
                      segment.segment === 'New' ? 'bg-blue-500' : 'bg-gray-500'
                    }`}></div>
                    <div>
                      <div className="font-medium text-stone-800 dark:text-slate-200">{segment.segment}</div>
                      <div className="text-sm text-stone-600 dark:text-slate-400">{segment.count} customers</div>
                    </div>
                  </div>
                  <div className="font-bold text-stone-800 dark:text-slate-200">
                    ${segment.revenue.toLocaleString()}
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Inventory Metrics */}
        <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
          <CardHeader className="bg-gradient-to-r from-orange-50/80 to-red-50/80 dark:from-orange-900/20 dark:to-red-900/20 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
            <CardTitle className="flex items-center space-x-2 text-stone-800 dark:text-slate-100">
              <BarChart3 className="w-5 h-5 text-orange-500" />
              <span>Inventory Health</span>
            </CardTitle>
            <CardDescription className="text-stone-600 dark:text-slate-400">
              Key inventory performance indicators
            </CardDescription>
          </CardHeader>
          <CardContent className="p-6">
            <div className="grid grid-cols-2 gap-4">
              <div className="p-4 bg-stone-50 dark:bg-slate-700/50 rounded-lg">
                <div className="text-2xl font-bold text-stone-800 dark:text-slate-200">
                  ${data.inventoryMetrics.totalValue.toLocaleString()}
                </div>
                <div className="text-sm text-stone-600 dark:text-slate-400">Total Value</div>
              </div>
              <div className="p-4 bg-stone-50 dark:bg-slate-700/50 rounded-lg">
                <div className="text-2xl font-bold text-stone-800 dark:text-slate-200">
                  {data.inventoryMetrics.turnoverRate}x
                </div>
                <div className="text-sm text-stone-600 dark:text-slate-400">Turnover Rate</div>
              </div>
              <div className="p-4 bg-stone-50 dark:bg-slate-700/50 rounded-lg">
                <div className="text-2xl font-bold text-green-600">
                  {data.inventoryMetrics.stockAccuracy}%
                </div>
                <div className="text-sm text-stone-600 dark:text-slate-400">Stock Accuracy</div>
              </div>
              <div className="p-4 bg-stone-50 dark:bg-slate-700/50 rounded-lg">
                <div className="text-2xl font-bold text-red-600">
                  {data.inventoryMetrics.lowStockItems}
                </div>
                <div className="text-sm text-stone-600 dark:text-slate-400">Low Stock Items</div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};

export default Analytics;
