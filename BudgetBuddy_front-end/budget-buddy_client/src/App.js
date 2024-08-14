import { Route, Routes } from "react-router-dom";
import { AuthProvider } from "./store";
import "./App.css";
import SignUpSignIn from "./pages/SignUp";
import React from "react";

function App() {
  return (
    <AuthProvider>
      <Routes>
        <Route path="/" element={<SignUpSignIn />} />
        <Route path="/t" element={<div>Hello</div>} />
      </Routes>
    </AuthProvider>
  );
}

export default App;
