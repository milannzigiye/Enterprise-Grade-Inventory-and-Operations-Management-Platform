# InvenTrack Pro

**Enterprise-Grade Inventory & Operations Management Platform**

---

## 📌 Project Overview
InvenTrack Pro is a modular, scalable, and cloud-ready inventory and operations platform tailored for enterprise-level logistics. Built using the ASP.NET ecosystem, it provides real-time insights, customizable dashboards, mobile accessibility, and multi-role functionality, delivering a professional-grade experience aligned with modern development practices.

---

## ⚙️ Technology Stack
- **Backend:** ASP.NET Core 7.0 / 8.0
- **Frontend:** Blazor WebAssembly or React.js + .NET Web API
- **Database:** SQL Server 2019 / 2022 + Entity Framework Core
- **Authentication:** ASP.NET Identity + JWT + Two-Factor Authentication (MFA)
- **Real-Time:** SignalR + Event-Driven Architecture
- **DevOps & CI/CD:** Azure DevOps Pipelines
- **Cloud Hosting:** Azure App Service + Azure SQL
- **Architecture Patterns:** Microservices, Clean Architecture, CQRS, Repository + Unit of Work

---

## 🧠 Key Features

### 🔐 Authentication & Authorization
- Secure login with hashed passwords
- Two-Factor Authentication (MFA)
- Role-Based Access Control (RBAC) with fine-grained permissions
- Single Sign-On (SSO) support

### 📦 Core Modules
1. **Warehouse Management**
   - Live inventory tracking
   - Visual warehouse mapping with drag & drop
   - Barcode/QR & RFID integration
2. **Product & Inventory**
   - SKU, batch, and serial tracking
   - Inventory valuation (FIFO/LIFO/FEFO)
   - Reorder alerts and discrepancy reports
3. **Supplier Management**
   - Supplier portal with quote submission
   - Purchase orders, delivery tracking, and analytics
4. **Customer Experience**
   - Customer portal + loyalty program
   - Wishlist, order tracking, chatbot
5. **Order Management**
   - Multi-channel order hub
   - Route optimization and GPS tracking
6. **Analytics & BI**
   - KPI dashboards, trend analysis, forecasting
   - Exportable reports (PDF, Excel, CSV)
7. **User & Admin Control**
   - Live account lock/unlock
   - Audit logs for user actions
   - Notification preferences, theme & language customization
8. **File Management**
   - Encrypted uploads and secure downloads
   - Expiring download links
9. **System Maintenance**
   - Automated database backups and restoration tools
   
---

## 📈 Advanced & Innovative Features
- **Mobile-First Design + PWA Compatibility**
- **Multi-Tenant Architecture with White-Label Support**
- **Real-Time Search & Filtering**
- **AI-Powered Chatbot for customer interactions**
- **Drag-and-Drop UI components**

---

## 🛡️ Security & Compliance
- End-to-end encryption (AES, RSA)
- GDPR-compliant data handling
- Retention policy enforcement

---

## 🧱 Architecture Principles
- Layered Clean Architecture (Controller → Service → Repository)
- CQRS for command/query segregation
- Repository + Unit of Work for clean data access
- Event-driven communication with service mesh and API Gateway

---

## 🚀 Setup Instructions

### ✅ Prerequisites
- Visual Studio 2022 (ASP.NET & .NET Core workloads)
- SQL Server 2014 or later
- .NET SDK 6.0+
- Git

### 🛠️ Setup Steps
```bash
1. git clone https://github.com/milannzigiye/Enterprise-Grade-Inventory-and-Operations-Management-System.git
2. Open the solution in Visual Studio 2022
3. Restore NuGet packages
4. Configure your database connection in `appsettings.json`
5. Create the database via EF Core migrations or SQL script in /Database/
6. Press F5 to run the project and launch in your browser
```

---

## 🧪 Development Approach
1. **Phase 1:** Database schema, auth system, base API
2. **Phase 2:** Warehouse, product, and user modules
3. **Phase 3:** Orders, suppliers, customer portals
4. **Phase 4:** BI analytics, integration layer, mobile app

---

## 📌 Project Success Factors
- Secure, encrypted, and modular
- Professional UI/UX with responsive design
- Test coverage and logging
- Strong demo with end-to-end workflows
- CI/CD + optional Dockerization for deployment
- 
## ERD

![Mermaid Chart - Create complex, visual diagrams with text  A smarter way of creating diagrams -2025-05-23-110707](https://github.com/user-attachments/assets/b905d2dd-487a-4ce1-b20b-73eef9adf068)

