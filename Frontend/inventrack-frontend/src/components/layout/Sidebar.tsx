// Sidebar.tsx
import React from 'react';
import { NavLink } from 'react-router-dom';
import { cn } from '../../lib/utils';
import {
  LayoutDashboard,
  Package,
  Users,
  ShoppingCart,
  Truck,
  Warehouse,
  UserCheck,
  BarChart3,
  Settings,
  Zap,
  TestTube,
} from 'lucide-react';

interface SidebarItem {
  name: string;
  href: string;
  icon: React.ComponentType<{ className?: string }>;
  badge?: string;
}

const sidebarItems: SidebarItem[] = [
  {
    name: 'Dashboard',
    href: '/',
    icon: LayoutDashboard,
  },
  {
    name: 'Products',
    href: '/products',
    icon: Package,
  },
  {
    name: 'Customers',
    href: '/customers',
    icon: Users,
  },
  {
    name: 'Orders',
    href: '/orders',
    icon: ShoppingCart,
    badge: '12',
  },
  {
    name: 'Suppliers',
    href: '/suppliers',
    icon: Truck,
  },
  {
    name: 'Warehouse',
    href: '/warehouse',
    icon: Warehouse,
  },
  {
    name: 'Users',
    href: '/users',
    icon: UserCheck,
  },
  {
    name: 'Analytics',
    href: '/analytics',
    icon: BarChart3,
  },
  {
    name: 'Integrations',
    href: '/integrations',
    icon: Zap,
  },
  {
    name: 'Settings',
    href: '/settings',
    icon: Settings,
  },
  {
    name: 'API Test',
    href: '/api-test',
    icon: TestTube,
  },
];

const Sidebar: React.FC = () => {
  return (
    <aside className="w-72 relative group">
      {/* Sidebar background with glass morphism */}
      <div className="absolute inset-0 bg-gradient-to-b from-stone-50/90 via-gray-50/95 to-stone-100/90 dark:from-slate-800/90 dark:via-gray-800/95 dark:to-slate-900/90 backdrop-blur-xl border-r border-stone-200/60 dark:border-slate-700/60 shadow-2xl shadow-stone-200/20 dark:shadow-slate-900/40 transition-all duration-700" />

      {/* Subtle gradient overlay */}
      <div className="absolute inset-0 bg-gradient-to-br from-red-50/20 via-transparent to-stone-50/30 dark:from-red-900/10 dark:via-transparent dark:to-slate-800/30 transition-all duration-700" />

      {/* Scrollable navigation */}
      <nav className="relative z-10 h-full overflow-y-auto scrollbar-thin scrollbar-thumb-stone-300 dark:scrollbar-thumb-slate-600 scrollbar-track-transparent hover:scrollbar-thumb-stone-400 dark:hover:scrollbar-thumb-slate-500 transition-all duration-300">
        <div className="p-6 space-y-3">
          {sidebarItems.map((item, index) => (
            <NavLink
              key={item.name}
              to={item.href}
              className={({ isActive }) =>
                cn(
                  'group flex items-center space-x-4 px-5 py-4 rounded-2xl transition-all duration-300 transform hover:scale-[1.02] relative overflow-hidden',
                  isActive
                    ? 'bg-gradient-to-r from-red-500 to-red-600 text-white shadow-lg shadow-red-500/30 dark:shadow-red-600/20'
                    : 'text-stone-700 dark:text-slate-300 hover:bg-gradient-to-r hover:from-stone-100 hover:to-gray-100 dark:hover:from-slate-700 dark:hover:to-slate-600 hover:text-stone-900 dark:hover:text-white hover:shadow-md hover:shadow-stone-200/50 dark:hover:shadow-slate-800/50'
                )
              }
              style={{
                animationDelay: `${index * 50}ms`,
                animation: 'slideInLeft 0.6s ease-out forwards'
              }}
            >
              {/* Active indicator line */}
              <div className={cn(
                'absolute left-0 top-0 bottom-0 w-1 bg-gradient-to-b from-red-400 to-red-600 transition-all duration-300',
                ({ isActive }: { isActive: boolean }) => isActive ? 'opacity-100' : 'opacity-0'
              )} />

              {/* Icon with subtle animation */}
              <div className="relative">
                <item.icon className="w-6 h-6 transition-transform duration-300 group-hover:scale-110" />
              </div>

              {/* Text with smooth transition */}
              <span className="font-semibold text-sm tracking-wide transition-all duration-300 group-hover:translate-x-1">
                {item.name}
              </span>

              {/* Badge with pulsing animation */}
              {item.badge && (
                <div className="ml-auto relative">
                  <span className="inline-flex items-center justify-center w-6 h-6 bg-gradient-to-r from-red-500 to-red-600 text-white text-xs font-bold rounded-full shadow-lg animate-pulse">
                    {item.badge}
                  </span>
                  <span className="absolute inset-0 w-6 h-6 bg-red-400 rounded-full animate-ping opacity-20" />
                </div>
              )}

              {/* Hover gradient effect */}
              <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/10 to-transparent translate-x-[-100%] group-hover:translate-x-[100%] transition-transform duration-1000 ease-out" />
            </NavLink>
          ))}
        </div>

        {/* Bottom gradient fade */}
        <div className="absolute bottom-0 left-0 right-0 h-8 bg-gradient-to-t from-stone-100/90 to-transparent dark:from-slate-900/90 pointer-events-none" />
      </nav>
    </aside>
  );
};

export default Sidebar;