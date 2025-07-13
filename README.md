
# 🥤 Vending Machine API

A clean, testable, and maintainable ASP.NET Core Web API that simulates the functionality of a vending machine. It allows users to register as **Sellers** or **Buyers** and provides secure endpoints for managing products and performing purchases using coins.

---

## 📌 1. Project Description

This project implements a vending machine system with the following business functionality:

- **Sellers** can:
  - Add, update, and delete products they own.
  - View all products.

- **Buyers** can:
  - Deposit coins (only: 5, 10, 20, 50, 100 cents).
  - Purchase available products (returns total cost, purchased items, and change).
  - Reset their deposit balance.

- **Authentication & Authorization**:
  - Uses **JWT** authentication.
  - Role-based authorization with two roles: `Buyer`, `Seller`.

---

## 🧰 2. Technologies Used

| Layer                     | Technologies                                                                 |
|--------------------------|------------------------------------------------------------------------------|
| **Backend Framework**    | [.NET 6](https://dotnet.microsoft.com/en-us/download)                        |
| **Authentication**       | ASP.NET Core Identity + JWT                                                  |
| **Database**             | SQL Server with Entity Framework Core                                        |
| **Testing**              | xUnit, Moq, AutoMapper                                     |
| **API Docs (optional)**  | Swagger / OpenAPI                                                             |

---

## 🏗️ 3. Architecture & Patterns

The solution follows **Clean Architecture** principles with a multi-layered structure:

### 🔷 Project Layers

```
VendingMachine
├── VendingMachine.Domain            // Entities, Interfaces, Enums
├── VendingMachine.Application       // DTOs, Interfaces, Core Business Logic
├── VendingMachine.Infrastructure    // EF Core, Identity, JWT, Repository Implementations
├── VendingMachine.API               // Controllers, Middleware, DI setup
├── VendingMachine.InterfaceAdapters// Adapters for test abstraction
└── VendingMachine.Tests             // Unit tests for all services via InterfaceAdapters
```

### ✅ Design Patterns & Principles Used

| Pattern / Principle            | Purpose                                                                 |
|-------------------------------|-------------------------------------------------------------------------|
| **Clean Architecture**        | Separation of concerns, testability                                     |
| **Repository Pattern**        | Abstract data access                                                    |
| **Decorator Pattern**         | Logging applied to services without breaking SRP                        |
| **JWT Authentication**        | Secure, stateless access control                                        |
| **Dependency Injection (DI)** | Manage services across layers                                           |
| **Fluent API** (EF Core)      | Configure entity relationships and constraints                          |
| **InterfaceAdapters Layer**   | Isolate test project from business logic                                |
| **Unit Testing (AAA)**        | Arrange-Act-Assert methodology                                          |

---

## 🚀 4. How to Run This Project

### ✅ Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB
- (Optional) [Postman](https://www.postman.com/) or Swagger for API testing

---

### 🛠️ Setup Steps

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Ahmed-Bauomy/VendingMachine-FlapKapTask.git
   cd VendingMachine
   ```

2. **Update `appsettings.json`**:
   In `VendingMachine.API` project, update connection string and JWT settings:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=VendingMachineDb;Trusted_Connection=True;"
   },
   "JwtSettings": {
     "Key": "Better_have_it_somewhere_safer!!",
     "ValidIssuer": "localhost",
     "ValidAudience": "localhost"
   }
   ```

3. **Run EF Core migrations**:
   ```bash
   cd VendingMachine.API
   dotnet ef database update
   ```

4. **Run the API**:
   ```bash
   dotnet run
   ```

5. **Test the API**:
   - Open browser at: `https://localhost:<port>/swagger` (if Swagger enabled)
   - Or use Postman to call endpoints with JWT tokens

---

## 🧪 Running the Tests

```bash
cd VendingMachine.Tests
dotnet test
```

The tests will validate:
- All user, product, and purchase functionalities
- Role-based access
- Mocked service behavior using InterfaceAdapters

---

## 📎 Notes

- The test project does **not reference core layers directly**, but interacts via the `InterfaceAdapters` layer only.
- Logging is applied via **decorator pattern** to both `ProductService` and `PurchaseService`.
- Global exception handling is applied through custom middleware in the API.

---

## 🤝 Contributions

Pull requests and suggestions are welcome!

---

## 🧑‍💻 Author

Ahmed Bayoumy – [https://www.linkedin.com/in/ahmed-bayoumy-80b322177//https://github.com/Ahmed-Bauomy]
