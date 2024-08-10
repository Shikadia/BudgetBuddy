# BudgetBuddy Backend

**BudgetBuddy Backend** is the server-side component of the BudgetBuddy application. It provides API endpoints for managing expenses, income, budgets, user accounts, and more. The backend is built using C# and ASP.NET Core.

## Features

- **User Management**: Create, update, delete, and authenticate users.
- **Expense Tracking**: Record and manage expenses.
- **Income Management**: Track and manage income records.
- **Address Management**: Manage user addresses.
- **Category Management**: Organize expenses into categories.

## Technologies Used

- **Framework**: ASP.NET Core
- **Language**: C#
- **Database**: SQL Server
- **Authentication**: Identity
- **ORM**: Entity Framework Core

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or preferred database

### Setup

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/budgetbuddy.git
   cd budgetbuddy/backend
   ```

2. **Restore Dependencies**

   ```bash
   dotnet restore
   ```

3. **Configure the Database**

   - Update the connection string in `appsettings.json` to match your database configuration.

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=yourserver;Database=budgetbuddy;User Id=yourusername;Password=yourpassword;"
   }
   ```

4. **Run Migrations**

   ```bash
   dotnet ef database update
   ```

5. **Start the Application**

   ```bash
   dotnet run
   ```

   The backend server will start on `http://localhost:5000` by default.

## API Endpoints

### **User Endpoints**

- **GET /api/users**: Retrieve a list of all users.
- **GET /api/users/{id}**: Retrieve details of a specific user by ID.
- **POST /api/users**: Create a new user.
- **PUT /api/users/{id}**: Update user details by ID.
- **DELETE /api/users/{id}**: Delete a user by ID.
- **POST /api/users/login**: Authenticate a user and return a token.
- **POST /api/users/register**: Register a new user.
- **PUT /api/users/{id}/update-balance**: Update the balance of a specific user by ID.

### **Expense Endpoints**

- **POST /api/expenses**: Create a new expense.
- **PUT /api/expenses/{id}**: Update an existing expense by ID.
- **DELETE /api/expenses/{id}**: Delete an expense by ID.
- **GET /api/expenses/user/{userId}**: Retrieve expenses for a specific user.
- **GET /api/expenses/category/{categoryId}**: Retrieve expenses for a specific category.

### **Category Endpoints**

- **GET /api/categories**: Retrieve a list of all categories.
- **GET /api/categories/{id}**: Retrieve details of a specific category by ID.
- **POST /api/categories**: Create a new category.
- **PUT /api/categories/{id}**: Update an existing category by ID.
- **DELETE /api/categories/{id}**: Delete a category by ID.

### **Income Endpoints**

- **GET /api/incomes**: Retrieve a list of all income records.
- **GET /api/incomes/{id}**: Retrieve details of a specific income record by ID.
- **POST /api/incomes**: Create a new income record.
- **PUT /api/incomes/{id}**: Update an existing income record by ID.
- **DELETE /api/incomes/{id}**: Delete an income record by ID.
- **GET /api/incomes/user/{userId}**: Retrieve income records for a specific user.
- **GET /api/incomes/source/{source}**: Retrieve income records by a specific source.
- **GET /api/incomes/date-range**: Retrieve income records within a specific date range.

### **Address Endpoints**

- **GET /api/addresses**: Retrieve a list of all addresses.
- **GET /api/addresses/{id}**: Retrieve details of a specific address by ID.
- **POST /api/addresses**: Create a new address.
- **PUT /api/addresses/{id}**: Update an existing address by ID.
- **DELETE /api/addresses/{id}**: Delete an address by ID.
- **GET /api/addresses/user/{userId}**: Retrieve all addresses associated with a specific user.

## Database Schema

The application uses the following primary tables:

- **Users**: Stores user information (`Id`, `Firstname`, `Lastname`, `Email`, `Balance`).
- **Expenses**: Records expense details (`Id`, `Description`, `Amount`, `Date`, `CategoryId`, `PaymentMethodId`, `UserId`).
- **Categories**: Defines expense categories (`Id`, `Name`).
- **Incomes**: Tracks income records (`Id`, `Source`, `Amount`, `Date`, `UserId`).
- **Addresses**: Contains user addresses (`Id`, `UserId`, `Street`, `City`, `State`, `PostalCode`, `Country`).

## Running Tests

To run tests, use the following command:

```bash
dotnet test
```

Ensure that you have test projects set up in your solution to run this command successfully.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. **Fork the repository** and create a new branch.
2. **Make your changes** and test thoroughly.
3. **Submit a pull request** with a detailed description of your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
