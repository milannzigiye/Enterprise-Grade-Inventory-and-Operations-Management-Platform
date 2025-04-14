# 🚀 Enterprise-Grade Stock and Operations Management System  
### Multi-Channel Inventory Control with Analytics, GPS, and AI Support

---

> **Note:** This system is developed as part of our **Final Year Project** and also contributes to our **Final Exam assessment**. The project simulates enterprise-level software built to handle real-world inventory challenges across multi-branch businesses. All development and collaboration are managed through GitHub.

---

## 📊 Project Overview

This platform is a fully integrated inventory and operations system that empowers businesses to manage products, warehouses, orders, and analytics across multiple locations and user roles.

It includes:
- **A manager/admin interface** for backend operations, inventory control, supplier handling, and reporting.
- **A customer interface** for browsing, ordering, tracking, and interacting with AI-powered support.
- **Intelligent components** like a chatbot for assistance and GPS mapping for delivery tracking.
- **Third-party connectivity** to allow integration with email systems, SMS gateways, and potentially online stores.

### Admin Dashboard (Preview)
> The main dashboard offers real-time KPIs including sales, low-stock alerts, order summaries, and financial analytics.

![Admin Dashboard Screenshot](...)

### Inventory Management View
> Managers can add, edit, or track stock levels across different warehouses, with real-time quantity adjustments and category-based filtering.

![Inventory Management Screenshot](...)

### Customer Order Flow
> Customers can view products by category, add items to cart, and place orders. Orders can then be tracked via GPS-enabled delivery views.

![Customer Order Flow Screenshot](...)

---


## 📝 Project Description

The system is a web-based platform designed to handle complex stock and operations management requirements for growing businesses. Unlike basic inventory tools, this solution is built for multi-branch coordination and role-specific interaction.

It introduces:
- **Dynamic stock tracking**
- **Real-time order and delivery monitoring**
- **Automated reporting tools**
- **AI support (via chatbot)**
- **User-specific dashboards**
- **Secure and modular architecture**

It is aimed at SMEs, warehouse hubs, logistics teams, and multi-outlet stores seeking control, insight, and scalability.

---

## 🧩 Key System Modules

### 🔐 Manager/Admin Interface
- Personalized dashboards showing performance insights
- Warehouse and product inventory control
- Supplier database and purchase order generation
- Analytics and financial reporting
- Email/SMS notifications setup
- ChatBot integration for operational queries
- Delivery route monitoring via GPS
- User and role management

### 🛍️ Customer Interface
- Product catalog with filtering
- Cart management and checkout system
- Order tracking with live updates
- ChatBot assistant for product support
- Downloadable receipts and invoice previews
- GPS-powered delivery updates

---

## ✅ Core Features

| Category              | Key Features |
|-----------------------|--------------|
| **Inventory Control** | Add/edit/delete items, track stock in multiple warehouses |
| **User Roles**        | Admin, Staff, Customer, Auditor (permissions-based) |
| **Order Management**  | Customer orders, supplier purchases, tracking, and status |
| **Security**          | Password hashing, role validation, SQL injection prevention |
| **Reporting**         | Exportable reports (CSV, Excel, PDF), custom filters |
| **ChatBot Integration** | NLP-based assistant available on both interfaces |
| **GPS Integration**   | Visual route mapping, branch locator, live tracking |
| **Third-Party APIs**  | Email gateway, SMS alerts, optional eCommerce sync |

---

## 🛠️ Tech Stack

| Layer         | Tools/Technologies                          |
|---------------|---------------------------------------------|
| **Frontend**  | Razor Pages, HTML5, CSS3, Bootstrap 5        |
| **Backend**   | ASP.NET Core Web API, C#                    |
| **Database**  | Microsoft SQL Server (2014+)                |
| **ORM**       | Entity Framework                            |
| **IDE**       | Visual Studio 2022                          |
| **DevOps**    | Git + GitHub for version control            |
| **Documentation & APIs** | Swagger, XML comments, API explorer |
| **Integrations** | Google Maps API (GPS), Dialogflow/Bot Framework, SMTP API |

---

## ⚙️ Setup Instructions

To get the system running locally for development, testing, or demonstration, follow the steps below:

### ✅ Requirements
- Visual Studio 2022 (with .NET Core & ASP.NET workloads)
- Microsoft SQL Server 2014 or later
- .NET 6 SDK or higher
- Git installed on your machine


📈 Reporting Example Screenshots

The system includes a comprehensive reporting engine that converts raw business data into visual insights. Below are examples of the types of reports users can generate.

Revenue & Sales Trend Report

Displays total revenue over time, top-selling products, and peak sales periods for better marketing decisions.

![Revenue and Sales Report](...)

Inventory Movement Report

Tracks all changes to inventory, such as restocking, low-stock alerts, and usage trends across all locations.

![Inventory Movement Report](...)

Customer Order Summary

Summarizes order volumes, fulfillment statuses, return rates, and payment completion trends.

![Customer Order Summary Report](...)

⸻

🧠 Use Cases

1. Inventory Manager – Real-time Branch Replenishment

Sarah, an inventory manager, logs in and sees that the Kigali branch is low on “Bluetooth Earbuds.” She uses the warehouse dashboard to transfer surplus stock from the Nyamirambo branch. The system updates inventory in real time and logs the transaction.

2. Customer – Seamless Ordering and Delivery Tracking

Jean browses products and places an order through the customer interface. After checkout, he receives an email confirmation and can track delivery through an interactive GPS map.

3. Admin – Audit and Oversight

David, the system admin, exports monthly supplier performance reports and verifies staff activity through the audit log. He uses these insights to brief upper management.

4. ChatBot – 24/7 Self-Service Support

A customer wants to know if a wristband is available in medium. Instead of waiting for staff, they ask the chatbot, which instantly provides the size options based on current inventory.

5. General Manager – Data-Driven Decision Making

Claire, a general manager, filters order fulfillment rates by branch. She notices delays in one region and adjusts that branch’s reorder threshold, preventing future bottlenecks.

⸻

✅ Final Notes

This project demonstrates how academic learning can be translated into real-world, enterprise-level software systems. It integrates:
	•	Modular system architecture
	•	Secure user role management
	•	Intelligent automation (ChatBot, notifications)
	•	Location services (GPS tracking)
	•	Multi-user, multi-interface design (Admin + Customer)

All development was performed collaboratively using GitHub with proper version control, pull requests, and task assignment.

For feedback, setup support, or demonstration requests, feel free to reach out via the repository Issues tab or contact a team member directly.

Thank you for reviewing our final year project!
