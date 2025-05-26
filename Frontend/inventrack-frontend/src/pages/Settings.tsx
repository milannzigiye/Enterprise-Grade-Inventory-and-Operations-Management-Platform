import React, { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/card';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import { Switch } from '../components/ui/switch';
import { useTheme } from '../contexts/ThemeContext';
import { useAuth } from '../contexts/AuthContext';
import { useToast } from '../hooks/use-toast';
import TwoFactorSetupComponent from '../components/auth/TwoFactorSetup';
import { Settings as SettingsIcon, User, Bell, Shield, Database, Palette, Moon, Sun, Mail } from 'lucide-react';

const Settings: React.FC = () => {
  const { theme, toggleTheme } = useTheme();
  const { toast } = useToast();
  const [notifications, setNotifications] = useState({
    email: true,
    lowStock: true,
    orders: true,
    marketing: false,
  });

  const handleNotificationChange = (key: keyof typeof notifications) => {
    setNotifications(prev => ({
      ...prev,
      [key]: !prev[key]
    }));
    toast({
      title: "Settings Updated",
      description: "Notification preferences have been saved",
      variant: "success",
    });
  };

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Settings</h1>
        <p className="text-stone-600 dark:text-slate-400">Manage your application preferences and configuration</p>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        {/* Appearance Settings */}
        <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
          <CardHeader className="bg-gradient-to-r from-stone-100/80 to-gray-100/80 dark:from-slate-700/80 dark:to-slate-600/80 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
            <CardTitle className="flex items-center space-x-2 text-stone-800 dark:text-slate-100">
              <Palette className="w-5 h-5 text-red-500" />
              <span>Appearance</span>
            </CardTitle>
            <CardDescription className="text-stone-600 dark:text-slate-400">
              Customize the look and feel of your application
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex items-center justify-between">
              <div className="flex items-center space-x-3">
                {theme === 'light' ? <Sun className="w-5 h-5" /> : <Moon className="w-5 h-5" />}
                <div>
                  <p className="font-medium">Theme</p>
                  <p className="text-sm text-gray-500 dark:text-gray-400">
                    {theme === 'light' ? 'Light mode' : 'Dark mode'}
                  </p>
                </div>
              </div>
              <Switch
                checked={theme === 'dark'}
                onCheckedChange={toggleTheme}
              />
            </div>
            <div className="pt-2">
              <p className="text-sm text-gray-600 dark:text-gray-400">
                Toggle between light and dark themes to match your preference.
              </p>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <User className="w-5 h-5" />
              <span>Profile Settings</span>
            </CardTitle>
            <CardDescription>
              Update your personal information
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <label className="text-sm font-medium">Full Name</label>
              <Input placeholder="Enter your full name" />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-medium">Email</label>
              <Input type="email" placeholder="Enter your email" />
            </div>
            <Button>Save Changes</Button>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <Bell className="w-5 h-5" />
              <span>Notifications</span>
            </CardTitle>
            <CardDescription>
              Configure your notification preferences
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex items-center justify-between">
              <div className="flex items-center space-x-3">
                <Mail className="w-4 h-4 text-gray-400" />
                <div>
                  <p className="font-medium">Email Notifications</p>
                  <p className="text-sm text-gray-500 dark:text-gray-400">Receive updates via email</p>
                </div>
              </div>
              <Switch
                checked={notifications.email}
                onCheckedChange={() => handleNotificationChange('email')}
              />
            </div>
            <div className="flex items-center justify-between">
              <div className="flex items-center space-x-3">
                <Bell className="w-4 h-4 text-gray-400" />
                <div>
                  <p className="font-medium">Low Stock Alerts</p>
                  <p className="text-sm text-gray-500 dark:text-gray-400">Get notified when stock is low</p>
                </div>
              </div>
              <Switch
                checked={notifications.lowStock}
                onCheckedChange={() => handleNotificationChange('lowStock')}
              />
            </div>
            <div className="flex items-center justify-between">
              <div className="flex items-center space-x-3">
                <SettingsIcon className="w-4 h-4 text-gray-400" />
                <div>
                  <p className="font-medium">Order Updates</p>
                  <p className="text-sm text-gray-500 dark:text-gray-400">Notifications for new orders</p>
                </div>
              </div>
              <Switch
                checked={notifications.orders}
                onCheckedChange={() => handleNotificationChange('orders')}
              />
            </div>
            <div className="flex items-center justify-between">
              <div className="flex items-center space-x-3">
                <Mail className="w-4 h-4 text-gray-400" />
                <div>
                  <p className="font-medium">Marketing Emails</p>
                  <p className="text-sm text-gray-500 dark:text-gray-400">Receive promotional content</p>
                </div>
              </div>
              <Switch
                checked={notifications.marketing}
                onCheckedChange={() => handleNotificationChange('marketing')}
              />
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <Shield className="w-5 h-5" />
              <span>Security</span>
            </CardTitle>
            <CardDescription>
              Manage your account security
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <label className="text-sm font-medium">Current Password</label>
              <Input type="password" placeholder="Enter current password" />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-medium">New Password</label>
              <Input type="password" placeholder="Enter new password" />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-medium">Confirm Password</label>
              <Input type="password" placeholder="Confirm new password" />
            </div>
            <Button>Update Password</Button>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <Database className="w-5 h-5" />
              <span>System Settings</span>
            </CardTitle>
            <CardDescription>
              Configure system-wide settings
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <label className="text-sm font-medium">Company Name</label>
              <Input placeholder="Enter company name" />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-medium">Default Currency</label>
              <Input placeholder="USD" />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-medium">Time Zone</label>
              <Input placeholder="UTC-5 (Eastern Time)" />
            </div>
            <Button>Save Settings</Button>
          </CardContent>
        </Card>
      </div>

      {/* Two-Factor Authentication */}
      <TwoFactorSetupComponent />

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center space-x-2">
            <SettingsIcon className="w-5 h-5" />
            <span>Advanced Settings</span>
          </CardTitle>
          <CardDescription>
            Advanced configuration options
          </CardDescription>
        </CardHeader>
        <CardContent>
          <p className="text-gray-600">
            Additional settings:
          </p>
          <ul className="mt-2 space-y-1 text-sm text-gray-600">
            <li>• API configuration</li>
            <li>• Backup settings</li>
            <li>• Data export options</li>
            <li>• Integration settings</li>
            <li>• Audit logs</li>
          </ul>
        </CardContent>
      </Card>
    </div>
  );
};

export default Settings;
