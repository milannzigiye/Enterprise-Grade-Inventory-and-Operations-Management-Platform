// Layout.tsx
import React from 'react';
import Header from './Header';
import Sidebar from './Sidebar';

interface LayoutProps {
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <div className="h-screen flex flex-col bg-gradient-to-br from-slate-50 via-gray-50 to-stone-100 dark:from-slate-900 dark:via-gray-900 dark:to-stone-900 transition-all duration-700">
      <Header />
      <div className="flex flex-1 overflow-hidden">
        <Sidebar />
        <main className="flex-1 overflow-auto relative">
          {/* Subtle background pattern */}
          <div className="absolute inset-0 bg-gradient-to-br from-transparent via-white/30 to-stone-100/50 dark:from-transparent dark:via-slate-800/30 dark:to-stone-900/50 pointer-events-none" />
          
          {/* Main content with glass morphism effect */}
          <div className="relative z-10 p-8 min-h-full">
            <div className="backdrop-blur-sm bg-white/70 dark:bg-slate-800/70 rounded-3xl p-6 shadow-xl shadow-stone-200/50 dark:shadow-slate-900/50 border border-stone-200/60 dark:border-slate-700/60 transition-all duration-500">
              {children}
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default Layout;