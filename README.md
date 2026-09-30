# 🛍️ MaleFashion — E-Commerce Web Application

## 📌 Summary

**MaleFashion** is a full-stack **ASP.NET Core e-commerce application** built using **Clean Architecture, Domain-Driven Design (DDD), CQRS, and modern software engineering practices**.

The system provides separate experiences for **Administrators and Customers** and supports the complete e-commerce lifecycle, from product management and inventory to shopping cart, checkout, payment simulation, and order processing.

### 🏗️ Architecture & Technologies

```text
Clean Architecture
        +
Domain-Driven Design (DDD)
        +
CQRS
        +
Mediator Pattern
        +
Cortex.Mediator
        +
Repository Pattern
        +
Unit of Work
        +
AutoMapper
        +
Entity Framework Core
        +
Stored Procedures
        +
ASP.NET Core Identity
        +
External Services
        +
Unit Testing
        +
Docker
        +
Deployment
```

The architecture keeps **business logic independent from infrastructure and presentation concerns**, making the application easier to **maintain, test, extend, and deploy**.

---

## 📚 Key Engineering Principles Demonstrated

### 🔹 Separation of Concerns

Each architectural layer has a clearly defined responsibility, reducing unnecessary dependencies between components.

### 🔹 Single Responsibility

Classes and services are designed around focused responsibilities, making the code easier to understand and maintain.

### 🔹 Dependency Inversion

Higher-level application logic depends on **abstractions and interfaces** rather than concrete infrastructure implementations.

### 🔹 Loose Coupling

**Cortex.Mediator, Dependency Injection, Repository Pattern, and interfaces** help reduce direct dependencies between application components.

### 🔹 Encapsulation

Domain entities, DTOs, commands, and queries control how application data and business behavior are exposed.

### 🔹 Testability

Application logic is separated from presentation and infrastructure concerns, making individual components easier to unit test.

### 🔹 Maintainability

Feature-oriented organization makes individual business capabilities easier to locate, understand, and modify.

### 🔹 Scalability

The separation between **Domain, Application, Infrastructure, and Web** provides a foundation for extending the application as business requirements grow.

---

## 👨‍💻 Project

### **MaleFashion — Full-Stack E-Commerce Web Application**

**Built with:**

`ASP.NET Core` • `C#` • `Clean Architecture` • `DDD` • `CQRS` • `Cortex.Mediator` • `EF Core` • `SQL Server` • `AutoMapper` • `ASP.NET Core Identity` • `Docker`

The project demonstrates how modern architectural patterns and engineering practices can be combined to build a structured, maintainable, and production-oriented e-commerce application.

## Introduction

**MaleFashion** is a full-stack **e-commerce web application** developed using **ASP.NET Core** and modern software engineering practices. The project follows **Clean Architecture** and **Domain-Driven Design (DDD)** principles to maintain a scalable, maintainable, and loosely coupled codebase.

The application implements **CQRS (Command Query Responsibility Segregation)** with the **Mediator Design Pattern** to separate business operations from data retrieval and provide a clean application flow. **AutoMapper** is used for efficient object-to-object mapping between entities, DTOs, and view models.

The project follows the **Repository Design Pattern** and **Unit of Work Pattern** to provide an organized and testable data-access layer. **Stored Procedures** are also used for specific database operations where optimized and reusable SQL queries are required.

MaleFashion includes **Unit Testing** to validate application behavior and improve code reliability. The application is also **Dockerized**, allowing the complete application environment to be built, configured, and deployed consistently across different environments.

The application provides a complete online shopping experience for customers along with a dedicated **Administration Panel** for managing products, categories, inventory, orders, discounts, users, and other e-commerce operations.

The system covers the complete e-commerce lifecycle, including:

- **Product Management**
- **Inventory Management**
- **Shopping Cart**
- **Wishlist**
- **Discounts and Coupons**
- **Checkout**
- **Order Processing**
- **Payment Simulation**
- **Authentication and Authorization**
- **User Management**
- **Email Services**
- **Google reCAPTCHA**
- **Unit Testing**
- **Docker Containerization**
- **Production Deployment**

The project focuses on **separation of concerns, maintainability, scalability, testability, loose coupling, and secure application development**.

---
## 🎯 Project Objectives

The main objective of **MaleFashion** is to demonstrate the practical implementation of modern software architecture, design patterns, and e-commerce development practices.

The project focuses on:

* 🏗️ **Clean Architecture**
* 🎯 **Domain-Driven Design (DDD)**
* 🔄 **CQRS**
* 🧩 **Mediator Design Pattern**
* 🌐 **ASP.NET Core MVC**
* 🔐 **Authentication & Authorization**
* 🛒 **E-commerce Business Workflows**
* 🗃️ **Repository Pattern**
* 🔄 **Unit of Work Pattern**
* 🗄️ **Entity Framework Core**
* 🛢️ **SQL Server**
* ⚙️ **Stored Procedures**
* 🗺️ **DTO Mapping with AutoMapper**
* 🔌 **External Service Integration**
* 🧪 **Unit Testing**
* 🐳 **Containerization with Docker**
* 🚀 **Production Deployment**


## 🎯 Project Overview

MaleFashion provides two primary user experiences:

### 👨‍💼 Admin Panel

Administrators can manage and maintain the e-commerce platform through a protected **Administration Panel**.

### 🛍️ Customer Storefront

Customers can browse products, select variants, manage their cart and wishlist, apply discounts, checkout, and place orders.

The application uses:

- **MaleFashion Storefront Template** for the customer-facing website
- **Sneat Admin Template** for the administration panel

---

## ✨ Key Features

### 👨‍💼 Admin Features

Administrators can manage:

- **Dashboard**
- **Products**
- **Categories**
- **Product Variants**
- **Inventory**
- **Discounts**
- **Coupons**
- **Orders**
- **Users and Roles**
- **Protected Administration Functionality**

---

### 🛍️ Customer Features

Customers can:

- **Register and Login**
- **Browse Products**
- **Search Products**
- **View Product Details**
- **Select Size and Color**
- **Add Products to Wishlist**
- **Add Products to Cart**
- **Update Cart Quantities**
- **Remove Cart Items**
- **Apply Coupon Codes**
- **Checkout**
- **Select Payment Methods**
- **Perform Simulated Payments**
- **Place Orders**
- **View Order-Related Information**

---

## 📸 Application Screenshots

###### 🔐 Authentication & Account Management

### 📝 Sign Up & Email Verification

<div align="center">

<img width="362" height="582" alt="MaleFashion Screenshot" src="https://github.com/user-attachments/assets/57630ddf-c9b3-4ae1-bd26-0cccdf946a7e" />

<img width="867" height="600" alt="MaleFashion Screenshot" src="https://github.com/user-attachments/assets/f8db6860-6e73-4eeb-95d5-72764e528a68" />

</div>


### 🔑 Sign In


<div align="center">

<img width="553" height="662" alt="MaleFashion Screenshot" src="https://github.com/user-attachments/assets/713a975e-6b7e-48b7-a511-d21a77886299" />

</div>


 ### 🔒 Change Password


<div align="center">

<img width="551" height="661" alt="MaleFashion Screenshot" src="https://github.com/user-attachments/assets/d94f4e8d-2684-41e3-b7fc-7fe8a371bfca" />

</div>


### 🔐 Forgot & Reset Password


Allows users to recover their account by requesting a password reset link through email and securely reset their password.

<div align="center">

<img width="545" height="518" alt="Forgot Password" src="https://github.com/user-attachments/assets/daae58ac-6a3d-436c-8a8e-e185df020a7b" />

<img width="482" height="567" alt="Password Reset Email" src="https://github.com/user-attachments/assets/bbfbc41b-1155-4db1-8d39-838ba1e23d6c" />

<img width="856" height="622" alt="Reset Password" src="https://github.com/user-attachments/assets/ea906082-67a2-434e-adf2-d501abe40073" />

<img width="541" height="645" alt="Password Reset Confirmation" src="https://github.com/user-attachments/assets/c425abae-13cb-4256-838e-2096083bdf6a" />

</div>






### 🏠 Storefront — Home Page


![MaleFashion Screenshot](https://github.com/user-attachments/assets/d273c52d-a754-4920-a992-18bfd5a31f7b)



### 🛍️ Storefront — Shop / Product Listing


<p align="center">
  <img src="https://github.com/user-attachments/assets/abc62a65-3449-4b9e-b3fe-0df562f22059" alt="Screenshot 1">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/25446cb5-301b-45cd-8f4f-f477d24c9ba9" alt="Screenshot 2">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/19e5b67a-9288-40c5-92c0-7a1a23e3c7cf" alt="Screenshot 3">
</p>



### 👕 Product — Product Details & Variants


<p align="center">
  <img src="https://github.com/user-attachments/assets/d6e7ac3b-c699-480a-92c1-9202954e476f" alt="Screenshot 1">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/43d6490e-9bbb-4ace-a84c-b7ac718550f7" alt="Screenshot 2">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/ff69c840-64a5-4328-ae51-4e7ed89091d4" alt="Screenshot 3">
</p>



### ❤️ Customer — Wishlist



<p align="center">
  <img src="https://github.com/user-attachments/assets/53d25e71-f3c4-4128-aa7b-091f650877fd" alt="Screenshot 1">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/e0678370-5d48-4025-a058-ba4564436c77" alt="Screenshot 2">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/1418cb68-e243-439f-9d36-23c471dce151" alt="Screenshot 3">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/5dbc98d6-58e5-4c5d-958b-241d9eaacf93" alt="Screenshot 4">
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/b99095d8-f17f-47fa-9df6-4962e53130c4" alt="Screenshot 5">
</p>



### 🛒 Customer — Shopping Cart


<div align="center">
<img width="1307" height="646" alt="MaleFashion Shopping Cart" src="https://github.com/user-attachments/assets/e0afdf86-7680-45fa-9b0c-e607c3f135c5" />
</div>


### 💳 Customer — Checkout


<div align="center">

<img width="787" height="302" alt="image" src="https://github.com/user-attachments/assets/bf2f0182-b95b-4d58-8f07-f2e8b6a7615c" />

<img width="731" height="297" alt="image" src="https://github.com/user-attachments/assets/e5ec8282-0223-4f2c-9b7b-bd566a1537ba" />

<img width="375" height="570" alt="Screenshot 2026-09-30 103300" src="https://github.com/user-attachments/assets/8829a6f1-2e8c-4c9c-bd53-ceedd5b3dc0b" />

</div>



### 📦 Customer — Place Order


<div align="center">

<img width="282" height="578" alt="Screenshot 2026-09-30 103521" src="https://github.com/user-attachments/assets/0ad42854-e5ab-4918-b736-ae29334383a8" />

<img width="417" height="676" alt="Screenshot 2026-09-30 103616" src="https://github.com/user-attachments/assets/970f586c-47cd-4000-8561-432b9c4180cc" />

</div>


### 📦 Customer — View Orders


<div align="center">

<img width="1512" height="546" alt="Screenshot 2026-09-30 104916" src="https://github.com/user-attachments/assets/f613366f-cd64-44c5-b0b1-e24543d8cce4" />

<img width="1533" height="462" alt="Screenshot 2026-09-30 105204" src="https://github.com/user-attachments/assets/d365cf67-74e6-419a-8be0-5fdf7dd9caee" />

<img width="1496" height="681" alt="Screenshot 2026-09-30 105239" src="https://github.com/user-attachments/assets/e552937f-438e-433d-8986-545120a91602" />

<img width="1506" height="440" alt="Screenshot 2026-09-30 105312" src="https://github.com/user-attachments/assets/9ccc4a8e-c79c-4a9c-91b4-4524778f96b7" />

</div>






### 📊 Admin — Dashboard


<div align="center">

<img width="1484" height="549" alt="image" src="https://github.com/user-attachments/assets/a56da057-db5f-4ce1-8260-722a2de49c2d" />

</div>



### 👥 Admin — User Accounts & Access Control


<div align="center">

<img width="1421" height="557" alt="image" src="https://github.com/user-attachments/assets/12314d58-76fb-4b8b-86e6-896f8408ad2e" />

</div>


### 📂 Category Management & CRUD Operations


<div align="center">

<img width="1382" height="465" alt="Screenshot 2026-09-30 113240" src="https://github.com/user-attachments/assets/8ed39cab-f033-4633-becf-e422dce92c5d" />

<img width="692" height="296" alt="Screenshot 2026-09-30 113302" src="https://github.com/user-attachments/assets/2e4979c3-ca85-4b70-8df1-bb151b50b27b" />

<img width="1401" height="381" alt="Screenshot 2026-09-30 113347" src="https://github.com/user-attachments/assets/5fa085dc-7927-437a-9d58-0790f5a4eb9a" />

<img width="557" height="216" alt="Screenshot 2026-09-30 113412" src="https://github.com/user-attachments/assets/d723d155-b30e-40d2-b615-2fe268010f20" />

</div>


### 🏷️ Product Management & Details

## 🛍️ Product Management


The **Product Management** module allows administrators to manage products, including creating, updating, viewing, and managing product information and variants.

<div align="center">

<img src="https://github.com/user-attachments/assets/2c0950f8-ce04-4fc8-8d11-bd17c251a1cb" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/f5196b6b-b8dc-4531-8d5c-0cc416b50ca8" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/09e20940-2005-4b86-ab62-b9f78b8c7141" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/9015cdc4-2bef-475b-afbb-197800b89f2b" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/5868ddfa-22bb-4a7b-82e6-542c2e818086" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/cf215313-49b4-4f3b-9b61-b15d3e78edd7" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/46629abe-0541-4d28-ace5-d0fb34cc69a0" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/42936f03-bc49-44d0-825f-1ae78fbe4864" width="90%" />

</div>

---


## 📦 Product Details


The **Product Details** section provides detailed information about individual products, including product information, pricing, variants, sizes, colors, and other product-specific data.

<div align="center">

<img src="https://github.com/user-attachments/assets/a848445d-679b-4d69-81c2-63e5acecf102" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/aa705f62-883e-4d62-b3dc-032ac72c665d" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/5ee28956-e203-4d18-87b1-fa26cae465d7" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/1e53264f-22be-4f9f-ac48-b2f24e9f99e6" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/30411504-2c4b-469d-b7e5-d5d95fba6452" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/e56fa004-450f-418f-8479-f5bf5a7d3be4" width="90%" />

<br><br>

<img src="https://github.com/user-attachments/assets/95590dd1-83b6-4fa0-b2db-714f058259b5" width="90%" />

</div>


### 📦 Inventory Management & CRUD Operations


<img width="1402" height="622" alt="Screenshot 2026-09-30 115401" src="https://github.com/user-attachments/assets/564daa4c-b176-462c-8dca-98c08f5b8408" />
<img width="930" height="362" alt="Screenshot 2026-09-30 115424" src="https://github.com/user-attachments/assets/db73384a-df81-43d5-b388-33b6fc8e4a44" />
<img width="932" height="340" alt="Screenshot 2026-09-30 115450" src="https://github.com/user-attachments/assets/e66ea74c-a2f6-4882-8b1e-796b46e29c06" />
<img width="552" height="216" alt="Screenshot 2026-09-30 115335" src="https://github.com/user-attachments/assets/264d8e06-50b7-4082-a9e9-3037c0de1b61" />


### 🏷️ Discount Management — CRUD Operations


<img width="1392" height="357" alt="Screenshot 2026-09-30 115921" src="https://github.com/user-attachments/assets/c0d836f5-7d27-4845-85a4-73b573798cde" />
<img width="1417" height="548" alt="Screenshot 2026-09-30 115951" src="https://github.com/user-attachments/assets/08f5702f-e7f0-45a5-9e2f-5c40fb39a4ec" />
<img width="1438" height="530" alt="Screenshot 2026-09-30 120026" src="https://github.com/user-attachments/assets/ae41e3f5-0701-4516-a8e1-98309688e5b5" />
<img width="563" height="212" alt="Screenshot 2026-09-30 120049" src="https://github.com/user-attachments/assets/2d2810ca-1e00-40f0-9b5e-0b965a433635" />


### 📋 Order Management — View & Details


#### 📦 Order Overview


Provides administrators with a centralized view of customer orders, including order status, customer information, payment status, and order date.

<img width="900" alt="Order Management Overview" src="https://github.com/user-attachments/assets/83ea5719-ad43-4fae-9ca7-03cd867550ec" />


#### 🔎 Order Details


Provides detailed information about an individual order, including customer information, ordered products, quantities, pricing, payment details, and order information.

<img width="900" alt="Order Details" src="https://github.com/user-attachments/assets/53de4b42-66ef-4a44-83a9-55ee7ea91385" />

<img width="900" alt="Order Details — Additional Information" src="https://github.com/user-attachments/assets/9c034869-8eab-4773-9dfb-13518e3231ca" />




## 🏗️ Architecture

MaleFashion follows **Clean Architecture** principles combined with concepts from **Domain-Driven Design (DDD)**.

```text
                                      ┌─────────────────────────┐
                    │    MaleFashion.Web      │
                    │                         │
                    │ Controllers / Areas     │
                    │ Views / Presentation    │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ MaleFashion.Application │
                    │                         │
                    │ Commands / Queries      │
                    │ Handlers / DTOs         │
                    │ Validators / Services   │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │   MaleFashion.Domain    │
                    │                         │
                    │ Entities / Aggregates   │
                    │ Value Objects / Rules   │
                    │ Domain Contracts        │
                    └─────────────────────────┘
                                 
                                 
                               
                                 
                    ┌────────────────────────┐
                    │MaleFashion.Infrastructure│
                    │                         │
                    │ EF Core / Repositories  │
                    │ Unit of Work / Identity │
                    │ Email Services          │
                    │ reCAPTCHA / Persistence │
                    └─────────────────────────┘

   MaleFashion.Web ─────► MaleFashion.Application ─────► MaleFashion.Domain
                              ▲
                              │
                              │ implements
                              │
                       MaleFashion.Infrastructure


## 🧱 Layer Responsibilities

MaleFashion follows a **Clean Architecture** approach where each layer has a clearly defined responsibility.

| Layer              | Responsibility                                                                         |
| ------------------ | -------------------------------------------------------------------------------------- |
| **Domain**         | Core business concepts, entities, aggregates, business rules, and abstractions         |
| **Application**    | Use cases, CQRS commands/queries, handlers, DTOs, validation, and application services |
| **Infrastructure** | Database, EF Core, repositories, Identity, email, and external services                |
| **Web**            | Controllers, Areas, Views, authentication flow, and presentation                       |

### 🔄 Dependency Direction

The primary dependency direction is:

```text
        Web
         │
         ▼
    Application
         │
         ▼
       Domain

Infrastructure
      │
      └── Implements Domain/Application Abstractions
```

This structure keeps the core business logic independent from frameworks, databases, and external services.

---

# 🧠 Domain-Driven Design (DDD)

MaleFashion also incorporates important **Domain-Driven Design (DDD)** concepts.

DDD focuses on modeling the application around **real-world business concepts, relationships, and business rules**, rather than allowing database or UI concerns to control the design.

## 📐 DDD Concepts Used

```text
                    E-Commerce Domain
                           │
             ┌─────────────┼─────────────┐
             │             │             │
          Product         Cart          Order
             │             │             │
          Variant       CartItem      OrderItem
             │                           │
         Inventory                     Payment
```

### 🧩 Important Domain Concepts

* **Entities**
* **Aggregate Roots**
* **Domain Relationships**
* **Business Rules**
* **Repository Abstractions**
* **Domain Contracts**

---

## 🏷️ Important Domain Entities

The main e-commerce domain entities include:

* `Product`
* `ProductVariant`
* `Inventory`
* `Cart`
* `CartItem`
* `Wishlist`
* `Discount`
* `Order`
* `OrderItem`
* `Payment`

---

## 🛍️ Example Domain Relationship

```text
Product
   │
   ├── ProductVariant
   │      ├── Size
   │      ├── Color
   │      └── SKU
   │
   └── Inventory
          └── Stock
```

This structure allows MaleFashion to represent real-world e-commerce concepts and their relationships directly within the application.

---

## 💡 Why DDD?

DDD helps MaleFashion model real-world business concepts and rules directly in the application.

For example, a `Product` can have multiple `ProductVariant` objects. Each variant can have its own:

* Size
* Color
* SKU
* Inventory quantity

This makes it possible to manage different product combinations independently.

### ✅ Benefits of DDD

* Business logic becomes easier to understand
* Domain rules are separated from infrastructure concerns
* Complex business behavior can be modeled explicitly
* Entities represent real-world business concepts
* Infrastructure changes have less impact on business logic
* Improves maintainability and extensibility
* Provides a strong foundation for future business requirements

---

# 🔄 E-Commerce Workflow

The main customer journey in **MaleFashion** is:

```text
Registration / Login
        ↓
    Browse Shop
        ↓
  Product Details
        ↓
 Select Size / Color
        ↓
   ┌────┴─────┐
   ↓          ↓
Add to Cart  Wishlist
   ↓
  Cart
   ↓
Apply Coupon / Discount
   ↓
 Checkout
   ↓
Payment Selection
   ↓
Payment Simulation
   ↓
 Place Order
   ↓
Order + Order Items
   ↓
  Payment
```

---

# 🛒 Product & Inventory Management

Products in **MaleFashion** support multiple variants, allowing different combinations of **size, color, and SKU** for the same product.

## 📦 Product Variants

```text
Product
   │
   ├── Size
   ├── Color
   └── SKU
```

Inventory is maintained at the **Product Variant** level.

```text
Product
   ↓
Product Variant
   ↓
Inventory
   ↓
Available Stock
```

This allows each variant to maintain its own stock quantity.

### 👕 Example

For a T-Shirt, each size and color combination can have its own inventory:

```text
T-Shirt
│
├── Small / Black   → 10
├── Medium / Black  → 15
├── Large / Black   → 8
├── Small / White   → 12
└── Medium / White  → 20
```

For example, when a customer purchases a **Medium / Black** T-Shirt:

```text
Medium / Black Stock
        ↓
      15 → 14
```

Only the stock of that specific product variant is decreased. Other sizes and colors remain unchanged.

---

# 💳 Payment Simulation

MaleFashion does **not process real financial transactions**.

Instead, payment functionality is implemented as a **simulation** for demonstration and project requirements.

## 💰 Supported Payment Methods

* **Cash on Delivery**
* **bKash Simulation**

## 🔄 Payment Workflow

```text
Customer
   ↓
Checkout
   ↓
Select Payment Method
   ↓
Enter Payment Information
   ↓
Validate Information
   ↓
Payment Accepted
   ↓
Create Order
   ↓
Save Payment Information
```

> **Note:** No real money is transferred through the payment simulation. The payment process is implemented only for demonstration and application workflow purposes.

## 🔐 Authentication & Authorization

MaleFashion uses **ASP.NET Core Identity** for authentication and authorization.

### 👥 User Roles

The application includes the following primary roles:

* **Admin**
* **Member**

Administrative functionality is protected using **role-based authorization**.

```csharp
[Authorize(Roles = "Admin")]
```

This ensures that only users with the **Admin** role can access protected administration features.

### 🔄 Authorization Flow

```text
User
  ↓
Login
  ↓
ASP.NET Core Identity
  ↓
Authentication
  ↓
Role Verification
  ↓
Admin / Member
  ↓
Access Protected Resource
```

---

## 🛡️ Google reCAPTCHA v3

**Google reCAPTCHA v3** is integrated to help protect public forms from automated submissions and malicious activity.

### 📋 Protected Forms

reCAPTCHA can be applied to:

* Login
* Registration
* Forgot Password
* Contact Us

### 🔄 reCAPTCHA Flow

```text
User
  ↓
Submit Form
  ↓
Generate reCAPTCHA Token
  ↓
Send Token to Server
  ↓
Server Verification
  ↓
Google reCAPTCHA
  ↓
Validation Result
  ↓
┌───────────────────────┐
│                       │
│  Valid                │  Invalid
│    ↓                  │    ↓
│ Continue Processing   │ Reject Request
│                       │
└───────────────────────┘
```

The server verifies the reCAPTCHA token before continuing with the requested operation.

This provides an additional layer of protection instead of relying only on client-side validation.


## 📧 Email Services

Email functionality is abstracted through application-level contracts and implemented in the **Infrastructure** layer.

Typical use cases include:

* Account-related emails
* Password reset emails

SMTP configuration is supplied through configuration or environment variables rather than being hard-coded.

```text
Application
     ↓
IEmailService
     ↓
Infrastructure
     ↓
SMTP Provider
     ↓
Email
```

This allows the SMTP provider to be changed without modifying application business logic.

---

## 🔄 CQRS

MaleFashion uses **CQRS (Command Query Responsibility Segregation)** to separate state-changing operations from data retrieval operations.

```text
                 Application
                      │
              ┌───────┴───────┐
              │               │
           Command          Query
              │               │
        Change State       Read State
              │               │
           Handler          Handler
```

### Commands

Commands perform **state-changing operations**.

Examples:

* `AddToCartCommand`
* `AddOrderCommand`
* `CreateProductCommand`
* `UpdateProductCommand`
* `DeleteProductCommand`

### Queries

Queries retrieve data **without changing application state**.

Examples:

* `GetCartQuery`
* `GetProductQuery`
* `GetCategoriesQuery`
* `GetOrdersQuery`

### Benefits

* Clear separation between reads and writes
* Easier testing
* Better organization of application use cases
* Reduced controller complexity
* Easier future optimization of read and write operations

---

## 🧩 Mediator Design Pattern

MaleFashion uses the **Mediator behavioral design pattern**.

The Mediator pattern reduces direct communication between controllers and individual application handlers.

Instead of a controller directly depending on multiple services:

```text
Controller
   │
   ├── ProductService
   ├── CartService
   ├── OrderService
   ├── DiscountService
   └── EmailService
```

The controller communicates through a mediator:

```text
Controller
    │
    ▼
 Mediator
    │
    ├── Command Handler
    └── Query Handler
```

This reduces coupling between the **presentation layer** and application operations.

---

## 🚀 Cortex.Mediator

The Mediator pattern is implemented using **Cortex.Mediator**.

A controller can send an application request through the mediator:

```csharp
var result = await _mediator.SendCommandAsync(command);
```

or:

```csharp
var result = await _mediator.SendQueryAsync(query);
```

The mediator locates and executes the appropriate handler.

### Request Flow

```text
Controller
    │
    │ SendCommandAsync()
    ▼
Cortex.Mediator
    │
    ▼
Command Handler
    │
    ▼
Repository / Unit of Work
    │
    ▼
Database
```

### What Cortex.Mediator Achieves

Cortex.Mediator provides the infrastructure required to implement the Mediator pattern and connect:

```text
Request
   ↓
Mediator
   ↓
Handler
```

This allows controllers to focus primarily on **HTTP and presentation concerns** instead of implementing business operations.

### Benefits

* Reduces coupling
* Keeps controllers thin
* Centralizes application request handling
* Works naturally with CQRS
* Improves testability
* Makes application use cases easier to locate
* Supports cleaner separation of responsibilities

---

## 🗺️ AutoMapper

The application uses **AutoMapper** for object-to-object mapping between different application models.

A common mapping flow is:

```text
Entity
  ↓
AutoMapper
  ↓
DTO / ViewModel
```

For example:

```text
Product Entity
      ↓
   Mapping
      ↓
ProductDto
```

Instead of manually assigning every property:

```csharp
var dto = new ProductDto
{
    Id = product.Id,
    ProductName = product.ProductName,
    Branding = product.Branding,
    ProductPrize = product.ProductPrize
};
```

mapping configuration can define how objects are transformed.

### What AutoMapper Achieves

AutoMapper reduces repetitive mapping code between:

* Entities
* DTOs
* ViewModels
* Application models

### Benefits

* Reduces boilerplate mapping code
* Keeps mapping rules centralized
* Makes handlers and controllers cleaner
* Separates domain entities from presentation models
* Reduces accidental exposure of entity objects

### Mapping Flow

```text
Database Entity
      │
      ▼
Application Mapping
      │
      ▼
     DTO
      │
      ▼
    View
```

The UI does not need to work directly with database entities, helping maintain a clean separation between the **data access, application, and presentation layers**.

## 🗄️ Stored Procedures

MaleFashion also uses **SQL Server Stored Procedures** for selected database operations, particularly data retrieval and server-side querying scenarios.

Examples include:

* `GetCategories`
* `GetPagedCategories`
* `GetDiscounts`
* `GetPagedDiscounts`

A stored procedure encapsulates SQL logic inside the database.

```text
Application
     ↓
Repository
     ↓
Stored Procedure
     ↓
SQL Server
     ↓
Result
```

### Example

A stored procedure can encapsulate a paginated category query:

```sql
EXEC dbo.GetPagedCategories
    @PageNumber = 1,
    @PageSize = 10,
    @OrderBy = 'CategoryName';
```

### What Stored Procedures Achieve

Stored procedures allow frequently used or database-intensive operations to be defined and executed on the SQL Server side.

They can encapsulate:

* Filtering
* Sorting
* Pagination
* Joins
* Aggregations
* Complex SQL operations

### Benefits

* Centralizes selected SQL logic
* Can simplify complex database queries
* Supports server-side pagination
* Reduces repeated SQL statements
* Provides a consistent database operation interface
* Can be useful for database-heavy reporting or querying operations

> **Note:** Stored procedures are used selectively. Entity Framework Core remains the primary ORM and data-access technology.

---

## 🗃️ Repository Pattern

The application uses the **Repository Pattern** to abstract data access.

```text
Application
     ↓
Repository Interface
     ↓
Infrastructure Repository
     ↓
Entity Framework Core
     ↓
SQL Server
```

For example:

```text
IProductRepository
       ↓
ProductRepository
       ↓
ApplicationDbContext
```

The application layer depends on repository abstractions, while the Infrastructure layer provides their implementations.

### Benefits

* Separates data-access logic
* Reduces direct database dependencies
* Improves testability
* Provides a consistent data-access abstraction
* Keeps application logic independent from EF Core implementation details

---

## 🔄 Unit of Work

The **Unit of Work** pattern coordinates multiple database operations as a single logical transaction.

For example, placing an order may require multiple related operations:

```text
Order
  +
Order Items
  +
Payment
  +
Inventory Update
```

These operations can be coordinated through a Unit of Work.

```text
                 Unit of Work
                      │
          ┌───────────┼───────────┐
          ▼           ▼           ▼
       Order      OrderItem    Payment
          │           │           │
          └───────────┼───────────┘
                      ▼
                    Commit
```

The Unit of Work provides a central point for coordinating related repository operations and transaction management.

### Benefits

* Coordinates related database changes
* Helps maintain data consistency
* Reduces partial database updates
* Provides a central transaction boundary
* Simplifies coordination between multiple repositories

---

## 🧪 Unit Testing

The project includes **application-layer unit tests** covering important business operations.

### Test Structure

```text
tests/
└── MaleFashion.Application.UnitTests/
    └── Features/
        ├── Carts/
        ├── Categories/
        ├── ContactMessages/
        ├── Discounts/
        ├── Inventories/
        ├── Orders/
        ├── Products/
        └── Wishlists/
```

### Testing Technologies

The project uses:

* **NUnit** — Testing framework
* **Moq** — Mocking framework
* **Shouldly** — Assertion library

The tests focus on application behavior independently from the UI and external infrastructure.

### Testing Approach

```text
Application Handler
       │
       ├── Mock Repository
       ├── Mock Dependencies
       │
       ▼
   Business Logic
       │
       ▼
     Assert
```

This allows application logic to be tested without requiring the actual database, web UI, or other external infrastructure.

### Benefits

* Detects regressions
* Validates business logic
* Improves confidence during refactoring
* Makes application behavior easier to verify
* Supports maintainable development
* Helps identify defects early

## 🗄️ Database

MaleFashion uses **SQL Server** as its relational database and **Entity Framework Core** as the primary ORM and data-access technology.

### Database Entities

The database contains entities related to:

- 👤 **Users**
- 🛡️ **Roles**
- 🛍️ **Products**
- 📂 **Categories**
- 🎨 **Product Variants**
- 📦 **Inventory**
- 🛒 **Cart**
- 🛒 **Cart Items**
- ❤️ **Wishlist**
- 🏷️ **Discounts**
- 📋 **Orders**
- 📦 **Order Items**
- 💳 **Payments**

### Entity Relationships

Entity relationships and delete behaviors are configured using **Entity Framework Core**.

## 🗄️ Database

MaleFashion uses **SQL Server** as its relational database and **Entity Framework Core** as the primary ORM and data-access technology.

## 🐳 Docker

MaleFashion is containerized using **Docker** and **Docker Compose** to provide a consistent and reproducible application environment.

### Docker Architecture

The application uses a **multi-stage .NET Docker build**:

```text
Dockerfile
    │
    ├── Restore
    ├── Build
    ├── Publish
    └── Runtime
```

### Docker Compose

Docker Compose simplifies application startup, configuration, environment variables, port mapping, and persistent volumes.

Start the application with:

```bash
docker compose up -d --build
```

The application is available at:

**http://localhost:8000**

### Docker Compose Configuration

```yaml
services:
  web:
    build:
      context: .
      dockerfile: Src/MaleFashion.Web/Dockerfile

    image: malefashion.web

    env_file:
      - Src/MaleFashion.Web/web.env

    environment:
      ASPNETCORE_URLS: http://+:80

    volumes:
      - malefashion-data:/app/Logs/

    ports:
      - "8000:80"

    entrypoint: ["dotnet", "MaleFashion.Web.dll"]

volumes:
  malefashion-data:
```

### Dockerfile

The Dockerfile uses a **multi-stage build** to restore, build, publish, and run the ASP.NET Core application.

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

RUN apt update && apt install -y nodejs

COPY ["Src/MaleFashion.Web/MaleFashion.Web.csproj", "MaleFashion.Web/"]
COPY ["Src/MaleFashion.Domain/MaleFashion.Domain.csproj", "MaleFashion.Domain/"]
COPY ["Src/MaleFashion.Application/MaleFashion.Application.csproj", "MaleFashion.Application/"]
COPY ["Src/MaleFashion.Infrastructure/MaleFashion.Infrastructure.csproj", "MaleFashion.Infrastructure/"]

RUN dotnet restore "MaleFashion.Web/MaleFashion.Web.csproj"

COPY ./Src/ .

WORKDIR "/src/MaleFashion.Web"

RUN dotnet build "MaleFashion.Web.csproj" -c Release -o /app

FROM build AS publish

RUN dotnet publish "MaleFashion.Web.csproj" -c Release -o /app

FROM build AS final

WORKDIR /app

COPY --from=publish /app .

EXPOSE 80

ENTRYPOINT ["dotnet", "MaleFashion.Web.dll"]
```

### 🔄 Docker Build Flow

```text
Docker Compose
      │
      ▼
   Dockerfile
      │
      ▼
    Restore
      │
      ▼
     Build
      │
      ▼
    Publish
      │
      ▼
  Runtime Image
      │
      ▼
MaleFashion.Web
      │
      ▼
localhost:8000
```

### 💾 Persistent Logs

The application logs are stored using a Docker named volume:

```yaml
volumes:
  - malefashion-data:/app/Logs/
```

This helps preserve application logs independently of the container lifecycle.

### Benefits

* 🧩 Consistent development environment
* 🔁 Reproducible application deployment
* 📦 Isolated runtime environment
* ⚡ Easier application setup
* 🔧 Simplified configuration
* 🚀 Streamlined deployment workflow
* 💾 Persistent application logs




## 📊 Design Patterns & Practices

MaleFashion applies a combination of architectural patterns, design patterns, frameworks, and development practices to improve maintainability, scalability, testability, and separation of responsibilities.

| Pattern / Technology      | What It Achieves                                                       | Main Benefit                       |
| ------------------------- | ---------------------------------------------------------------------- | ---------------------------------- |
| **Clean Architecture**    | Separates presentation, application, domain, and infrastructure layers | Maintainability                    |
| **Domain-Driven Design**  | Models software around business concepts and rules                     | Strong domain modeling             |
| **CQRS**                  | Separates commands from queries                                        | Clear application responsibilities |
| **Mediator Pattern**      | Decouples request senders from handlers                                | Loose coupling                     |
| **Cortex.Mediator**       | Implements mediator-based request handling                             | Cleaner controllers                |
| **Repository Pattern**    | Abstracts data-access operations                                       | Testability                        |
| **Unit of Work**          | Coordinates related database operations                                | Consistency                        |
| **Dependency Injection**  | Provides dependencies through abstractions                             | Loose coupling                     |
| **AutoMapper**            | Maps entities to DTOs and ViewModels                                   | Less boilerplate                   |
| **DTOs**                  | Transfers controlled application data                                  | Encapsulation                      |
| **Stored Procedures**     | Encapsulates selected SQL operations                                   | Centralized database logic         |
| **ASP.NET Core Identity** | Handles authentication and authorization                               | Secure user management             |
| **Entity Framework Core** | Provides ORM and database access                                       | Productivity                       |
| **Unit Testing**          | Validates application behavior                                         | Reliability                        |
| **Serilog**               | Provides structured application logging                                | Observability                      |
| **Docker**                | Containerizes the application                                          | Deployment consistency             |
| **Docker Compose**        | Coordinates container configuration and application setup              | Easier environment setup           |


## 🧰 Technology Stack

### Backend

* **C#**
* **ASP.NET Core MVC**
* **Entity Framework Core**
* **ASP.NET Core Identity**
* **Clean Architecture**
* **Domain-Driven Design (DDD)**
* **CQRS**
* **Cortex.Mediator**
* **Mediator Design Pattern**
* **Repository Pattern**
* **Unit of Work Pattern**
* **Dependency Injection**
* **Mapster**

### Database

* **SQL Server**
* **Entity Framework Core**
* **LINQ**

### Frontend

* **HTML5**
* **CSS3**
* **JavaScript**
* **Bootstrap**
* **jQuery**
* **DataTables**
* **Razor Views**

### Authentication & Security

* **ASP.NET Core Identity**
* **Role-Based Authorization**
* **Google reCAPTCHA v3**
* **Password Reset & Email Verification**

### External Services

* **SMTP / Email Service**
* **Google reCAPTCHA v3**

### Logging & Infrastructure

* **Serilog**
* **Docker**
* **Docker Compose**

### Testing

* **NUnit**
* **Moq**
* **Shouldly**

### Deployment

* **SmartASP.NET**

## 📂 Solution Structure

```text
MaleFashion
│
├── Src/
│   │
│   ├── MaleFashion.Domain/
│   │   ├── Entities/
│   │   ├── Interfaces/
│   │   ├── Enums/
│   │   └── Common/
│   │
│   ├── MaleFashion.Application/
│   │   ├── Features/
│   │   │   ├── Products/
│   │   │   ├── Categories/
│   │   │   ├── Carts/
│   │   │   ├── Orders/
│   │   │   ├── Discounts/
│   │   │   └── Wishlists/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   └── Services/
│   │
│   ├── MaleFashion.Infrastructure/
│   │   ├── Persistence/
│   │   ├── Repositories/
│   │   ├── Identity/
│   │   ├── Services/
│   │   └── Configurations/
│   │
│   └── MaleFashion.Web/
│       ├── Areas/
│       │   ├── Admin/
│       │   └── Customer/
│       ├── Controllers/
│       ├── Views/
│       └── wwwroot/
|       └── Dockerfile
│
├── tests/
│   └── MaleFashion.Application.UnitTests/
│
│
└── docker-compose.yml
```

---

## 🔄 Complete System Overview

```text
                         MALEFASHION
                              │
              ┌───────────────┴───────────────┐
              │                               │
            ADMIN                         CUSTOMER
              │                               │
              ▼                               ▼
    Admin Authentication              Registration / Login
              │                               │
              ▼                               ▼
       Admin Dashboard                    Storefront
              │                               │
       ┌──────┼──────┐                        ▼
       │      │      │                    Products
       ▼      ▼      ▼                        │
   Products Categories Inventory               ▼
       │      │      │                  Product Details
       │      │      │                        │
       └──────┼──────┘                        ▼
              │                           Add to Cart
              │                               │
              │                               ▼
              │                             Cart
              │                               │
              │                               ▼
              │                           Checkout
              │                               │
              │                         ┌─────┴─────┐
              │                         │           │
              │                        COD       bKash
              │                                  Simulation
              │                         │           │
              │                         └─────┬─────┘
              │                               │
              │                               ▼
              │                             Order
              │                               │
              └───────────────────────────────┘
                                              │
                                              ▼
                                      Order Management
```

---

## 🔗 Internal Application Flow

The overall technical request flow follows **ASP.NET Core MVC → Mediator → Application → Domain → Infrastructure**:

```text
                         Browser
                            │
                            ▼
                ASP.NET Core MVC Controller
                            │
                            ▼
                     Cortex.Mediator
                            │
                  ┌─────────┴─────────┐
                  │                   │
                  ▼                   ▼
               Command              Query
                  │                   │
                  ▼                   ▼
          Command Handler       Query Handler
                  │                   │
                  └─────────┬─────────┘
                            ▼
                   Application Logic
                            │
                            ▼
                   Domain Abstractions
                            │
                            ▼
                     Infrastructure
                            │
                 ┌──────────┴──────────┐
                 │                     │
                 ▼                     ▼
             Repository         External Services
                 │
                 ▼
        Entity Framework Core
                 │
                 ▼
             SQL Server
```

### 🏗️ Architecture Responsibility

| Layer              | Responsibility                                          |
| ------------------ | ------------------------------------------------------- |
| **Domain**         | Business entities, enums, and core abstractions         |
| **Application**    | Use cases, CQRS handlers, DTOs, and application logic   |
| **Infrastructure** | Database, repositories, Identity, and external services |
| **Web**            | MVC controllers, Razor views, Areas, and static files   |

This architecture keeps responsibilities separated and makes the application easier to **maintain, test, and extend**.


## 🚀 Deployment

MaleFashion is prepared for deployment using a **containerized ASP.NET Core environment**.

### ☁️ Deployment Target

**SmartASP.NET**

### ⚙️ Production Configuration

Production settings are separated from development configuration using **environment-specific configuration** and **environment variables**.

Deployment configuration includes:

* **Production connection strings**
* **SMTP / Email configuration**
* **Google reCAPTCHA configuration**
* **Application secrets**
* **Logging configuration**
* **Environment-specific settings**

### 🔐 Security

Sensitive credentials and application secrets are **not hard-coded** into the source code. Production-specific values should be supplied through environment variables or secure configuration mechanisms.

This approach helps keep sensitive information separate from the application source code while providing a consistent deployment environment.


---

<div align="center">

### 🛍️ MaleFashion

**A Full-Stack ASP.NET Core E-Commerce Application**

<br>

© 2026 **Tushar Basak** · All Rights Reserved.

<sub>Built with ASP.NET Core MVC • C# • Clean Architecture • DDD • CQRS</sub>

</div>



