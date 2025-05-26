import React, { useState, useEffect } from 'react';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '../ui/dialog';
import { useToast } from '../../hooks/use-toast';
import { productService } from '../../services/productService';
import type { Product, ProductCreateDto, ProductCategory } from '../../types/index';
import { Save, X, Package } from 'lucide-react';

interface ProductFormProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  product?: Product | null;
  mode: 'create' | 'edit';
}

const ProductForm: React.FC<ProductFormProps> = ({
  isOpen,
  onClose,
  onSuccess,
  product,
  mode
}) => {
  const { toast } = useToast();
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState<ProductCategory[]>([]);

  const [formData, setFormData] = useState<ProductCreateDto>({
    SKU: '',
    Name: '',
    Description: '',
    CategoryId: 1,
    UnitOfMeasure: 'piece',
    Weight: 0,
    Dimensions: '',
    UnitCost: 0,
    ListPrice: 0,
    MinimumStockLevel: 0,
    ReorderQuantity: 0,
    LeadTimeInDays: 0,
    ImageUrl: '',
  });

  useEffect(() => {
    if (product && mode === 'edit') {
      setFormData({
        SKU: product.SKU,
        Name: product.Name,
        Description: product.Description || '',
        CategoryId: product.CategoryId,
        UnitOfMeasure: product.UnitOfMeasure,
        Weight: product.Weight || 0,
        Dimensions: product.Dimensions || '',
        UnitCost: product.UnitCost,
        ListPrice: product.ListPrice,
        MinimumStockLevel: product.MinimumStockLevel,
        ReorderQuantity: product.ReorderQuantity,
        LeadTimeInDays: product.LeadTimeInDays,
        ImageUrl: product.ImageUrl || '',
      });
    } else {
      // Reset form for create mode
      setFormData({
        SKU: '',
        Name: '',
        Description: '',
        CategoryId: 1,
        UnitOfMeasure: 'piece',
        Weight: 0,
        Dimensions: '',
        UnitCost: 0,
        ListPrice: 0,
        MinimumStockLevel: 0,
        ReorderQuantity: 0,
        LeadTimeInDays: 0,
        ImageUrl: '',
      });
    }
  }, [product, mode, isOpen]);

  const handleInputChange = (field: keyof ProductCreateDto, value: string | number) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      if (mode === 'create') {
        await productService.createProduct(formData);
        toast({
          title: "Success",
          description: "Product created successfully",
          variant: "success",
        });
      } else if (product) {
        await productService.updateProduct(product.ProductId, {
          ...formData,
          IsActive: product.IsActive,
        });
        toast({
          title: "Success",
          description: "Product updated successfully",
          variant: "success",
        });
      }

      onSuccess();
      onClose();
    } catch (error) {
      console.error('Error saving product:', error);
      toast({
        title: "Error",
        description: `Failed to ${mode} product`,
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
            <Package className="w-6 h-6 text-red-500" />
            <span>{mode === 'create' ? 'Add New Product' : 'Edit Product'}</span>
          </DialogTitle>
          <DialogDescription className="text-stone-600 dark:text-slate-400">
            {mode === 'create'
              ? 'Fill in the details to create a new product'
              : 'Update the product information below'
            }
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="space-y-2">
              <label className="text-sm font-medium text-stone-700 dark:text-slate-300">SKU *</label>
              <Input
                value={formData.SKU}
                onChange={(e) => handleInputChange('SKU', e.target.value)}
                placeholder="Enter SKU"
                className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium text-stone-700 dark:text-slate-300">Product Name *</label>
              <Input
                value={formData.Name}
                onChange={(e) => handleInputChange('Name', e.target.value)}
                placeholder="Enter product name"
                className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                required
              />
            </div>
          </div>

          <div className="space-y-2">
            <label className="text-sm font-medium text-stone-700 dark:text-slate-300">Description</label>
            <textarea
              className="w-full p-3 bg-white/80 dark:bg-slate-800/80 border border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300 resize-none"
              rows={3}
              value={formData.Description}
              onChange={(e) => handleInputChange('Description', e.target.value)}
              placeholder="Enter product description"
            />
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="space-y-2">
              <label className="text-sm font-medium">Unit of Measure *</label>
              <select
                className="w-full p-3 border border-sage-green rounded-md"
                value={formData.UnitOfMeasure}
                onChange={(e) => handleInputChange('UnitOfMeasure', e.target.value)}
                required
              >
                <option value="piece">Piece</option>
                <option value="kg">Kilogram</option>
                <option value="liter">Liter</option>
                <option value="meter">Meter</option>
                <option value="box">Box</option>
              </select>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium">Weight (kg)</label>
              <Input
                type="number"
                step="0.01"
                value={formData.Weight}
                onChange={(e) => handleInputChange('Weight', parseFloat(e.target.value) || 0)}
                placeholder="0.00"
              />
            </div>
          </div>

          <div className="space-y-2">
            <label className="text-sm font-medium">Dimensions</label>
            <Input
              value={formData.Dimensions}
              onChange={(e) => handleInputChange('Dimensions', e.target.value)}
              placeholder="e.g., 20x15x8 cm"
            />
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="space-y-2">
              <label className="text-sm font-medium">Unit Cost *</label>
              <Input
                type="number"
                step="0.01"
                value={formData.UnitCost}
                onChange={(e) => handleInputChange('UnitCost', parseFloat(e.target.value) || 0)}
                placeholder="0.00"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium">List Price *</label>
              <Input
                type="number"
                step="0.01"
                value={formData.ListPrice}
                onChange={(e) => handleInputChange('ListPrice', parseFloat(e.target.value) || 0)}
                placeholder="0.00"
                required
              />
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="space-y-2">
              <label className="text-sm font-medium">Minimum Stock Level *</label>
              <Input
                type="number"
                value={formData.MinimumStockLevel}
                onChange={(e) => handleInputChange('MinimumStockLevel', parseInt(e.target.value) || 0)}
                placeholder="0"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium">Reorder Quantity *</label>
              <Input
                type="number"
                value={formData.ReorderQuantity}
                onChange={(e) => handleInputChange('ReorderQuantity', parseInt(e.target.value) || 0)}
                placeholder="0"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium">Lead Time (days) *</label>
              <Input
                type="number"
                value={formData.LeadTimeInDays}
                onChange={(e) => handleInputChange('LeadTimeInDays', parseInt(e.target.value) || 0)}
                placeholder="0"
                required
              />
            </div>
          </div>

          <div className="space-y-2">
            <label className="text-sm font-medium">Image URL</label>
            <Input
              value={formData.ImageUrl}
              onChange={(e) => handleInputChange('ImageUrl', e.target.value)}
              placeholder="https://example.com/image.jpg"
            />
          </div>

          <DialogFooter className="pt-6 border-t border-stone-200/60 dark:border-slate-600/60">
            <Button
              type="button"
              variant="outline"
              onClick={onClose}
              className="border-stone-200 dark:border-slate-600 text-stone-700 dark:text-slate-300 hover:bg-stone-100 dark:hover:bg-slate-700 rounded-xl transition-all duration-300"
            >
              <X className="w-4 h-4 mr-2" />
              Cancel
            </Button>
            <Button
              type="submit"
              disabled={loading}
              className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
            >
              <Save className="w-4 h-4 mr-2" />
              {loading ? 'Saving...' : mode === 'create' ? 'Create Product' : 'Update Product'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default ProductForm;
