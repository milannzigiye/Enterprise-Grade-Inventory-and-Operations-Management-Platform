import React, { useState, useEffect } from 'react';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { useAuth } from '../../contexts/AuthContext';
import { authService, type TwoFactorSetup } from '../../services/authService';
import { useToast } from '../../hooks/use-toast';
import { Shield, QrCode, Key, CheckCircle, XCircle } from 'lucide-react';

const TwoFactorSetupComponent: React.FC = () => {
  const [setup, setSetup] = useState<TwoFactorSetup | null>(null);
  const [verificationCode, setVerificationCode] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isEnabled, setIsEnabled] = useState(false);
  const { user } = useAuth();
  const { toast } = useToast();

  useEffect(() => {
    if (user) {
      setIsEnabled(user.twoFactorEnabled);
    }
  }, [user]);

  const handleGetSetup = async () => {
    try {
      setIsLoading(true);
      const setupData = await authService.getTwoFactorSetup();
      setSetup(setupData);
    } catch (error: any) {
      toast({
        title: 'Error',
        description: error.response?.data?.message || 'Failed to get 2FA setup',
        variant: 'destructive',
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handleEnable = async () => {
    if (!verificationCode) {
      toast({
        title: 'Error',
        description: 'Please enter the verification code',
        variant: 'destructive',
      });
      return;
    }

    try {
      setIsLoading(true);
      await authService.enableTwoFactor(verificationCode);
      setIsEnabled(true);
      setSetup(null);
      setVerificationCode('');
      toast({
        title: 'Success',
        description: 'Two-factor authentication enabled successfully',
      });
    } catch (error: any) {
      toast({
        title: 'Error',
        description: error.response?.data?.message || 'Failed to enable 2FA',
        variant: 'destructive',
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handleDisable = async () => {
    try {
      setIsLoading(true);
      await authService.disableTwoFactor();
      setIsEnabled(false);
      setSetup(null);
      toast({
        title: 'Success',
        description: 'Two-factor authentication disabled successfully',
      });
    } catch (error: any) {
      toast({
        title: 'Error',
        description: error.response?.data?.message || 'Failed to disable 2FA',
        variant: 'destructive',
      });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Shield className="w-5 h-5" />
          Two-Factor Authentication
        </CardTitle>
        <CardDescription>
          Add an extra layer of security to your account with two-factor authentication.
        </CardDescription>
      </CardHeader>
      <CardContent className="space-y-6">
        <div className="flex items-center justify-between p-4 border rounded-lg">
          <div className="flex items-center gap-3">
            {isEnabled ? (
              <CheckCircle className="w-5 h-5 text-green-500" />
            ) : (
              <XCircle className="w-5 h-5 text-red-500" />
            )}
            <div>
              <p className="font-medium">
                Two-Factor Authentication is {isEnabled ? 'Enabled' : 'Disabled'}
              </p>
              <p className="text-sm text-gray-600 dark:text-gray-400">
                {isEnabled 
                  ? 'Your account is protected with 2FA'
                  : 'Enable 2FA to secure your account'
                }
              </p>
            </div>
          </div>
          {isEnabled ? (
            <Button
              variant="destructive"
              onClick={handleDisable}
              disabled={isLoading}
            >
              Disable 2FA
            </Button>
          ) : (
            <Button
              onClick={handleGetSetup}
              disabled={isLoading}
            >
              Enable 2FA
            </Button>
          )}
        </div>

        {setup && !isEnabled && (
          <div className="space-y-6 p-6 border rounded-lg bg-gray-50 dark:bg-gray-800">
            <div className="text-center">
              <h3 className="text-lg font-semibold mb-2">Set up Two-Factor Authentication</h3>
              <p className="text-sm text-gray-600 dark:text-gray-400 mb-4">
                Scan the QR code with your authenticator app or enter the secret key manually.
              </p>
            </div>

            <div className="grid md:grid-cols-2 gap-6">
              <div className="text-center">
                <div className="flex items-center justify-center mb-3">
                  <QrCode className="w-5 h-5 mr-2" />
                  <span className="font-medium">QR Code</span>
                </div>
                <div className="bg-white p-4 rounded-lg inline-block">
                  <img 
                    src={setup.qrCodeUrl} 
                    alt="2FA QR Code" 
                    className="w-48 h-48 mx-auto"
                  />
                </div>
              </div>

              <div>
                <div className="flex items-center mb-3">
                  <Key className="w-5 h-5 mr-2" />
                  <span className="font-medium">Manual Entry</span>
                </div>
                <div className="bg-white dark:bg-gray-700 p-4 rounded-lg border">
                  <p className="text-sm text-gray-600 dark:text-gray-400 mb-2">
                    If you can't scan the QR code, enter this key manually:
                  </p>
                  <code className="block p-2 bg-gray-100 dark:bg-gray-600 rounded text-sm font-mono break-all">
                    {setup.manualEntryKey}
                  </code>
                </div>
              </div>
            </div>

            <div className="space-y-4">
              <div>
                <label htmlFor="verificationCode" className="block text-sm font-medium mb-2">
                  Enter the 6-digit code from your authenticator app:
                </label>
                <Input
                  id="verificationCode"
                  type="text"
                  placeholder="000000"
                  value={verificationCode}
                  onChange={(e) => setVerificationCode(e.target.value.replace(/\D/g, '').slice(0, 6))}
                  className="text-center text-lg tracking-widest"
                  maxLength={6}
                />
              </div>
              <Button
                onClick={handleEnable}
                disabled={isLoading || verificationCode.length !== 6}
                className="w-full"
              >
                {isLoading ? 'Verifying...' : 'Enable Two-Factor Authentication'}
              </Button>
            </div>
          </div>
        )}

        {isEnabled && (
          <div className="p-4 bg-green-50 dark:bg-green-900/20 border border-green-200 dark:border-green-800 rounded-lg">
            <div className="flex items-center gap-2 text-green-800 dark:text-green-200">
              <CheckCircle className="w-5 h-5" />
              <span className="font-medium">Two-Factor Authentication is Active</span>
            </div>
            <p className="text-sm text-green-700 dark:text-green-300 mt-1">
              Your account is now protected with two-factor authentication. You'll need to enter a code from your authenticator app when signing in.
            </p>
          </div>
        )}
      </CardContent>
    </Card>
  );
};

export default TwoFactorSetupComponent;
