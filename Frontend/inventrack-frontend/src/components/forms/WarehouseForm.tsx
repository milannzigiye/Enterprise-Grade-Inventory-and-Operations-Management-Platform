import React, { useState, useEffect } from 'react';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from '../ui/dialog';
import { useToast } from '../../hooks/use-toast';
import type { Warehouse } from '../../types/index';
import { warehouseService } from '../../services/warehouseService';
import { X, Save, Warehouse as WarehouseIcon } from 'lucide-react';

interface WarehouseCreateDto {
  warehouseName: string;
  location: string;
  capacity?: number;
  currentStock?: number;
  managerName?: string;
  contactNumber?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
}

interface WarehouseFormProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  warehouse?: Warehouse | null;
  mode: 'create' | 'edit';
}

const WarehouseForm: React.FC<WarehouseFormProps> = ({
  isOpen,
  onClose,
  onSuccess,
  warehouse,
  mode
}) => {
  const [formData, setFormData] = useState<WarehouseCreateDto>({
    warehouseName: '',
    location: '',
    capacity: 0,
    currentStock: 0,
    managerName: '',
    contactNumber: '',
    address: '',
    city: '',
    state: '',
    zipCode: '',
    country: 'USA'
  });
  const [loading, setLoading] = useState(false);
  const { toast } = useToast();

  useEffect(() => {
    if (warehouse && mode === 'edit') {
      setFormData({
        warehouseName: warehouse.warehouseName,
        location: warehouse.location,
        capacity: warehouse.capacity || 0,
        currentStock: warehouse.currentStock || 0,
        managerName: warehouse.managerName || '',
        contactNumber: warehouse.contactNumber || '',
        address: warehouse.address || '',
        city: warehouse.city || '',
        state: warehouse.state || '',
        zipCode: warehouse.zipCode || '',
        country: warehouse.country || 'USA'
      });
    } else {
      setFormData({
        warehouseName: '',
        location: '',
        capacity: 0,
        currentStock: 0,
        managerName: '',
        contactNumber: '',
        address: '',
        city: '',
        state: '',
        zipCode: '',
        country: 'USA'
      });
    }
  }, [warehouse, mode, isOpen]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'capacity' || name === 'currentStock' ? parseInt(value) || 0 : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.warehouseName.trim() || !formData.location.trim()) {
      toast({
        title: "Validation Error",
        description: "Please enter warehouse name and location",
        variant: "destructive",
      });
      return;
    }

    try {
      setLoading(true);
      
      if (mode === 'create') {
        await warehouseService.createWarehouse(formData);
        toast({
          title: "Success",
          description: "Warehouse created successfully",
          variant: "success",
        });
      } else if (warehouse) {
        await warehouseService.updateWarehouse(warehouse.warehouseId, {
          ...warehouse,
          ...formData
        });
        toast({
          title: "Success",
          description: "Warehouse updated successfully",
          variant: "success",
        });
      }
      
      onSuccess();
      onClose();
    } catch (error) {
      console.error('Error saving warehouse:', error);
      toast({
        title: "Error",
        description: `Failed to ${mode} warehouse`,
        variant: "destructive",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto backdrop-blur-xl bg-white/95 dark:bg-slate-800/95 border border-stone-200/60 dark:border-slate-600/60 rounded-3xl shadow-2xl">
        <DialogHeader>
          <DialogTitle className="flex items-center space-x-2 text-stone-800 dark:text-slate-100 text-xl font-bold">
            <WarehouseIcon className="w-6 h-6 text-red-500" />
            <span>{mode === 'create' ? 'Add New Warehouse' : 'Edit Warehouse'}</span>
          </DialogTitle>
          <DialogDescription className="text-stone-600 dark:text-slate-400">
            {mode === 'create' 
              ? 'Create a new warehouse location for inventory storage'
              : 'Update warehouse information'
            }
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Basic Information */}
            <Card className="md:col-span-2 bg-gradient-to-br from-stone-50 to-gray-50 dark:from-slate-700 dark:to-slate-600 border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-lg">
              <CardHeader className="pb-4">
                <CardTitle className="text-lg text-stone-800 dark:text-slate-100 font-semibold">Warehouse Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Warehouse Name *
                    </label>
                    <Input
                      name="warehouseName"
                      value={formData.warehouseName}
                      onChange={handleInputChange}
                      placeholder="Enter warehouse name"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                      required
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Location *
                    </label>
                    <Input
                      name="location"
                      value={formData.location}
                      onChange={handleInputChange}
                      placeholder="Enter location"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                      required
                    />
                  </div>
                </div>
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Capacity
                    </label>
                    <Input
                      name="capacity"
                      type="number"
                      min="0"
                      value={formData.capacity}
                      onChange={handleInputChange}
                      placeholder="Enter capacity"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Current Stock
                    </label>
                    <Input
                      name="currentStock"
                      type="number"
                      min="0"
                      value={formData.currentStock}
                      onChange={handleInputChange}
                      placeholder="Enter current stock"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                    />
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* Contact Information */}
            <Card className="md:col-span-2 bg-gradient-to-br from-stone-50 to-gray-50 dark:from-slate-700 dark:to-slate-600 border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-lg">
              <CardHeader className="pb-4">
                <CardTitle className="text-lg text-stone-800 dark:text-slate-100 font-semibold">Contact Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Manager Name
                    </label>
                    <Input
                      name="managerName"
                      value={formData.managerName}
                      onChange={handleInputChange}
                      placeholder="Enter manager name"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Contact Number
                    </label>
                    <Input
                      name="contactNumber"
                      value={formData.contactNumber}
                      onChange={handleInputChange}
                      placeholder="Enter contact number"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                    />
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* Address Information */}
            <Card className="md:col-span-2 bg-gradient-to-br from-stone-50 to-gray-50 dark:from-slate-700 dark:to-slate-600 border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-lg">
              <CardHeader className="pb-4">
                <CardTitle className="text-lg text-stone-800 dark:text-slate-100 font-semibold">Address Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                    Street Address
                  </label>
                  <Input
                    name="address"
                    value={formData.address}
                    onChange={handleInputChange}
                    placeholder="Enter street address"
                    className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                  />
                </div>
                
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      City
                    </label>
                    <Input
                      name="city"
                      value={formData.city}
                      onChange={handleInputChange}
                      placeholder="Enter city"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      State
                    </label>
                    <Input
                      name="state"
                      value={formData.state}
                      onChange={handleInputChange}
                      placeholder="Enter state"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      ZIP Code
                    </label>
                    <Input
                      name="zipCode"
                      value={formData.zipCode}
                      onChange={handleInputChange}
                      placeholder="Enter ZIP code"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                    />
                  </div>
                </div>
                
                <div>
                  <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                    Country
                  </label>
                  <Input
                    name="country"
                    value={formData.country}
                    onChange={handleInputChange}
                    placeholder="Enter country"
                    className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                  />
                </div>
              </CardContent>
            </Card>
          </div>

          {/* Form Actions */}
          <div className="flex items-center justify-end space-x-4 pt-6 border-t border-stone-200/60 dark:border-slate-600/60">
            <Button
              type="button"
              variant="outline"
              onClick={onClose}
              className="px-6 py-3 border-stone-300 dark:border-slate-600 text-stone-700 dark:text-slate-300 hover:bg-stone-100 dark:hover:bg-slate-700 rounded-xl transition-all duration-300"
            >
              <X className="w-4 h-4 mr-2" />
              Cancel
            </Button>
            <Button
              type="submit"
              disabled={loading}
              className="px-6 py-3 bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105"
            >
              <Save className="w-4 h-4 mr-2" />
              {loading ? 'Saving...' : mode === 'create' ? 'Create Warehouse' : 'Update Warehouse'}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default WarehouseForm;
