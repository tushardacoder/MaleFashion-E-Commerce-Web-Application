# 🛍️ MaleFashion — E-Commerce Web Application

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

## 🏗️ Architecture

MaleFashion follows **Clean Architecture** principles combined with concepts from **Domain-Driven Design (DDD)**.

```text
                    ┌─────────────────────────┐
                    │    MaleFashion.Web      │
                    │                         │
                    │ Controllers / Areas     │
                    │ Views / Presentation     │
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
                    └────────────▲────────────┘
                                 │
                                 │ Implements
                                 │
                    ┌────────────┴────────────┐
                    │ MaleFashion.Infrastructure │
                    │                         │
                    │ EF Core / Repositories  │
                    │ Unit of Work / Identity │
                    │ Email Services          │
                    │ reCAPTCHA / Persistence │
                    └─────────────────────────┘
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


