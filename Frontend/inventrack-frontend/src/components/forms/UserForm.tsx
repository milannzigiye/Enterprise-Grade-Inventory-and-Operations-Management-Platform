import React, { useState, useEffect } from 'react';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from '../ui/dialog';
import { useToast } from '../../hooks/use-toast';
import type { User } from '../../types/index';
import { userService } from '../../services/userService';
import { X, Save, UserCheck } from 'lucide-react';

interface UserCreateDto {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  password?: string;
}

interface UserFormProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  user?: User | null;
  mode: 'create' | 'edit';
}

const UserForm: React.FC<UserFormProps> = ({
  isOpen,
  onClose,
  onSuccess,
  user,
  mode
}) => {
  const [formData, setFormData] = useState<UserCreateDto>({
    username: '',
    email: '',
    firstName: '',
    lastName: '',
    password: ''
  });
  const [loading, setLoading] = useState(false);
  const { toast } = useToast();

  useEffect(() => {
    if (user && mode === 'edit') {
      setFormData({
        username: user.username,
        email: user.email,
        firstName: user.firstName,
        lastName: user.lastName,
        password: '' // Don't populate password for security
      });
    } else {
      setFormData({
        username: '',
        email: '',
        firstName: '',
        lastName: '',
        password: ''
      });
    }
  }, [user, mode, isOpen]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.username.trim() || !formData.email.trim() || !formData.firstName.trim() || !formData.lastName.trim()) {
      toast({
        title: "Validation Error",
        description: "Please fill in all required fields",
        variant: "destructive",
      });
      return;
    }

    if (mode === 'create' && !formData.password?.trim()) {
      toast({
        title: "Validation Error",
        description: "Password is required for new users",
        variant: "destructive",
      });
      return;
    }

    try {
      setLoading(true);
      
      if (mode === 'create') {
        await userService.createUser(formData);
        toast({
          title: "Success",
          description: "User created successfully",
          variant: "success",
        });
      } else if (user) {
        const updateData = { ...formData };
        if (!updateData.password?.trim()) {
          delete updateData.password; // Don't update password if empty
        }
        await userService.updateUser(user.userId, updateData);
        toast({
          title: "Success",
          description: "User updated successfully",
          variant: "success",
        });
      }
      
      onSuccess();
      onClose();
    } catch (error) {
      console.error('Error saving user:', error);
      toast({
        title: "Error",
        description: `Failed to ${mode} user`,
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
            <UserCheck className="w-6 h-6 text-red-500" />
            <span>{mode === 'create' ? 'Add New User' : 'Edit User'}</span>
          </DialogTitle>
          <DialogDescription className="text-stone-600 dark:text-slate-400">
            {mode === 'create' 
              ? 'Create a new user account for the system'
              : 'Update user account information'
            }
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Account Information */}
            <Card className="md:col-span-2 bg-gradient-to-br from-stone-50 to-gray-50 dark:from-slate-700 dark:to-slate-600 border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-lg">
              <CardHeader className="pb-4">
                <CardTitle className="text-lg text-stone-800 dark:text-slate-100 font-semibold">Account Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Username *
                    </label>
                    <Input
                      name="username"
                      value={formData.username}
                      onChange={handleInputChange}
                      placeholder="Enter username"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                      required
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Email *
                    </label>
                    <Input
                      name="email"
                      type="email"
                      value={formData.email}
                      onChange={handleInputChange}
                      placeholder="Enter email address"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                      required
                    />
                  </div>
                </div>
                
                {(mode === 'create' || formData.password) && (
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Password {mode === 'create' ? '*' : '(leave empty to keep current)'}
                    </label>
                    <Input
                      name="password"
                      type="password"
                      value={formData.password}
                      onChange={handleInputChange}
                      placeholder={mode === 'create' ? "Enter password" : "Enter new password"}
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                      required={mode === 'create'}
                    />
                  </div>
                )}
              </CardContent>
            </Card>

            {/* Personal Information */}
            <Card className="md:col-span-2 bg-gradient-to-br from-stone-50 to-gray-50 dark:from-slate-700 dark:to-slate-600 border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-lg">
              <CardHeader className="pb-4">
                <CardTitle className="text-lg text-stone-800 dark:text-slate-100 font-semibold">Personal Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      First Name *
                    </label>
                    <Input
                      name="firstName"
                      value={formData.firstName}
                      onChange={handleInputChange}
                      placeholder="Enter first name"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                      required
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-stone-700 dark:text-slate-300 mb-2">
                      Last Name *
                    </label>
                    <Input
                      name="lastName"
                      value={formData.lastName}
                      onChange={handleInputChange}
                      placeholder="Enter last name"
                      className="bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
                      required
                    />
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
              {loading ? 'Saving...' : mode === 'create' ? 'Create User' : 'Update User'}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default UserForm;
