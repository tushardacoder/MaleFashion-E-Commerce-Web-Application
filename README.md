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




