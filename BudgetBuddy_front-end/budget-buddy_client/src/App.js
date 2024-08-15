import { Route, Routes } from "react-router-dom";
import { AuthProvider } from "./store";
import "./App.css";
import SignUpSignIn from "./pages/SignUp";
import React from "react";
import Dashboard from "./pages/Dashboard";
import ProtectedRoute from "./utils/protectedRoute";

function App() {
  return (
      <Routes>
        <Route path="/" element={<SignUpSignIn />} />
        <Route
          path="/dashboard"
          element={<ProtectedRoute element={<Dashboard />} />}
        />
      </Routes>
  );
}

export default App;
