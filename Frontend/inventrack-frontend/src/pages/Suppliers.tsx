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
import SupplierForm from '../components/forms/SupplierForm';
import { useToast } from '../hooks/use-toast';
import type { Supplier } from '../types/index';
import { supplierService } from '../services/supplierService';
import { Plus, Search, Filter, Edit, Trash2, Eye, Mail, Phone, Truck, Building } from 'lucide-react';

const Suppliers: React.FC = () => {
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredSuppliers, setFilteredSuppliers] = useState<Supplier[]>([]);
  const [showSupplierForm, setShowSupplierForm] = useState(false);
  const [selectedSupplier, setSelectedSupplier] = useState<Supplier | null>(null);
  const [formMode, setFormMode] = useState<'create' | 'edit'>('create');
  const { toast } = useToast();

  useEffect(() => {
    loadSuppliers();
  }, []);

  useEffect(() => {
    // Filter suppliers based on search term
    const filtered = suppliers.filter(
      (supplier) =>
        supplier.supplierName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        supplier.contactPerson?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        supplier.email?.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredSuppliers(filtered);
  }, [suppliers, searchTerm]);

  const loadSuppliers = async () => {
    try {
      setLoading(true);
      const data = await supplierService.getAllSuppliers();
      setSuppliers(data);
    } catch (error) {
      console.error('Error loading suppliers:', error);
      // Mock data for demo
      setSuppliers([
        {
          supplierId: 1,
          supplierName: 'TechCorp Solutions',
          contactPerson: 'John Smith',
          email: 'john@techcorp.com',
          phoneNumber: '+1 (555) 123-4567',
          address: '123 Tech Street',
          city: 'San Francisco',
          state: 'CA',
          zipCode: '94105',
          country: 'USA',
          isActive: true,
          registrationDate: '2024-01-15T10:00:00Z',
        },
        {
          supplierId: 2,
          supplierName: 'Global Electronics Ltd',
          contactPerson: 'Sarah Johnson',
          email: 'sarah@globalelectronics.com',
          phoneNumber: '+1 (555) 987-6543',
          address: '456 Electronics Ave',
          city: 'Austin',
          state: 'TX',
          zipCode: '73301',
          country: 'USA',
          isActive: true,
          registrationDate: '2024-01-10T14:30:00Z',
        },
        {
          supplierId: 3,
          supplierName: 'Premium Parts Inc',
          contactPerson: 'Mike Wilson',
          email: 'mike@premiumparts.com',
          phoneNumber: '+1 (555) 456-7890',
          address: '789 Industrial Blvd',
          city: 'Chicago',
          state: 'IL',
          zipCode: '60601',
          country: 'USA',
          isActive: false,
          registrationDate: '2024-01-05T09:15:00Z',
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteSupplier = async (supplierId: number) => {
    if (window.confirm('Are you sure you want to delete this supplier?')) {
      try {
        await supplierService.deleteSupplier(supplierId);
        await loadSuppliers();
        toast({
          title: "Success",
          description: "Supplier deleted successfully",
          variant: "success",
        });
      } catch (error) {
        console.error('Error deleting supplier:', error);
        toast({
          title: "Error",
          description: "Failed to delete supplier",
          variant: "destructive",
        });
      }
    }
  };

  const handleCreateSupplier = () => {
    setSelectedSupplier(null);
    setFormMode('create');
    setShowSupplierForm(true);
  };

  const handleEditSupplier = (supplier: Supplier) => {
    setSelectedSupplier(supplier);
    setFormMode('edit');
    setShowSupplierForm(true);
  };

  const handleFormSuccess = () => {
    loadSuppliers();
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64 fade-in">
        <div className="text-lg text-neutral-medium">Loading suppliers...</div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Suppliers</h1>
          <p className="text-stone-600 dark:text-slate-400">Manage your supplier relationships and procurement</p>
        </div>
        <Button
          onClick={handleCreateSupplier}
          className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105"
        >
          <Plus className="w-4 h-4 mr-2" />
          Add Supplier
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
                placeholder="Search suppliers by name, contact, or email..."
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

      {/* Suppliers Table */}
      <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
        <CardHeader className="bg-gradient-to-r from-stone-100/80 to-gray-100/80 dark:from-slate-700/80 dark:to-slate-600/80 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
          <div className="flex items-center justify-between">
            <div>
              <CardTitle className="text-stone-800 dark:text-slate-100 flex items-center space-x-2">
                <Truck className="w-5 h-5 text-red-500" />
                <span>Suppliers ({filteredSuppliers.length})</span>
              </CardTitle>
              <CardDescription className="text-stone-600 dark:text-slate-400">
                Manage your supplier network and procurement relationships
              </CardDescription>
            </div>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="hover:bg-transparent border-b border-stone-200/60 dark:border-slate-600/60">
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Supplier</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Contact</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Location</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Registration Date</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Status</TableHead>
                <TableHead className="text-right text-stone-700 dark:text-slate-300 font-semibold">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredSuppliers.map((supplier) => (
                <TableRow key={supplier.supplierId} className="hover:bg-stone-50/80 dark:hover:bg-slate-700/50 transition-all duration-200 border-b border-stone-100/60 dark:border-slate-700/60">
                  <TableCell>
                    <div className="flex items-center space-x-3">
                      <div className="w-10 h-10 bg-gradient-to-br from-red-500 to-red-600 rounded-lg flex items-center justify-center shadow-lg">
                        <Building className="w-5 h-5 text-white" />
                      </div>
                      <div>
                        <div className="font-medium text-stone-800 dark:text-slate-100">{supplier.supplierName}</div>
                        <div className="text-sm text-stone-500 dark:text-slate-400">ID: {supplier.supplierId}</div>
                      </div>
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="space-y-1">
                      {supplier.contactPerson && (
                        <div className="text-sm text-stone-800 dark:text-slate-100 font-medium">{supplier.contactPerson}</div>
                      )}
                      {supplier.email && (
                        <div className="flex items-center space-x-2">
                          <Mail className="w-4 h-4 text-stone-500 dark:text-slate-400" />
                          <span className="text-sm text-stone-700 dark:text-slate-300">{supplier.email}</span>
                        </div>
                      )}
                      {supplier.phoneNumber && (
                        <div className="flex items-center space-x-2">
                          <Phone className="w-4 h-4 text-stone-500 dark:text-slate-400" />
                          <span className="text-sm text-stone-700 dark:text-slate-300">{supplier.phoneNumber}</span>
                        </div>
                      )}
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="text-sm">
                      <div className="text-stone-800 dark:text-slate-100">{supplier.city}, {supplier.state}</div>
                      <div className="text-stone-500 dark:text-slate-400">{supplier.country}</div>
                    </div>
                  </TableCell>
                  <TableCell className="text-stone-700 dark:text-slate-300">{formatDate(supplier.registrationDate)}</TableCell>
                  <TableCell>
                    <span className={`badge ${
                      supplier.isActive ? 'badge-success' : 'badge-error'
                    }`}>
                      {supplier.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </TableCell>
                  <TableCell className="text-right">
                    <div className="flex items-center justify-end space-x-2">
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-stone-500 hover:text-blue-600 dark:text-slate-400 dark:hover:text-blue-400 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded-lg transition-all duration-200"
                      >
                        <Eye className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-stone-500 hover:text-amber-600 dark:text-slate-400 dark:hover:text-amber-400 hover:bg-amber-50 dark:hover:bg-amber-900/20 rounded-lg transition-all duration-200"
                        onClick={() => handleEditSupplier(supplier)}
                      >
                        <Edit className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-stone-500 hover:text-red-600 dark:text-slate-400 dark:hover:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition-all duration-200"
                        onClick={() => handleDeleteSupplier(supplier.supplierId)}
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

      {/* Supplier Form Modal */}
      <SupplierForm
        isOpen={showSupplierForm}
        onClose={() => setShowSupplierForm(false)}
        onSuccess={handleFormSuccess}
        supplier={selectedSupplier}
        mode={formMode}
      />
    </div>
  );
};

export default Suppliers;
