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
import UserForm from '../components/forms/UserForm';
import { useToast } from '../hooks/use-toast';
import type { User } from '../types/index';
import { userService } from '../services/userService';
import { Plus, Search, Filter, Edit, Trash2, Eye, Mail, UserCheck, Users as UsersIcon } from 'lucide-react';

const Users: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredUsers, setFilteredUsers] = useState<User[]>([]);
  const [showUserForm, setShowUserForm] = useState(false);
  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const [formMode, setFormMode] = useState<'create' | 'edit'>('create');
  const { toast } = useToast();

  useEffect(() => {
    loadUsers();
  }, []);

  useEffect(() => {
    // Filter users based on search term
    const filtered = users.filter(
      (user) =>
        user.username.toLowerCase().includes(searchTerm.toLowerCase()) ||
        user.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
        user.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        user.lastName.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredUsers(filtered);
  }, [users, searchTerm]);

  const loadUsers = async () => {
    try {
      setLoading(true);
      const data = await userService.getAllUsers();
      setUsers(data);
    } catch (error) {
      console.error('Error loading users:', error);
      // Mock data for demo
      setUsers([
        {
          userId: 1,
          username: 'admin',
          email: 'admin@inventrackpro.com',
          firstName: 'Admin',
          lastName: 'User',
          isActive: true,
          createdDate: '2024-01-01T10:00:00Z',
          lastLoginDate: '2024-01-20T15:30:00Z',
          roles: [{ roleId: 1, roleName: 'Administrator', description: 'Full system access' }]
        },
        {
          userId: 2,
          username: 'manager',
          email: 'manager@inventrackpro.com',
          firstName: 'John',
          lastName: 'Manager',
          isActive: true,
          createdDate: '2024-01-05T09:00:00Z',
          lastLoginDate: '2024-01-19T14:20:00Z',
          roles: [{ roleId: 2, roleName: 'Manager', description: 'Management access' }]
        },
        {
          userId: 3,
          username: 'employee',
          email: 'employee@inventrackpro.com',
          firstName: 'Jane',
          lastName: 'Employee',
          isActive: true,
          createdDate: '2024-01-10T11:00:00Z',
          lastLoginDate: '2024-01-18T16:45:00Z',
          roles: [{ roleId: 3, roleName: 'Employee', description: 'Basic access' }]
        },
        {
          userId: 4,
          username: 'inactive_user',
          email: 'inactive@inventrackpro.com',
          firstName: 'Inactive',
          lastName: 'User',
          isActive: false,
          createdDate: '2024-01-08T12:00:00Z',
          lastLoginDate: '2024-01-15T10:00:00Z',
          roles: [{ roleId: 3, roleName: 'Employee', description: 'Basic access' }]
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteUser = async (userId: number) => {
    if (window.confirm('Are you sure you want to delete this user?')) {
      try {
        await userService.deleteUser(userId);
        await loadUsers();
        toast({
          title: "Success",
          description: "User deleted successfully",
          variant: "success",
        });
      } catch (error) {
        console.error('Error deleting user:', error);
        toast({
          title: "Error",
          description: "Failed to delete user",
          variant: "destructive",
        });
      }
    }
  };

  const handleCreateUser = () => {
    setSelectedUser(null);
    setFormMode('create');
    setShowUserForm(true);
  };

  const handleEditUser = (user: User) => {
    setSelectedUser(user);
    setFormMode('edit');
    setShowUserForm(true);
  };

  const handleFormSuccess = () => {
    loadUsers();
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64 fade-in">
        <div className="text-lg text-stone-600 dark:text-slate-400">Loading users...</div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-stone-800 dark:text-slate-100 font-mona">Users</h1>
          <p className="text-stone-600 dark:text-slate-400">Manage user accounts and permissions</p>
        </div>
        <Button
          onClick={handleCreateUser}
          className="bg-gradient-to-r from-red-500 to-red-600 hover:from-red-600 hover:to-red-700 text-white rounded-xl shadow-lg shadow-red-500/30 transition-all duration-300 transform hover:scale-105"
        >
          <Plus className="w-4 h-4 mr-2" />
          Add User
        </Button>
      </div>

      {/* Search and Filters */}
      <Card className="bg-gradient-to-br from-stone-50/80 to-gray-50/80 dark:from-slate-700/80 dark:to-slate-600/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-lg">
        <CardContent className="p-6">
          <div className="flex items-center space-x-4">
            <div className="flex-1 relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-stone-400 dark:text-slate-500 w-4 h-4" />
              <Input
                type="text"
                placeholder="Search users by name, username, or email..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-10 bg-white/80 dark:bg-slate-800/80 border-stone-200 dark:border-slate-600 rounded-xl focus:ring-2 focus:ring-red-500/20 focus:border-red-500 transition-all duration-300"
              />
            </div>
            <Button variant="outline" className="border-stone-300 dark:border-slate-600 text-stone-700 dark:text-slate-300 hover:bg-stone-100 dark:hover:bg-slate-700 rounded-xl transition-all duration-300">
              <Filter className="w-4 h-4 mr-2" />
              Filters
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Users Table */}
      <Card className="bg-white/80 dark:bg-slate-800/80 backdrop-blur-xl border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-xl">
        <CardHeader className="bg-gradient-to-r from-stone-100/80 to-gray-100/80 dark:from-slate-700/80 dark:to-slate-600/80 rounded-t-2xl border-b border-stone-200/60 dark:border-slate-600/60">
          <div className="flex items-center justify-between">
            <div>
              <CardTitle className="text-stone-800 dark:text-slate-100 flex items-center space-x-2">
                <UsersIcon className="w-5 h-5 text-red-500" />
                <span>Users ({filteredUsers.length})</span>
              </CardTitle>
              <CardDescription className="text-stone-600 dark:text-slate-400">
                Manage system users and their access levels
              </CardDescription>
            </div>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="hover:bg-transparent border-b border-stone-200/60 dark:border-slate-600/60">
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">User</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Contact</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Role</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Last Login</TableHead>
                <TableHead className="text-stone-700 dark:text-slate-300 font-semibold">Status</TableHead>
                <TableHead className="text-right text-stone-700 dark:text-slate-300 font-semibold">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredUsers.map((user) => (
                <TableRow key={user.userId} className="hover:bg-stone-50/50 dark:hover:bg-slate-700/50 transition-all duration-200 border-b border-stone-100/60 dark:border-slate-700/60">
                  <TableCell>
                    <div className="flex items-center space-x-3">
                      <div className="w-10 h-10 bg-gradient-to-br from-red-500 to-red-600 rounded-full flex items-center justify-center shadow-lg">
                        <span className="text-white font-semibold text-sm">
                          {user.firstName[0]}{user.lastName[0]}
                        </span>
                      </div>
                      <div>
                        <div className="font-semibold text-stone-800 dark:text-slate-100">
                          {user.firstName} {user.lastName}
                        </div>
                        <div className="text-sm text-stone-500 dark:text-slate-400">@{user.username}</div>
                      </div>
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="flex items-center space-x-2">
                      <Mail className="w-4 h-4 text-stone-400 dark:text-slate-500" />
                      <span className="text-sm text-stone-600 dark:text-slate-300">{user.email}</span>
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="text-sm">
                      {user.roles && user.roles.length > 0 ? (
                        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-gradient-to-r from-blue-100 to-blue-200 dark:from-blue-800 dark:to-blue-700 text-blue-800 dark:text-blue-100 border border-blue-200 dark:border-blue-600">
                          {user.roles[0].roleName}
                        </span>
                      ) : (
                        <span className="text-stone-500 dark:text-slate-400">No role assigned</span>
                      )}
                    </div>
                  </TableCell>
                  <TableCell className="text-stone-600 dark:text-slate-300">
                    {user.lastLoginDate ? formatDate(user.lastLoginDate) : 'Never'}
                  </TableCell>
                  <TableCell>
                    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                      user.isActive
                        ? 'bg-gradient-to-r from-green-100 to-green-200 dark:from-green-800 dark:to-green-700 text-green-800 dark:text-green-100 border border-green-200 dark:border-green-600'
                        : 'bg-gradient-to-r from-red-100 to-red-200 dark:from-red-800 dark:to-red-700 text-red-800 dark:text-red-100 border border-red-200 dark:border-red-600'
                    }`}>
                      {user.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </TableCell>
                  <TableCell className="text-right">
                    <div className="flex items-center justify-end space-x-2">
                      <Button variant="ghost" size="sm" className="text-stone-500 dark:text-slate-400 hover:text-blue-600 dark:hover:text-blue-400 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded-lg transition-all duration-200">
                        <Eye className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-stone-500 dark:text-slate-400 hover:text-green-600 dark:hover:text-green-400 hover:bg-green-50 dark:hover:bg-green-900/20 rounded-lg transition-all duration-200"
                        onClick={() => handleEditUser(user)}
                      >
                        <Edit className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-stone-500 dark:text-slate-400 hover:text-red-600 dark:hover:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition-all duration-200"
                        onClick={() => handleDeleteUser(user.userId)}
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

      {/* User Form Modal */}
      <UserForm
        isOpen={showUserForm}
        onClose={() => setShowUserForm(false)}
        onSuccess={handleFormSuccess}
        user={selectedUser}
        mode={formMode}
      />
    </div>
  );
};

export default Users;
