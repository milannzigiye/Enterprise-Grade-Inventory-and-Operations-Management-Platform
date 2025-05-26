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
import CustomerForm from '../components/forms/CustomerForm';
import { useToast } from '../hooks/use-toast';
import type { Customer } from '../types/index';
import { customerService } from '../services/customerService';
import { Plus, Search, Filter, Edit, Trash2, Eye, Mail, Phone, Users } from 'lucide-react';

const Customers: React.FC = () => {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredCustomers, setFilteredCustomers] = useState<Customer[]>([]);
  const [showCustomerForm, setShowCustomerForm] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState<Customer | null>(null);
  const [formMode, setFormMode] = useState<'create' | 'edit'>('create');
  const { toast } = useToast();

  useEffect(() => {
    loadCustomers();
  }, []);

  useEffect(() => {
    const filtered = customers.filter(
      (customer) =>
        customer.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        customer.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        customer.email.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredCustomers(filtered);
  }, [customers, searchTerm]);

  const loadCustomers = async () => {
    try {
      setLoading(true);
      const data = await customerService.getAllCustomers();
      setCustomers(data);
    } catch (error) {
      console.error('Error loading customers:', error);
      // Mock data for demo
      setCustomers([
        {
          customerId: 1,
          firstName: 'John',
          lastName: 'Doe',
          email: 'john.doe@email.com',
          phoneNumber: '+1 (555) 123-4567',
          registrationDate: '2024-01-15T10:00:00Z',
          isActive: true,
          address: '123 Main St',
          city: 'New York',
          state: 'NY',
          zipCode: '10001',
          country: 'USA',
        },
        {
          customerId: 2,
          firstName: 'Jane',
          lastName: 'Smith',
          email: 'jane.smith@email.com',
          phoneNumber: '+1 (555) 987-6543',
          registrationDate: '2024-01-10T14:30:00Z',
          isActive: true,
          address: '456 Oak Ave',
          city: 'Los Angeles',
          state: 'CA',
          zipCode: '90210',
          country: 'USA',
        },
        {
          customerId: 3,
          firstName: 'Bob',
          lastName: 'Johnson',
          email: 'bob.johnson@email.com',
          phoneNumber: '+1 (555) 456-7890',
          registrationDate: '2024-01-08T09:15:00Z',
          isActive: false,
          address: '789 Pine Rd',
          city: 'Chicago',
          state: 'IL',
          zipCode: '60601',
          country: 'USA',
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteCustomer = async (customerId: number) => {
    if (window.confirm('Are you sure you want to delete this customer?')) {
      try {
        await customerService.deleteCustomer(customerId);
        await loadCustomers();
        toast({
          title: "Success",
          description: "Customer deleted successfully",
          variant: "success",
        });
      } catch (error) {
        console.error('Error deleting customer:', error);
        toast({
          title: "Error",
          description: "Failed to delete customer",
          variant: "destructive",
        });
      }
    }
  };

  const handleCreateCustomer = () => {
    setSelectedCustomer(null);
    setFormMode('create');
    setShowCustomerForm(true);
  };

  const handleEditCustomer = (customer: Customer) => {
    setSelectedCustomer(customer);
    setFormMode('edit');
    setShowCustomerForm(true);
  };

  const handleFormSuccess = () => {
    loadCustomers();
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
        <div className="text-lg text-neutral-medium">Loading customers...</div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Customers</h1>
          <p className="text-stone-600 dark:text-slate-400">Manage your customer relationships</p>
        </div>
        <Button
          onClick={handleCreateCustomer}
          className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105"
        >
          <Plus className="w-4 h-4 mr-2" />
          Add Customer
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
                placeholder="Search customers by name or email..."
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

      {/* Customers Table */}
      <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
        <CardHeader className="bg-gradient-to-r from-stone-100/80 to-gray-100/80 dark:from-slate-700/80 dark:to-slate-600/80 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
          <div className="flex items-center justify-between">
            <div>
              <CardTitle className="text-stone-800 dark:text-slate-100 flex items-center space-x-2">
                <Users className="w-5 h-5 text-red-500" />
                <span>Customers ({filteredCustomers.length})</span>
              </CardTitle>
              <CardDescription className="text-stone-600 dark:text-slate-400">
                Manage your customer relationships and contact information
              </CardDescription>
            </div>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="hover:bg-transparent border-b border-stone-200/60 dark:border-slate-600/60">
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Customer</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Contact</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Location</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Registration Date</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Status</TableHead>
                <TableHead className="text-right text-stone-700 dark:text-slate-300 font-semibold">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredCustomers.map((customer) => (
                <TableRow key={customer.customerId} className="hover:bg-stone-50/80 dark:hover:bg-slate-700/50 transition-all duration-200 border-b border-stone-100/60 dark:border-slate-700/60">
                  <TableCell>
                    <div className="flex items-center space-x-3">
                      <div className="w-10 h-10 bg-gradient-to-br from-red-500 to-red-600 rounded-full flex items-center justify-center shadow-lg">
                        <span className="text-white font-medium text-sm">
                          {customer.firstName[0]}{customer.lastName[0]}
                        </span>
                      </div>
                      <div>
                        <div className="font-medium text-stone-800 dark:text-slate-100">
                          {customer.firstName} {customer.lastName}
                        </div>
                        <div className="text-sm text-stone-500 dark:text-slate-400">ID: {customer.customerId}</div>
                      </div>
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="space-y-1">
                      <div className="flex items-center space-x-2">
                        <Mail className="w-4 h-4 text-stone-500 dark:text-slate-400" />
                        <span className="text-sm text-stone-700 dark:text-slate-300">{customer.email}</span>
                      </div>
                      {customer.phoneNumber && (
                        <div className="flex items-center space-x-2">
                          <Phone className="w-4 h-4 text-stone-500 dark:text-slate-400" />
                          <span className="text-sm text-stone-700 dark:text-slate-300">{customer.phoneNumber}</span>
                        </div>
                      )}
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="text-sm">
                      <div className="text-stone-800 dark:text-slate-100">{customer.city}, {customer.state}</div>
                      <div className="text-stone-500 dark:text-slate-400">{customer.country}</div>
                    </div>
                  </TableCell>
                  <TableCell className="text-stone-700 dark:text-slate-300">{formatDate(customer.registrationDate)}</TableCell>
                  <TableCell>
                    <span className={`badge ${
                      customer.isActive ? 'badge-success' : 'badge-error'
                    }`}>
                      {customer.isActive ? 'Active' : 'Inactive'}
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
                        onClick={() => handleEditCustomer(customer)}
                      >
                        <Edit className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-stone-500 hover:text-red-600 dark:text-slate-400 dark:hover:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition-all duration-200"
                        onClick={() => handleDeleteCustomer(customer.customerId)}
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

      {/* Customer Form Modal */}
      <CustomerForm
        isOpen={showCustomerForm}
        onClose={() => setShowCustomerForm(false)}
        onSuccess={handleFormSuccess}
        customer={selectedCustomer}
        mode={formMode}
      />
    </div>
  );
};

export default Customers;
