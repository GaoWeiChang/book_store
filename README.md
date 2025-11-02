# Bookstore Management System

A practice project demonstrating large-scale system design - built with .NET Core MVC (.NET 8) using n-layer architecture for enterprise-grade application development.

## Getting Started
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
