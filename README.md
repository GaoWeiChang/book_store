# Bookstore Management System 📚

A project that demonstrating large-scale system design - built with .NET Core MVC (.NET 8) using n-layer architecture for enterprise-grade application development.
<img width="1899" height="940" alt="image" src="https://github.com/user-attachments/assets/e422cf73-b8fe-4f30-8589-3f782bdaea32" />

## ✨ Key Features
### 🛒 E-Commerce Features
* Product Management – Create, update, delete, and view products with pricing, images, and stock.
* Category Management – Organize products into categories for easier navigation.
* Shopping Cart – Add/remove items, adjust quantities, and view order summary.
* Secure Online Payment – Integrated Stripe API for safe checkout and payment authorization.

### 📦 Order & Fulfillment Workflow
* Order Management – Admin can view, update, and process customer orders.
* Complete Order Lifecycle:
  * `Payment Pending`
  * `Payment Approved`
  * `Order Processing`
  * `Order Shipped`
  * `Order Cancelled`

### 👤 User & Admin Features
* Authentication & Authorization – Login, register, with Admin/Customer role separation.
* User Management (Admin)
  * Lock / Unlock Accounts to prevent or restore access.
  * Edit Customer Profile – Update customer name and contact information.
* Change User Roles – Promote or demote users between Admin and Customer roles.
* Admin Dashboard – Manage products, categories, users, and orders in one place.

## ⚙️ Architecture 
* N-Layer Architecture – Separation of UI, Business Logic, and Data Access. (`Controller` - `Service` - `Repository`)
* Repository + Unit of Work Patterns – Improve maintainability and testability.

## 🌟 Getting Started
1.) Clone the repository
```
https://github.com/GaoWeiChang/book_store.git
```

2.) `appsettings.json`
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_SERVER_CONNECTIONSTRING"
  },
  "Stripe": {
    "SecretKey": "STRIPE_SECRET_KEY",
    "PublishableKey": "STRIPE_PUBLISHABLE_KEY"
  }
}
```

3.) Add migration
- go to package manager console
- change Default project to `book_store.DataAccess`
<img width="533" height="38" alt="image" src="https://github.com/user-attachments/assets/69df4ec0-0dd0-451f-aa38-6686f9a2bdca" />

4.) Type `Update-Database` in package manager console
