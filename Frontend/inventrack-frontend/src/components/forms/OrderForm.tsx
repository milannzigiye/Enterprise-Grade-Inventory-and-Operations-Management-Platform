import React, { useState, useEffect } from 'react';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from '../ui/dialog';
import { useToast } from '../../hooks/use-toast';
import type { Order, OrderCreateDto, Customer, Product } from '../../types/index';
import { orderService } from '../../services/orderService';
import { customerService } from '../../services/customerService';
import { productService } from '../../services/productService';
import { X, Save, ShoppingCart, Plus, Trash2 } from 'lucide-react';

interface OrderFormProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  order?: Order | null;
  mode: 'create' | 'edit';
}

interface OrderItem {
  productId: number;
  quantity: number;
  unitPrice: number;
  productName?: string;
}

const OrderForm: React.FC<OrderFormProps> = ({
  isOpen,
  onClose,
  onSuccess,
  order,
  mode
}) => {
  const [formData, setFormData] = useState<{
    customerId: number;
    orderItems: OrderItem[];
    shippingAddress: string;
    billingAddress: string;
    paymentMethod: string;
  }>({
    customerId: 0,
    orderItems: [{ productId: 0, quantity: 1, unitPrice: 0 }],
    shippingAddress: '',
    billingAddress: '',
    paymentMethod: 'Credit Card'
  });
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(false);
  const { toast } = useToast();

  useEffect(() => {
    if (isOpen) {
      loadCustomers();
      loadProducts();
    }
  }, [isOpen]);

  useEffect(() => {
    if (order && mode === 'edit') {
      setFormData({
        customerId: order.customerId,
        orderItems: order.orderItems?.map(item => ({
          productId: item.productId,
          quantity: item.quantity,
          unitPrice: item.unitPrice,
          productName: item.product?.name
        })) || [{ productId: 0, quantity: 1, unitPrice: 0 }],
        shippingAddress: order.shippingAddress || '',
        billingAddress: order.billingAddress || '',
        paymentMethod: order.paymentMethod || 'Credit Card'
      });
    } else {
      setFormData({
        customerId: 0,
        orderItems: [{ productId: 0, quantity: 1, unitPrice: 0 }],
        shippingAddress: '',
        billingAddress: '',
        paymentMethod: 'Credit Card'
      });
    }
  }, [order, mode, isOpen]);

  const loadCustomers = async () => {
    try {
      const data = await customerService.getAllCustomers();
      setCustomers(data);
    } catch (error) {
      console.error('Error loading customers:', error);
    }
  };

  const loadProducts = async () => {
    try {
      const data = await productService.getAllProducts();
      setProducts(data);
    } catch (error) {
      console.error('Error loading products:', error);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'customerId' ? parseInt(value) : value
    }));
  };

  const handleOrderItemChange = (index: number, field: keyof OrderItem, value: string | number) => {
    setFormData(prev => ({
      ...prev,
      orderItems: prev.orderItems.map((item, i) =>
        i === index
          ? {
              ...item,
              [field]: field === 'productId' ? parseInt(value as string) :
                      field === 'quantity' ? parseInt(value as string) :
                      field === 'unitPrice' ? parseFloat(value as string) : value
            }
          : item
      )
    }));
  };

  const addOrderItem = () => {
    setFormData(prev => ({
      ...prev,
      orderItems: [...prev.orderItems, { productId: 0, quantity: 1, unitPrice: 0 }]
    }));
  };

  const removeOrderItem = (index: number) => {
    if (formData.orderItems.length > 1) {
      setFormData(prev => ({
        ...prev,
        orderItems: prev.orderItems.filter((_, i) => i !== index)
      }));
    }
  };

  const calculateTotal = () => {
    return formData.orderItems.reduce((total, item) => total + (item.quantity * item.unitPrice), 0);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.customerId || formData.orderItems.some(item => !item.productId || item.quantity <= 0)) {
      toast({
        title: "Validation Error",
        description: "Please select a customer and ensure all order items are valid",
        variant: "destructive",
      });
      return;
    }

    try {
      setLoading(true);

      const orderData: OrderCreateDto = {
        customerId: formData.customerId,
        orderItems: formData.orderItems.map(item => ({
          productId: item.productId,
          quantity: item.quantity,
          unitPrice: item.unitPrice
        })),
        shippingAddress: formData.shippingAddress,
        billingAddress: formData.billingAddress,
        paymentMethod: formData.paymentMethod
      };

      if (mode === 'create') {
        await orderService.createOrder(orderData);
        toast({
          title: "Success",
          description: "Order created successfully",
          variant: "success",
        });
      } else if (order) {
        await orderService.updateOrder(order.orderId, {
          ...order,
          ...orderData
        });
        toast({
          title: "Success",
          description: "Order updated successfully",
          variant: "success",
        });
      }

      onSuccess();
      onClose();
    } catch (error) {
      console.error('Error saving order:', error);
      toast({
        title: "Error",
        description: `Failed to ${mode} order`,
        variant: "destructive",
      });
    } finally {
      setLoading(false);
    }
  };

  const getProductName = (productId: number) => {
    const product = products.find(p => p.productId === productId);
    return product?.name || 'Select Product';
  };

  const getProductPrice = (productId: number) => {
    const product = products.find(p => p.productId === productId);
    return product?.listPrice || 0;
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="max-w-4xl max-h-[90vh] overflow-y-auto backdrop-blur-xl bg-white/95 dark:bg-slate-800/95 border border-stone-200/60 dark:border-slate-600/60 rounded-3xl shadow-2xl">
        <DialogHeader>
          <DialogTitle className="flex items-center space-x-2 text-stone-800 dark:text-slate-100 text-xl font-bold">
            <ShoppingCart className="w-6 h-6 text-red-500" />
            <span>{mode === 'create' ? 'Create New Order' : 'Edit Order'}</span>
          </DialogTitle>
          <DialogDescription className="text-stone-600 dark:text-slate-400">
            {mode === 'create'
              ? 'Create a new order for a customer'
              : 'Update order information'
            }
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {/* Customer Selection */}
            <Card className="md:col-span-2 bg-gradient-to-br from-stone-50 to-gray-50 dark:from-slate-700 dark:to-slate-600 border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-lg">
              <CardHeader className="pb-4">
                <CardTitle className="text-lg text-stone-800 dark:text-slate-100 font-semibold">Customer Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                    Customer *
                  </label>
                  <select
                    name="customerId"
                    value={formData.customerId}
                    onChange={handleInputChange}
                    className="w-full bg-white/80 dark:bg-slate-800/80 border border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300 p-3"
                    required
                  >
                    <option value={0}>Select a customer</option>
                    {customers.map(customer => (
                      <option key={customer.customerId} value={customer.customerId}>
                        {customer.firstName} {customer.lastName} - {customer.email}
                      </option>
                    ))}
                  </select>
                </div>
              </CardContent>
            </Card>

            {/* Order Items */}
            <Card className="md:col-span-2">
              <CardHeader className="pb-4">
                <CardTitle className="text-lg text-neutral-dark flex items-center justify-between">
                  Order Items
                  <Button
                    type="button"
                    variant="outline"
                    size="sm"
                    onClick={addOrderItem}
                    className="border-primary text-primary hover:bg-primary hover:text-white"
                  >
                    <Plus className="w-4 h-4 mr-2" />
                    Add Item
                  </Button>
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                {formData.orderItems.map((item, index) => (
                  <div key={index} className="grid grid-cols-1 md:grid-cols-5 gap-4 p-4 border border-neutral-light rounded-lg">
                    <div className="md:col-span-2">
                      <label className="block text-sm font-medium text-neutral-dark mb-2">
                        Product *
                      </label>
                      <select
                        value={item.productId}
                        onChange={(e) => {
                          const productId = parseInt(e.target.value);
                          handleOrderItemChange(index, 'productId', productId);
                          if (productId > 0) {
                            handleOrderItemChange(index, 'unitPrice', getProductPrice(productId));
                          }
                        }}
                        className="w-full input"
                        required
                      >
                        <option value={0}>Select a product</option>
                        {products.map(product => (
                          <option key={product.productId} value={product.productId}>
                            {product.name} - ${product.listPrice}
                          </option>
                        ))}
                      </select>
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-neutral-dark mb-2">
                        Quantity *
                      </label>
                      <Input
                        type="number"
                        min="1"
                        value={item.quantity}
                        onChange={(e) => handleOrderItemChange(index, 'quantity', e.target.value)}
                        className="input"
                        required
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-neutral-dark mb-2">
                        Unit Price *
                      </label>
                      <Input
                        type="number"
                        step="0.01"
                        min="0"
                        value={item.unitPrice}
                        onChange={(e) => handleOrderItemChange(index, 'unitPrice', e.target.value)}
                        className="input"
                        required
                      />
                    </div>
                    <div className="flex items-end">
                      <Button
                        type="button"
                        variant="outline"
                        size="sm"
                        onClick={() => removeOrderItem(index)}
                        disabled={formData.orderItems.length === 1}
                        className="border-error text-error hover:bg-error hover:text-white"
                      >
                        <Trash2 className="w-4 h-4" />
                      </Button>
                    </div>
                  </div>
                ))}

                <div className="text-right pt-4 border-t border-neutral-light">
                  <div className="text-lg font-semibold text-neutral-dark">
                    Total: ${calculateTotal().toFixed(2)}
                  </div>
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
              {loading ? 'Saving...' : mode === 'create' ? 'Create Order' : 'Update Order'}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default OrderForm;
