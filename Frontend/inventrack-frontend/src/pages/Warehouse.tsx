import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/card';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '../components/ui/table';
import WarehouseForm from '../components/forms/WarehouseForm';
import { useToast } from '../hooks/use-toast';
import { warehouseService } from '../services/warehouseService';
import type { Warehouse } from '../types/index';
import { Plus, Warehouse as WarehouseIcon, Search, Edit, Trash2, MapPin, Package, TrendingUp, Eye, Filter } from 'lucide-react';

const Warehouse: React.FC = () => {
  const { toast } = useToast();
  const [warehouses, setWarehouses] = useState<Warehouse[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [showWarehouseForm, setShowWarehouseForm] = useState(false);
  const [selectedWarehouse, setSelectedWarehouse] = useState<Warehouse | null>(null);
  const [formMode, setFormMode] = useState<'create' | 'edit'>('create');

  useEffect(() => {
    loadWarehouses();
  }, []);

  const loadWarehouses = async () => {
    try {
      setLoading(true);
      const data = await warehouseService.getAllWarehouses();
      setWarehouses(data);
    } catch (error) {
      console.error('Error loading warehouses:', error);
      // Mock data for demo
      setWarehouses([
        {
          warehouseId: 1,
          warehouseName: 'Main Warehouse',
          address: '123 Industrial Blvd',
          city: 'New York',
          state: 'NY',
          zipCode: '10001',
          country: 'USA',
          isActive: true,
          capacity: 10000,
          currentUtilization: 7500,
        },
        {
          warehouseId: 2,
          warehouseName: 'West Coast Distribution',
          address: '456 Logistics Ave',
          city: 'Los Angeles',
          state: 'CA',
          zipCode: '90210',
          country: 'USA',
          isActive: true,
          capacity: 15000,
          currentUtilization: 12000,
        },
        {
          warehouseId: 3,
          warehouseName: 'East Coast Hub',
          address: '789 Storage St',
          city: 'Miami',
          state: 'FL',
          zipCode: '33101',
          country: 'USA',
          isActive: false,
          capacity: 8000,
          currentUtilization: 0,
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteWarehouse = async (warehouseId: number) => {
    if (window.confirm('Are you sure you want to delete this warehouse?')) {
      try {
        await warehouseService.deleteWarehouse(warehouseId);
        await loadWarehouses();
        toast({
          title: "Success",
          description: "Warehouse deleted successfully",
          variant: "success",
        });
      } catch (error) {
        console.error('Error deleting warehouse:', error);
        toast({
          title: "Error",
          description: "Failed to delete warehouse",
          variant: "destructive",
        });
      }
    }
  };

  const handleCreateWarehouse = () => {
    setSelectedWarehouse(null);
    setFormMode('create');
    setShowWarehouseForm(true);
  };

  const handleEditWarehouse = (warehouse: Warehouse) => {
    setSelectedWarehouse(warehouse);
    setFormMode('edit');
    setShowWarehouseForm(true);
  };

  const handleFormSuccess = () => {
    loadWarehouses();
  };

  const filteredWarehouses = warehouses.filter(warehouse =>
    warehouse.warehouseName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    warehouse.city?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    warehouse.state?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const getUtilizationPercentage = (current?: number, capacity?: number) => {
    if (!current || !capacity) return 0;
    return Math.round((current / capacity) * 100);
  };

  const getUtilizationColor = (percentage: number) => {
    if (percentage >= 90) return 'bg-gradient-to-r from-red-100 to-red-200 dark:from-red-800 dark:to-red-700 text-red-800 dark:text-red-100 border border-red-200 dark:border-red-600';
    if (percentage >= 70) return 'bg-gradient-to-r from-yellow-100 to-yellow-200 dark:from-yellow-800 dark:to-yellow-700 text-yellow-800 dark:text-yellow-100 border border-yellow-200 dark:border-yellow-600';
    return 'bg-gradient-to-r from-green-100 to-green-200 dark:from-green-800 dark:to-green-700 text-green-800 dark:text-green-100 border border-green-200 dark:border-green-600';
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64 fade-in">
        <div className="text-lg text-stone-600 dark:text-slate-400">Loading warehouses...</div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Warehouses</h1>
          <p className="text-stone-600 dark:text-slate-400">Manage warehouse operations and inventory</p>
        </div>
        <Button
          onClick={handleCreateWarehouse}
          className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105"
        >
          <Plus className="w-4 h-4 mr-2" />
          Add Warehouse
        </Button>
      </div>

      {/* Search and Filters */}
      <Card className="bg-gradient-to-br from-stone-50/80 to-gray-50/80 dark:from-slate-700/80 dark:to-slate-600/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-lg">
        <CardContent className="p-6">
          <div className="flex items-center space-x-4">
            <div className="flex-1 relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-stone-400 dark:text-slate-500 w-4 h-4" />
              <Input
                type="text"
                placeholder="Search warehouses by name, city, or state..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-10 bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
              />
            </div>
            <Button variant="outline" className="border-stone-300 dark:border-slate-600 text-stone-700 dark:text-slate-300 hover:bg-stone-100 dark:hover:bg-slate-700 rounded-xl transition-all duration-300">
              <Filter className="w-4 h-4 mr-2" />
              Filters
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Warehouses Table */}
      <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
        <CardHeader className="bg-gradient-to-r from-stone-100/80 to-gray-100/80 dark:from-slate-700/80 dark:to-slate-600/80 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
          <div className="flex items-center justify-between">
            <div>
              <CardTitle className="text-stone-800 dark:text-slate-100 flex items-center space-x-2">
                <WarehouseIcon className="w-5 h-5 text-red-500" />
                <span>Warehouses ({filteredWarehouses.length})</span>
              </CardTitle>
              <CardDescription className="text-stone-600 dark:text-slate-400">
                Manage your warehouse locations and capacity
              </CardDescription>
            </div>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="hover:bg-transparent border-b border-stone-200/60 dark:border-slate-600/60">
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Warehouse</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Location</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Capacity</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Utilization</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Status</TableHead>
                <TableHead className="text-right text-stone-700 dark:text-slate-300 font-semibold">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredWarehouses.map((warehouse) => {
                const utilizationPercentage = getUtilizationPercentage(
                  warehouse.currentUtilization,
                  warehouse.capacity
                );
                return (
                  <TableRow key={warehouse.warehouseId} className="hover:bg-stone-50/50 dark:hover:bg-slate-700/50 transition-all duration-200 border-b border-stone-100/60 dark:border-slate-700/60">
                    <TableCell>
                      <div className="flex items-center space-x-3">
                        <div className="w-10 h-10 bg-gradient-to-br from-red-500 to-red-600 rounded-lg flex items-center justify-center shadow-lg">
                          <WarehouseIcon className="w-5 h-5 text-white" />
                        </div>
                        <div>
                          <div className="font-semibold text-stone-800 dark:text-slate-100">
                            {warehouse.warehouseName}
                          </div>
                          <div className="text-sm text-stone-500 dark:text-slate-400">
                            ID: {warehouse.warehouseId}
                          </div>
                        </div>
                      </div>
                    </TableCell>
                    <TableCell>
                      <div className="flex items-center space-x-2">
                        <MapPin className="w-4 h-4 text-stone-400 dark:text-slate-500" />
                        <div>
                          <div className="text-sm text-stone-600 dark:text-slate-300">{warehouse.address}</div>
                          <div className="text-xs text-stone-500 dark:text-slate-400">
                            {warehouse.city}, {warehouse.state} {warehouse.zipCode}
                          </div>
                        </div>
                      </div>
                    </TableCell>
                    <TableCell>
                      <div className="flex items-center space-x-2">
                        <Package className="w-4 h-4 text-stone-400 dark:text-slate-500" />
                        <span className="text-stone-600 dark:text-slate-300">{warehouse.capacity?.toLocaleString() || 'N/A'} units</span>
                      </div>
                    </TableCell>
                    <TableCell>
                      <div className="space-y-2">
                        <div className="flex items-center space-x-2">
                          <TrendingUp className="w-4 h-4 text-stone-400 dark:text-slate-500" />
                          <span className="text-sm text-stone-600 dark:text-slate-300">
                            {warehouse.currentUtilization?.toLocaleString() || 0} / {warehouse.capacity?.toLocaleString() || 0}
                          </span>
                        </div>
                        <div className="w-full bg-stone-200 dark:bg-slate-600 rounded-full h-2">
                          <div
                            className="bg-gradient-to-r from-red-500 to-red-600 h-2 rounded-full transition-all duration-300"
                            style={{ width: `${utilizationPercentage}%` }}
                          ></div>
                        </div>
                        <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getUtilizationColor(utilizationPercentage)}`}>
                          {utilizationPercentage}% utilized
                        </span>
                      </div>
                    </TableCell>
                    <TableCell>
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                        warehouse.isActive
                          ? 'bg-gradient-to-r from-green-100 to-green-200 dark:from-green-800 dark:to-green-700 text-green-800 dark:text-green-100 border border-green-200 dark:border-green-600'
                          : 'bg-gradient-to-r from-red-100 to-red-200 dark:from-red-800 dark:to-red-700 text-red-800 dark:text-red-100 border border-red-200 dark:border-red-600'
                      }`}>
                        {warehouse.isActive ? 'Active' : 'Inactive'}
                      </span>
                    </TableCell>
                    <TableCell className="text-right">
                      <div className="flex items-center justify-end space-x-2">
                        <Button variant="ghost" size="sm" className="text-stone-500 dark:text-slate-400 hover:text-blue-600 dark:hover:text-blue-400 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded-lg transition-all duration-200">
                          <Eye className="w-4 h-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="sm"
                          className="text-stone-500 dark:text-slate-400 hover:text-green-600 dark:hover:text-green-400 hover:bg-green-50 dark:hover:bg-green-900/20 rounded-lg transition-all duration-200"
                          onClick={() => handleEditWarehouse(warehouse)}
                        >
                          <Edit className="w-4 h-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="sm"
                          className="text-stone-500 dark:text-slate-400 hover:text-red-600 dark:hover:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition-all duration-200"
                          onClick={() => handleDeleteWarehouse(warehouse.warehouseId)}
                        >
                          <Trash2 className="w-4 h-4" />
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      {/* Warehouse Form Modal */}
      <WarehouseForm
        isOpen={showWarehouseForm}
        onClose={() => setShowWarehouseForm(false)}
        onSuccess={handleFormSuccess}
        warehouse={selectedWarehouse}
        mode={formMode}
      />
    </div>
  );
};

export default Warehouse;
