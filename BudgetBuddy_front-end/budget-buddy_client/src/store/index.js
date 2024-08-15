import { createContext, useEffect } from "react";
import { api } from "../api";
import { useNavigate } from "react-router-dom";
import { handleApiErrors } from "../utils/errorHandler";
import { useDispatch, useSelector } from "react-redux";

import { toast } from "react-toastify";

const login = "login";
const logout = "logout";

const loginAction = (user, id, token, refreshToken) => ({
  type: login,
  payload: { user, id, token, refreshToken },
});

const logoutAction = () => ({
  type: logout,
});

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { user, id, token, refreshToken, loading } = useSelector(
    (state) => state.auth
  );

  const signUp = async (userData) => {
    console.log(process.env);
    try {
      const response = await api.auth.signUp(userData);
      console.log("SignUp Response:", response);
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };

  const login = async (credentials) => {
    try {
      const response = await api.auth.login(credentials);
      console.log("Signin Response:", response);
      dispatch(
        loginAction(
          response.data.data,
          response.data.data.id,
          response.data.data.token,
          response.data.data.refreshToken
        )
      );
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };

  const logout = () => {
    toast.success("Hope to see you again");
    navigate("/");
    dispatch(logoutAction());
  };

  useEffect(() => {}, []);

  return (
    <AuthContext.Provider
      value={{ user, id, token, refreshToken, loading, signUp, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};
