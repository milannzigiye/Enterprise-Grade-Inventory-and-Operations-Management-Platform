import React from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/card';
import { Button } from '../components/ui/button';
import { Plus, Zap, CheckCircle, XCircle } from 'lucide-react';

const Integrations: React.FC = () => {
  const integrations = [
    {
      name: 'Shopify',
      description: 'E-commerce platform integration',
      status: 'connected',
      icon: '🛍️',
    },
    {
      name: 'QuickBooks',
      description: 'Accounting software integration',
      status: 'disconnected',
      icon: '📊',
    },
    {
      name: 'Amazon',
      description: 'Marketplace integration',
      status: 'connected',
      icon: '📦',
    },
    {
      name: 'Stripe',
      description: 'Payment processing',
      status: 'connected',
      icon: '💳',
    },
  ];

  return (
    <div className="space-y-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Integrations</h1>
          <p className="text-stone-600 dark:text-slate-400">Connect with external services and platforms</p>
        </div>
        <Button className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105">
          <Plus className="w-4 h-4 mr-2" />
          Add Integration
        </Button>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        {integrations.map((integration, index) => (
          <Card key={index} className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl hover:shadow-2xl transition-all duration-300">
            <CardHeader className="bg-gradient-to-r from-stone-100/80 to-gray-100/80 dark:from-slate-700/80 dark:to-slate-600/80 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
              <CardTitle className="flex items-center justify-between text-stone-800 dark:text-slate-100">
                <div className="flex items-center space-x-3">
                  <span className="text-2xl">{integration.icon}</span>
                  <span>{integration.name}</span>
                </div>
                <div className="flex items-center space-x-2">
                  {integration.status === 'connected' ? (
                    <CheckCircle className="w-5 h-5 text-green-500" />
                  ) : (
                    <XCircle className="w-5 h-5 text-red-500" />
                  )}
                  <span className={`text-sm font-medium ${
                    integration.status === 'connected'
                      ? 'text-green-600 dark:text-green-400'
                      : 'text-red-600 dark:text-red-400'
                  }`}>
                    {integration.status === 'connected' ? 'Connected' : 'Disconnected'}
                  </span>
                </div>
              </CardTitle>
              <CardDescription className="text-stone-600 dark:text-slate-400">
                {integration.description}
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="flex space-x-2">
                <Button
                  variant={integration.status === 'connected' ? 'outline' : 'default'}
                  size="sm"
                >
                  {integration.status === 'connected' ? 'Configure' : 'Connect'}
                </Button>
                {integration.status === 'connected' && (
                  <Button variant="outline" size="sm">
                    Disconnect
                  </Button>
                )}
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center space-x-2">
            <Zap className="w-5 h-5" />
            <span>Integration Management</span>
          </CardTitle>
          <CardDescription>
            Manage your external integrations and data sync
          </CardDescription>
        </CardHeader>
        <CardContent>
          <p className="text-gray-600">
            Features available:
          </p>
          <ul className="mt-2 space-y-1 text-sm text-gray-600">
            <li>• Real-time data synchronization</li>
            <li>• Webhook management</li>
            <li>• API key configuration</li>
            <li>• Sync status monitoring</li>
            <li>• Error handling and logs</li>
          </ul>
        </CardContent>
      </Card>
    </div>
  );
};

export default Integrations;
