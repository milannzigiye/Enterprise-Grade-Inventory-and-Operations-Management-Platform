import React, { useState } from 'react';
import { Button } from '../components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/card';
import { productService } from '../services/productService';
import { customerService } from '../services/customerService';
import { orderService } from '../services/orderService';
import { supplierService } from '../services/supplierService';
import { useToast } from '../hooks/use-toast';
import { CheckCircle, XCircle, Loader2 } from 'lucide-react';

interface TestResult {
  name: string;
  status: 'pending' | 'success' | 'error';
  message?: string;
}

const ApiTest: React.FC = () => {
  const [tests, setTests] = useState<TestResult[]>([]);
  const [isRunning, setIsRunning] = useState(false);
  const { toast } = useToast();

  const updateTest = (name: string, status: 'success' | 'error', message?: string) => {
    setTests(prev => prev.map(test => 
      test.name === name ? { ...test, status, message } : test
    ));
  };

  const runApiTests = async () => {
    setIsRunning(true);
    const testList: TestResult[] = [
      { name: 'Products - Get All', status: 'pending' },
      { name: 'Customers - Get All', status: 'pending' },
      { name: 'Orders - Get All', status: 'pending' },
      { name: 'Suppliers - Get All', status: 'pending' },
      { name: 'Products - Create', status: 'pending' },
      { name: 'Customers - Create', status: 'pending' },
    ];
    
    setTests(testList);

    // Test Products - Get All
    try {
      const products = await productService.getAllProducts();
      updateTest('Products - Get All', 'success', `Retrieved ${products.length} products`);
    } catch (error) {
      updateTest('Products - Get All', 'error', `Error: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }

    // Test Customers - Get All
    try {
      const customers = await customerService.getAllCustomers();
      updateTest('Customers - Get All', 'success', `Retrieved ${customers.length} customers`);
    } catch (error) {
      updateTest('Customers - Get All', 'error', `Error: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }

    // Test Orders - Get All
    try {
      const orders = await orderService.getAllOrders();
      updateTest('Orders - Get All', 'success', `Retrieved ${orders.length} orders`);
    } catch (error) {
      updateTest('Orders - Get All', 'error', `Error: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }

    // Test Suppliers - Get All
    try {
      const suppliers = await supplierService.getAllSuppliers();
      updateTest('Suppliers - Get All', 'success', `Retrieved ${suppliers.length} suppliers`);
    } catch (error) {
      updateTest('Suppliers - Get All', 'error', `Error: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }

    // Test Products - Create
    try {
      const newProduct = await productService.createProduct({
        sku: `TEST-${Date.now()}`,
        name: 'Test Product',
        description: 'Test product for API testing',
        categoryId: 1,
        unitOfMeasure: 'piece',
        weight: 1.0,
        dimensions: '10x10x10 cm',
        unitCost: 10.00,
        listPrice: 20.00,
        minimumStockLevel: 5,
        reorderQuantity: 25,
        leadTimeInDays: 7,
        imageUrl: 'https://via.placeholder.com/150'
      });
      updateTest('Products - Create', 'success', `Created product with ID: ${newProduct.productId}`);
    } catch (error) {
      updateTest('Products - Create', 'error', `Error: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }

    // Test Customers - Create
    try {
      const newCustomer = await customerService.createCustomer({
        firstName: 'Test',
        lastName: 'Customer',
        email: `test${Date.now()}@example.com`,
        phoneNumber: '123-456-7890',
        address: '123 Test St',
        city: 'Test City',
        state: 'TS',
        zipCode: '12345',
        country: 'Test Country'
      });
      updateTest('Customers - Create', 'success', `Created customer with ID: ${newCustomer.customerId}`);
    } catch (error) {
      updateTest('Customers - Create', 'error', `Error: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }

    setIsRunning(false);
    
    const successCount = tests.filter(t => t.status === 'success').length;
    const totalTests = tests.length;
    
    toast({
      title: "API Tests Completed",
      description: `${successCount}/${totalTests} tests passed`,
      variant: successCount === totalTests ? "success" : "destructive",
    });
  };

  const getStatusIcon = (status: TestResult['status']) => {
    switch (status) {
      case 'pending':
        return <Loader2 className="w-4 h-4 animate-spin text-yellow-500" />;
      case 'success':
        return <CheckCircle className="w-4 h-4 text-green-500" />;
      case 'error':
        return <XCircle className="w-4 h-4 text-red-500" />;
    }
  };

  return (
    <div className="container mx-auto p-6">
      <Card className="max-w-4xl mx-auto">
        <CardHeader>
          <CardTitle className="flex items-center space-x-2">
            <span>API Connectivity Test</span>
          </CardTitle>
          <CardDescription>
            Test the connection between frontend and backend APIs
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-6">
          <Button 
            onClick={runApiTests} 
            disabled={isRunning}
            className="w-full"
          >
            {isRunning ? (
              <>
                <Loader2 className="w-4 h-4 mr-2 animate-spin" />
                Running Tests...
              </>
            ) : (
              'Run API Tests'
            )}
          </Button>

          {tests.length > 0 && (
            <div className="space-y-3">
              <h3 className="text-lg font-semibold">Test Results:</h3>
              {tests.map((test, index) => (
                <div key={index} className="flex items-center justify-between p-3 border rounded-lg">
                  <div className="flex items-center space-x-3">
                    {getStatusIcon(test.status)}
                    <span className="font-medium">{test.name}</span>
                  </div>
                  {test.message && (
                    <span className={`text-sm ${
                      test.status === 'success' ? 'text-green-600' : 
                      test.status === 'error' ? 'text-red-600' : 'text-gray-600'
                    }`}>
                      {test.message}
                    </span>
                  )}
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
};

export default ApiTest;
