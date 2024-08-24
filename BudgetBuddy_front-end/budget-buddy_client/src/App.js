import { Route, Routes } from "react-router-dom";
import "./App.css";
import SignUpSignIn from "./pages/SignUp";
import React from "react";
import Dashboard from "./pages/Dashboard";
import ProtectedRoute from "./utils/protectedRoute";
import ForgotPassword from "./pages/ForgotPassword";
import ResetPassword from "./pages/ResetPassord";
import ConfirmEmail from "./pages/ConfirmEmail";
import ChangePassword from "./pages/ChangePassword";
import AddAddress from "./pages/AddAddress";

function App() {
  return (
      <Routes>
        <Route path="/" element={<SignUpSignIn />} />
        <Route path="/forgotPassword" element={<ForgotPassword/>}/>
        <Route path="/resetPassword" element={<ResetPassword/>}/>
        <Route path="/confirmEmail" element={<ConfirmEmail/>}/>
        <Route
          path="/dashboard"
          element={<ProtectedRoute element={<Dashboard />} />}
        />
        <Route path="/change-password" element={<ProtectedRoute element={<ChangePassword/>}/>}/>
        <Route path="/add-address" element={<AddAddress/>}/>
      </Routes>
  );
}

export default App;
