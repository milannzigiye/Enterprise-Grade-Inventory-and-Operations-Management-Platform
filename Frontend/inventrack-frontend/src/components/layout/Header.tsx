import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Popover, PopoverContent, PopoverTrigger } from '../ui/popover';
import { useTheme } from '../../contexts/ThemeContext';
import { useAuth } from '../../contexts/AuthContext';
import { useToast } from '../../hooks/use-toast';
import { Search, Bell, User, Settings, Moon, Sun, LogOut, UserCircle } from 'lucide-react';

const Header: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchResults, setSearchResults] = useState<any[]>([]);
  const [showSearchResults, setShowSearchResults] = useState(false);
  const { theme, toggleTheme } = useTheme();
  const { user, logout } = useAuth();
  const { toast } = useToast();
  const navigate = useNavigate();

  // Mock search data - in real app, this would come from your search service
  const searchableItems = [
    { type: 'product', name: 'Wireless Headphones', path: '/products' },
    { type: 'customer', name: 'John Doe', path: '/customers' },
    { type: 'order', name: 'Order #1234', path: '/orders' },
    { type: 'supplier', name: 'TechCorp', path: '/suppliers' },
    { type: 'warehouse', name: 'Main Warehouse', path: '/warehouse' },
    { type: 'user', name: 'Admin User', path: '/users' },
    { type: 'page', name: 'Analytics', path: '/analytics' },
    { type: 'page', name: 'Settings', path: '/settings' },
  ];

  const handleSearch = (value: string) => {
    setSearchTerm(value);
    if (value.trim()) {
      const results = searchableItems.filter(item =>
        item.name.toLowerCase().includes(value.toLowerCase()) ||
        item.type.toLowerCase().includes(value.toLowerCase())
      );
      setSearchResults(results);
      setShowSearchResults(true);
    } else {
      setSearchResults([]);
      setShowSearchResults(false);
    }
  };

  const handleSearchResultClick = (path: string) => {
    navigate(path);
    setSearchTerm('');
    setShowSearchResults(false);
  };

  const handleLogout = async () => {
    try {
      await logout();
      toast({
        title: 'Success',
        description: 'Logged out successfully',
      });
      navigate('/login');
    } catch (error) {
      toast({
        title: 'Error',
        description: 'Failed to logout',
        variant: 'destructive',
      });
    }
  };

  return (
    <header className="relative">
      {/* Background with glass morphism */}
      <div className="absolute inset-0 bg-gradient-to-r from-stone-50/95 via-white/98 to-gray-50/95 dark:from-slate-800/95 dark:via-slate-900/98 dark:to-gray-900/95 backdrop-blur-xl border-b border-stone-200/60 dark:border-slate-700/60 shadow-xl shadow-stone-200/20 dark:shadow-slate-900/40" />

      {/* Subtle animated gradient overlay */}
      <div className="absolute inset-0 bg-gradient-to-r from-red-50/10 via-transparent to-stone-50/20 dark:from-red-900/5 dark:via-transparent dark:to-slate-800/10 animate-gradient-x" />

      <div className="relative z-10 flex items-center justify-between px-8 py-5">
        {/* Logo and Title with enhanced styling */}
        <div className="flex items-center space-x-6">
          <div className="flex items-center space-x-4 group">
            <div className="relative">
              <div className="w-12 h-12 bg-gradient-to-br from-red-500 to-red-600 rounded-2xl flex items-center justify-center shadow-lg shadow-red-500/30 transform group-hover:scale-105 transition-all duration-300">
                <span className="text-white font-bold text-xl tracking-wider">I</span>
              </div>
              <div className="absolute inset-0 w-12 h-12 bg-red-400 rounded-2xl animate-pulse opacity-20 group-hover:opacity-30 transition-opacity duration-300" />
            </div>
            <div>
              <h1 className="text-3xl font-black text-transparent bg-clip-text bg-gradient-to-r from-stone-800 to-gray-700 dark:from-slate-100 dark:to-gray-200 tracking-tight">
                InvenTrack Pro
              </h1>
              <p className="text-sm text-stone-500 dark:text-slate-400 font-medium">
                Inventory Management System
              </p>
            </div>
          </div>
        </div>

        {/* Enhanced Search Bar */}
        <div className="flex-1 max-w-xl mx-12 relative group">
          <div className="relative">
            <div className="absolute inset-0 bg-gradient-to-r from-stone-100 to-gray-100 dark:from-slate-700 dark:to-slate-600 rounded-2xl blur-sm group-focus-within:blur-none transition-all duration-300" />
            <div className="relative bg-white/80 dark:bg-slate-800/80 backdrop-blur-sm rounded-2xl border border-stone-200/60 dark:border-slate-600/60 shadow-lg shadow-stone-200/30 dark:shadow-slate-900/30 group-focus-within:shadow-xl group-focus-within:shadow-stone-300/40 dark:group-focus-within:shadow-slate-800/40 transition-all duration-300">
              <Search className="absolute left-5 top-1/2 transform -translate-y-1/2 text-stone-400 dark:text-slate-400 w-5 h-5 group-focus-within:text-red-500 transition-colors duration-300" />
              <Input
                type="text"
                placeholder="Search products, orders, customers, pages..."
                value={searchTerm}
                onChange={(e) => handleSearch(e.target.value)}
                className="pl-14 pr-6 py-4 w-full bg-transparent border-0 text-stone-700 dark:text-slate-200 placeholder:text-stone-400 dark:placeholder:text-slate-400 focus:ring-0 focus:outline-none text-base"
                onFocus={() => searchTerm && setShowSearchResults(true)}
                onBlur={() => setTimeout(() => setShowSearchResults(false), 200)}
              />
            </div>
          </div>

          {/* Enhanced Search Results Dropdown */}
          {showSearchResults && searchResults.length > 0 && (
            <div className="absolute top-full left-0 right-0 mt-2 bg-white/95 dark:bg-slate-800/95 backdrop-blur-xl border border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-2xl shadow-stone-300/30 dark:shadow-slate-900/50 z-50 max-h-80 overflow-y-auto">
              {searchResults.map((result, index) => (
                <div
                  key={index}
                  className="px-6 py-4 hover:bg-gradient-to-r hover:from-stone-50 hover:to-gray-50 dark:hover:from-slate-700 dark:hover:to-slate-600 cursor-pointer border-b border-stone-100/60 dark:border-slate-700/60 last:border-b-0 transition-all duration-200 group"
                  onClick={() => handleSearchResultClick(result.path)}
                >
                  <div className="flex items-center space-x-4">
                    <span className="text-xs bg-gradient-to-r from-red-500 to-red-600 text-white px-3 py-1.5 rounded-full capitalize font-semibold shadow-sm">
                      {result.type}
                    </span>
                    <span className="text-stone-700 dark:text-slate-200 font-medium group-hover:text-stone-900 dark:group-hover:text-white transition-colors duration-200">
                      {result.name}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Right Side Actions with enhanced styling */}
        <div className="flex items-center space-x-3">
          <Button
            variant="ghost"
            size="icon"
            className="w-12 h-12 rounded-2xl hover:bg-stone-100/80 dark:hover:bg-slate-700/80 hover:shadow-lg hover:shadow-stone-200/40 dark:hover:shadow-slate-800/40 transition-all duration-300 group"
          >
            <Bell className="w-5 h-5 text-stone-600 dark:text-slate-300 group-hover:text-red-500 transition-colors duration-300" />
          </Button>

          <Button
            variant="ghost"
            size="icon"
            onClick={toggleTheme}
            className="w-12 h-12 rounded-2xl hover:bg-stone-100/80 dark:hover:bg-slate-700/80 hover:shadow-lg hover:shadow-stone-200/40 dark:hover:shadow-slate-800/40 transition-all duration-300 group"
          >
            {theme === 'light' ?
              <Moon className="w-5 h-5 text-stone-600 group-hover:text-blue-500 transition-all duration-300 group-hover:rotate-12" /> :
              <Sun className="w-5 h-5 text-slate-300 group-hover:text-yellow-400 transition-all duration-300 group-hover:rotate-12" />
            }
          </Button>

          <Popover>
            <PopoverTrigger asChild>
              <Button
                variant="ghost"
                size="icon"
                className="w-12 h-12 rounded-2xl hover:bg-stone-100/80 dark:hover:bg-slate-700/80 hover:shadow-lg hover:shadow-stone-200/40 dark:hover:shadow-slate-800/40 transition-all duration-300 group"
              >
                <User className="w-5 h-5 text-stone-600 dark:text-slate-300 group-hover:text-red-500 transition-colors duration-300" />
              </Button>
            </PopoverTrigger>
            <PopoverContent className="w-64 bg-white/95 dark:bg-slate-800/95 backdrop-blur-xl border border-stone-200/60 dark:border-slate-600/60 rounded-2xl shadow-2xl shadow-stone-300/30 dark:shadow-slate-900/50" align="end">
              <div className="space-y-3">
                <div className="flex items-center space-x-3 p-3 rounded-xl bg-gradient-to-r from-stone-50 to-gray-50 dark:from-slate-700 dark:to-slate-600">
                  <div className="w-12 h-12 bg-gradient-to-br from-red-500 to-red-600 rounded-2xl flex items-center justify-center shadow-lg">
                    <UserCircle className="w-6 h-6 text-white" />
                  </div>
                  <div>
                    <p className="text-sm font-semibold text-stone-800 dark:text-slate-100">
                      {user?.profile?.firstName && user?.profile?.lastName
                        ? `${user.profile.firstName} ${user.profile.lastName}`
                        : user?.username || 'User'
                      }
                    </p>
                    <p className="text-xs text-stone-500 dark:text-slate-400">{user?.email || 'user@example.com'}</p>
                  </div>
                </div>

                <div className="space-y-1">
                  <Button
                    variant="ghost"
                    className="w-full justify-start rounded-xl hover:bg-stone-100/80 dark:hover:bg-slate-700/80 transition-all duration-200"
                    onClick={() => navigate('/profile')}
                  >
                    <UserCircle className="w-4 h-4 mr-3" />
                    Profile
                  </Button>
                  <Button
                    variant="ghost"
                    className="w-full justify-start rounded-xl hover:bg-stone-100/80 dark:hover:bg-slate-700/80 transition-all duration-200"
                    onClick={() => navigate('/settings')}
                  >
                    <Settings className="w-4 h-4 mr-3" />
                    Settings
                  </Button>
                </div>

                <hr className="border-stone-200/60 dark:border-slate-600/60" />

                <Button
                  variant="ghost"
                  className="w-full justify-start text-red-600 hover:text-red-700 hover:bg-red-50/80 dark:hover:bg-red-900/20 rounded-xl transition-all duration-200"
                  onClick={handleLogout}
                >
                  <LogOut className="w-4 h-4 mr-3" />
                  Logout
                </Button>
              </div>
            </PopoverContent>
          </Popover>
        </div>
      </div>
    </header>
  );
};

export default Header;