# 📦 Stock Management System

> 📘 **Note:** This system is developed as part of our **Final Year Project** and also contributes to our **Final Exam assessment**. All code, documentation, and design submissions are managed through GitHub for review, correction, and collaboration.

---

## 🔍 Introduction

The **Stock Management System** is a full-stack web application created by our group as part of our college coursework. The main objective of this project is to build a system that allows businesses to manage their inventory and stock in a simple, fast, and organized way. The system keeps track of available products, orders, stock levels, customer interactions, and other important data needed for smooth day-to-day operations.

This solution is designed specifically for **small to medium-sized businesses (SMBs)** that want to move away from manually updating spreadsheets or writing things down on paper. By moving their stock management process to a digital platform, businesses can reduce human error, save time, and make better decisions based on real-time information.

<!-- ✅ ADD SYSTEM SCREENSHOTS HERE WHEN READY -->
<!-- Example: ![Dashboard Screenshot](screenshots/dashboard.png) -->

---

## 🎯 Project Goal and Objectives

The purpose of this project is not only to develop a working web application but also to apply the technical skills we’ve learned so far in a real-world-like situation. This includes working as a team, following a clean coding structure, using GitHub for version control, and applying development tools such as Visual Studio and SQL Server.

### Specific Goals:
- ✅ Build a responsive stock management system.
- ✅ Allow different user roles (Admin and permitted Staff) to log in and perform actions.
- ✅ Add/edit/delete products and track their stock levels in real-time.
- ✅ Manage and track customer orders and supplier purchases.
- ✅ Generate real-time reports and data exports.
- ✅ Implement basic system security, including password hashing and input validation.

---

## 🖥️ System Overview

### Key Functional Modules:

#### 1. User Authentication and Access Control
- Role-based access for **Admin** and **permitted Staff**.
- Secure login and registration.
- Passwords are hashed before being stored.
- Basic form validations and user feedback messages for errors or incorrect inputs.

#### 2. Dashboard
- Overview of:
  - Total sales
  - Top-selling products
  - Low-stock alerts
  - Revenue and expenses
- Fully responsive layout, works on mobile

#### 3. Inventory Management
- Add/edit/remove products (name, price, quantity, category, image)
- Product categories: Phones, Watches, Accessories
- Real-time stock quantity tracking with warning indicators for low stock

#### 4. Order Management
- Handle both customer and supplier orders
- View and update order statuses (Pending, Shipped, Completed)
- Order history tracking and search by order details

#### 5. Reporting and Statistics
- Real-time visual and tabular reports:
  - Total revenue
  - Total expenses
  - Inventory movement
  - Order frequency
- Supports filtering (e.g., by date or product category) where applicable
- Reports can be exported (e.g., CSV, Excel, PDF)

#### 6. Mobile View
- Optimized view for smartphones
- Clean, focused layout for small screens

---

## 🔄 CRUD Functionality

This system fully supports **CRUD operations** across all major modules:

### 📦 Inventory Module
- **Create** new product entries
- **Read** and search all current inventory
- **Update** existing product details (price, image, stock quantity)
- **Delete** discontinued items

### 📃 Orders Module
- **Create** orders
- **Read** order records with filters
- **Update** order status and contents
- **Delete** test/duplicate/canceled orders

### 👤 User Accounts (Admin-Only)
- **Create** new users with staff roles
- **Read** and list user accounts
- **Update** credentials and roles
- **Delete** users when no longer needed

---

## 🧰 Technologies and Tools Used

| Component         | Technology                            |
|------------------|----------------------------------------|
| Language          | C#                                    |
| Framework         | ASP.NET MVC & Web API                 |
| Development IDE   | Visual Studio 2022                    |
| Database          | Microsoft SQL Server 2014             |
| Frontend          | Razor Pages, HTML5, CSS3, JavaScript, Bootstrap |
| UI Design         | Figma                                 |
| Version Control   | Git + GitHub                          |
| ORM/DB Access     | Entity Framework                      |

---

## 🔗 API Integration

- RESTful Web APIs
- Used to connect frontend and backend operations
- Supports:
  - Inventory data retrieval and update
  - Order creation and tracking
  - Report generation and export

---

## 📊 Entity Relationship Diagram (ERD)

<!-- ✅ ADD ERD DIAGRAM IMAGE OR LINK HERE WHEN READY -->
<!-- Example: ![ERD](docs/ERD.png) -->

If not already designed, the ERD will visually represent the relationship between key database tables such as:
- Users
- Products
- Orders
- Order Items
- Categories

---

## ⚙️ Setup Instructions

To run the system locally on your machine:

### Prerequisites:
- Visual Studio 2022 (with ASP.NET and .NET Core workloads)
- SQL Server 2014 or later
- .NET SDK installed (version matching the project)

### Steps:
1. **Clone the repository**
   ```bash
   git clone https://github.com/your-group/stock-management-system.git
