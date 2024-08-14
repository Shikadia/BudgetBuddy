import { createContext, useEffect, useState } from "react";
import { api } from "../api";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { handleApiErrors } from "../utils/errorHandler";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [refreshToken, setRefreshToken] = useState(null);
  const [loading, setLoading] = useState(null);
  const navigate = useNavigate();

  const signUp = async (userData) => {
    console.log(process.env);
    setLoading(true);
    try {
      const response = await api.auth.signUp(userData);
      console.log("SignUp Response:", response);
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    } finally {
      setLoading(false);
    }
  };

  const login = async (credentials) => {
    setLoading(true);
    try {
      const response = await api.auth.login(credentials);
      console.log("Signin Response:", response);
      setUser(response.data.data.id);
      setToken(response.data.data.token);
      setRefreshToken(response.data.data.refreshToken);
      localStorage.setItem("token", response.data.data.token);
      localStorage.setItem("refreshToken", response.data.data.refreshToken);
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    setRefreshToken(null); // Clear refresh token
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    navigate("/t");
  };

  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    const storedRefreshToken = localStorage.getItem("refreshToken"); 
    if (storedToken) {
      setToken(storedToken);
    }
    if (storedRefreshToken) {
      setRefreshToken(storedRefreshToken); // Set refresh token state
    }
  }, []);

  return (
    <AuthContext.Provider
      value={{ user, token, refreshToken, loading, signUp, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};
