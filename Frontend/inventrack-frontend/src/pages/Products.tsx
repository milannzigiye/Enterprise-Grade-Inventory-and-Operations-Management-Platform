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
import ProductForm from '../components/forms/ProductForm';
import { useToast } from '../hooks/use-toast';
import type { Product } from '../types/index';
import { productService } from '../services/productService';
import { Plus, Search, Filter, Edit, Trash2, Eye, Package } from 'lucide-react';

const Products: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredProducts, setFilteredProducts] = useState<Product[]>([]);
  const [showProductForm, setShowProductForm] = useState(false);
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);
  const [formMode, setFormMode] = useState<'create' | 'edit'>('create');
  const { toast } = useToast();

  useEffect(() => {
    loadProducts();
  }, []);

  useEffect(() => {
    // Filter products based on search term
    const filtered = products.filter(
      (product) =>
        product.Name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        product.SKU.toLowerCase().includes(searchTerm.toLowerCase()) ||
        product.Description?.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredProducts(filtered);
  }, [products, searchTerm]);

  const loadProducts = async () => {
    try {
      setLoading(true);
      const data = await productService.getAllProducts();
      setProducts(data);
    } catch (error) {
      console.error('Error loading products:', error);
      // For demo purposes, use mock data
      setProducts([
        {
          ProductId: 1,
          SKU: 'WH-001',
          Name: 'Wireless Headphones',
          Description: 'Premium wireless headphones with noise cancellation',
          CategoryId: 1,
          UnitOfMeasure: 'piece',
          Weight: 0.3,
          Dimensions: '20x15x8 cm',
          UnitCost: 45.00,
          ListPrice: 89.99,
          MinimumStockLevel: 10,
          ReorderQuantity: 50,
          LeadTimeInDays: 7,
          IsActive: true,
          CreatedDate: '2024-01-15T10:00:00Z',
          ImageUrl: 'https://via.placeholder.com/150',
        },
        {
          ProductId: 2,
          SKU: 'SM-002',
          Name: 'Smartphone Case',
          Description: 'Protective case for smartphones',
          CategoryId: 1,
          UnitOfMeasure: 'piece',
          Weight: 0.1,
          Dimensions: '15x8x1 cm',
          UnitCost: 5.00,
          ListPrice: 19.99,
          MinimumStockLevel: 25,
          ReorderQuantity: 100,
          LeadTimeInDays: 3,
          IsActive: true,
          CreatedDate: '2024-01-10T14:30:00Z',
          ImageUrl: 'https://via.placeholder.com/150',
        },
        {
          ProductId: 3,
          SKU: 'TB-003',
          Name: 'Bluetooth Speaker',
          Description: 'Portable Bluetooth speaker with excellent sound quality',
          CategoryId: 1,
          UnitOfMeasure: 'piece',
          Weight: 0.5,
          Dimensions: '12x12x6 cm',
          UnitCost: 25.00,
          ListPrice: 59.99,
          MinimumStockLevel: 15,
          ReorderQuantity: 30,
          LeadTimeInDays: 5,
          IsActive: true,
          CreatedDate: '2024-01-08T09:15:00Z',
          ImageUrl: 'https://via.placeholder.com/150',
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteProduct = async (productId: number) => {
    if (window.confirm('Are you sure you want to delete this product?')) {
      try {
        await productService.deleteProduct(productId);
        await loadProducts();
        toast({
          title: "Success",
          description: "Product deleted successfully",
          variant: "success",
        });
      } catch (error) {
        console.error('Error deleting product:', error);
        toast({
          title: "Error",
          description: "Failed to delete product",
          variant: "destructive",
        });
      }
    }
  };

  const handleCreateProduct = () => {
    setSelectedProduct(null);
    setFormMode('create');
    setShowProductForm(true);
  };

  const handleEditProduct = (product: Product) => {
    setSelectedProduct(product);
    setFormMode('edit');
    setShowProductForm(true);
  };

  const handleFormSuccess = () => {
    loadProducts();
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(amount);
  };

  const getStockStatus = (product: Product) => {
    // For demo, we'll simulate stock levels
    const currentStock = Math.floor(Math.random() * 100);
    if (currentStock <= product.MinimumStockLevel) {
      return { status: 'Low Stock', color: 'text-red-600 bg-red-50', stock: currentStock };
    } else if (currentStock <= product.MinimumStockLevel * 2) {
      return { status: 'Medium Stock', color: 'text-yellow-600 bg-yellow-50', stock: currentStock };
    } else {
      return { status: 'In Stock', color: 'text-green-600 bg-green-50', stock: currentStock };
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64 fade-in">
        <div className="text-lg text-neutral-medium">Loading products...</div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Products</h1>
          <p className="text-stone-600 dark:text-slate-400">Manage your product inventory and catalog</p>
        </div>
        <Button
          onClick={handleCreateProduct}
          className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105"
        >
          <Plus className="w-4 h-4 mr-2" />
          Add Product
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
                placeholder="Search products by name, SKU, or description..."
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

      {/* Products Table */}
      <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
        <CardHeader className="bg-gradient-to-r from-stone-100/80 to-gray-100/80 dark:from-slate-700/80 dark:to-slate-600/80 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
          <div className="flex items-center justify-between">
            <div>
              <CardTitle className="text-stone-800 dark:text-slate-100 flex items-center space-x-2">
                <Package className="w-5 h-5 text-red-500" />
                <span>Products ({filteredProducts.length})</span>
              </CardTitle>
              <CardDescription className="text-stone-600 dark:text-slate-400">
                Manage your product catalog and inventory levels
              </CardDescription>
            </div>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="hover:bg-transparent border-b border-stone-200/60 dark:border-slate-600/60">
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Product</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">SKU</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Category</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Stock Status</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Unit Cost</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">List Price</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Status</TableHead>
                <TableHead className="text-right text-stone-700 dark:text-slate-300 font-semibold">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredProducts.map((product) => {
                const stockInfo = getStockStatus(product);
                const badgeClass = {
                  'Low Stock': 'badge-error',
                  'Medium Stock': 'badge-warning',
                  'In Stock': 'badge-success'
                }[stockInfo.status] || 'badge-info';

                return (
                  <TableRow key={product.ProductId} className="hover:bg-neutral-light/50 transition-colors">
                    <TableCell>
                      <div className="flex items-center space-x-4">
                        <img
                          src={product.ImageUrl || 'https://via.placeholder.com/40'}
                          alt={product.Name}
                          className="w-10 h-10 rounded-lg object-cover border border-neutral-light"
                        />
                        <div>
                          <div className="font-medium text-neutral-dark">{product.Name}</div>
                          <div className="text-sm text-neutral-medium">{product.Description}</div>
                        </div>
                      </div>
                    </TableCell>
                    <TableCell className="font-mono text-sm text-neutral-dark">{product.SKU}</TableCell>
                    <TableCell className="text-neutral-medium">Electronics</TableCell>
                    <TableCell>
                      <span className={`badge ${badgeClass}`}>
                        {stockInfo.status} ({stockInfo.stock})
                      </span>
                    </TableCell>
                    <TableCell className="text-neutral-dark">{formatCurrency(product.UnitCost)}</TableCell>
                    <TableCell className="font-medium text-neutral-dark">{formatCurrency(product.ListPrice)}</TableCell>
                    <TableCell>
                      <span className={`badge ${product.IsActive ? 'badge-success' : 'badge-error'}`}>
                        {product.IsActive ? 'Active' : 'Inactive'}
                      </span>
                    </TableCell>
                    <TableCell className="text-right">
                      <div className="flex items-center justify-end space-x-2">
                        <Button variant="ghost" size="sm" className="text-neutral-medium hover:text-primary">
                          <Eye className="w-4 h-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="sm"
                          className="text-neutral-medium hover:text-primary"
                          onClick={() => handleEditProduct(product)}
                        >
                          <Edit className="w-4 h-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="sm"
                          className="text-neutral-medium hover:text-error"
                          onClick={() => handleDeleteProduct(product.ProductId)}
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

      {/* Product Form Modal */}
      <ProductForm
        isOpen={showProductForm}
        onClose={() => setShowProductForm(false)}
        onSuccess={handleFormSuccess}
        product={selectedProduct}
        mode={formMode}
      />
    </div>
  );
};

export default Products;