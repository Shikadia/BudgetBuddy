# BudgetBuddy

**BudgetBuddy** is an expense and income management application designed to help users track their spending, manage their budgets, and organize their financial information efficiently. It offers features for recording expenses and income, managing categories, handling user authentication, and generating reports.

## Features

- **Expense Tracking**: Record and categorize your expenses.
- **Income Management**: Track and manage your sources of income.
- **Budget Management**: Set and monitor budgets for different categories.
- **User Accounts**: Create and manage user profiles.
- **Address Management**: Store and manage user addresses.

## Technologies Used

- **Frontend**: React
- **Backend**: C#
- **Database**: SQL Server (or any other preferred database)
- **Authentication**: ...

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (for the backend)
- [Node.js](https://nodejs.org/) (for the frontend)
- [Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (database)

### Backend Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/budgetbuddy.git
   cd budgetbuddy/backend
   ```

2. **Install dependencies**

   ```bash
   dotnet restore
   ```

3. **Configure the database**

   Update the connection string in `appsettings.json` to match your database configuration.

4. **Run the application**

   ```bash
   dotnet run
   ```

   The backend server will start on `http://localhost:5000` by default.

### Frontend Setup

1. **Navigate to the frontend directory**

   ```bash
   cd ../frontend
   ```

2. **Install dependencies**

   ```bash
   npm install
   ```

3. **Run the application**

   ```bash
   npm start
   ```

   The frontend server will start on `http://localhost:3000` by default.

## API Endpoints

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

### **User Endpoints**

- **GET /api/users**: Retrieve a list of all users.
- **GET /api/users/{id}**: Retrieve details of a specific user by ID.
- **POST /api/users**: Create a new user.
- **PUT /api/users/{id}**: Update user details by ID.
- **DELETE /api/users/{id}**: Delete a user by ID.
- **POST /api/users/login**: Authenticate a user and return a token.
- **POST /api/users/register**: Register a new user.
- **PUT /api/users/{id}/update-balance**: Update the balance of a specific user by ID.

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
- **PaymentMethods**: Stores payment methods (`Id`, `Name`).
- **Budgets**: Manages user budgets (`Id`, `UserId`, `CategoryId`, `Amount`, `StartDate`, `EndDate`).
- **Incomes**: Tracks income records (`Id`, `Source`, `Amount`, `Date`, `UserId`).
- **Addresses**: Contains user addresses (`Id`, `UserId`, `Street`, `City`, `State`, `PostalCode`, `Country`).

## Usage

1. **Register**: Create a new user account using the `/api/users/register` endpoint.
2. **Login**: Authenticate and obtain a token using the `/api/users/login` endpoint.
3. **Manage Expenses and Income**: Use the expense and income endpoints to add, update, and view financial records.
4. **Set Budgets**: Create and manage budgets using the budget endpoints.
5. **Manage Addresses**: Add, update, and delete addresses associated with your account.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. **Fork the repository** and create a new branch.
2. **Make your changes** and test thoroughly.
3. **Submit a pull request** with a detailed description of your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
