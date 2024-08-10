# BudgetBuddy Frontend

**BudgetBuddy Frontend** is the client-side component of the BudgetBuddy application. It provides a user-friendly interface for managing expenses, income, budgets, and more. The frontend is built using React.

## Features

- **Expense Tracking**: View and manage your expenses.
- **Income Management**: Track and manage your income records.
- **Budget Management**: Set and monitor budgets.
- **User Accounts**: Register, log in, and manage user profiles.
- **Address Management**: View and update user addresses.
- **Responsive Design**: Accessible and usable on various devices and screen sizes.

## Technologies Used

- **Library**: React
- **State Management**: React Context API / Redux
- **Routing**: React Router
- **HTTP Client**: Axios (or Fetch API)
- **Styling**: CSS Modules / Styled-components / Tailwind CSS

## Installation

### Prerequisites

- [Node.js](https://nodejs.org/) (version 14.x or later)
- [npm](https://www.npmjs.com/) (comes with Node.js)

### Setup

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/budgetbuddy.git
   cd budgetbuddy/frontend
   ```

2. **Install Dependencies**

   ```bash
   npm install
   ```

   This command installs all the dependencies listed in the `package.json` file.

3. **Configure Environment Variables**

   Create a `.env` file in the root of the `frontend` directory and add any necessary environment variables. For example:

   ```
   REACT_APP_API_URL=http://localhost:5000/api
   ```

   Replace `http://localhost:5000/api` with the URL of your backend API.

4. **Run the Application**

   ```bash
   npm start
   ```

   This command starts the development server and opens your React application in the default web browser, typically accessible at `http://localhost:3000`.

## Project Structure

- **`src/`**: Contains the main application code.
  - **`components/`**: Reusable React components.
  - **`pages/`**: Page components representing different views.
  - **`services/`**: API service functions using Axios or Fetch.
  - **`contexts/`**: React Context providers for global state management (if used).
  - **`hooks/`**: Custom React hooks.
  - **`styles/`**: CSS or styled-components for styling the application.
  - **`App.js`**: Main application component.
  - **`index.js`**: Entry point of the React application.

## Routing

- **`/`**: Home page.
- **`/login`**: User login page.
- **`/register`**: User registration page.
- **`/dashboard`**: Main dashboard showing expense, income, and budget summaries.
- **`/expenses`**: Page for viewing and managing expenses.
- **`/income`**: Page for tracking and managing income.
- **`/profile`**: Page for managing user profiles and addresses.

## API Integration

The frontend communicates with the backend via the API. The following endpoints are used:

- **GET /api/expenses**: Retrieve a list of all expenses.
- **POST /api/expenses**: Create a new expense.
- **PUT /api/expenses/{id}**: Update an existing expense.
- **DELETE /api/expenses/{id}**: Delete an expense.
- **GET /api/incomes**: Retrieve a list of all income records.
- **POST /api/incomes**: Create a new income record.
- **PUT /api/incomes/{id}**: Update an existing income record.
- **DELETE /api/incomes/{id}**: Delete an income record.
- **POST /api/users/login**: Authenticate a user and receive a token.
- **POST /api/users/register**: Register a new user.

## Running Tests

To run tests, use the following command:

```bash
npm test
```

Ensure you have test files and configurations set up in your project.

## Build for Production

To create an optimized production build, use:

```bash
npm run build
```

This command creates a `build` directory with the production-ready static files.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. **Fork the repository** and create a new branch.
2. **Make your changes** and test thoroughly.
3. **Submit a pull request** with a detailed description of your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For questions or feedback, please reach out to [otukachinedu661@gmail.com](mailto:otukachinedu661@gmail.com).
