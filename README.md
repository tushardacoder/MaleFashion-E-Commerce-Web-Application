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
