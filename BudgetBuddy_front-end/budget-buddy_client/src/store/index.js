import { createContext, useEffect } from "react";
import { api } from "../api";
import { useNavigate } from "react-router-dom";
import { handleApiErrors } from "../utils/errorHandler";
import { useDispatch, useSelector } from "react-redux";

import { toast } from "react-toastify";

const login = "login";
const logout = "logout";
const refreshtoken = "refreshtoken";
const addaddress = "addaddress";
const getalladdress = "getalladdress";
const getalltransactions = "getalltransactions";
const addtransactions = "addtransactions";
const googlesignup = "googlesignup";

const loginAction = (user, id, token, refreshToken) => ({
  type: login,
  payload: { user, id, token, refreshToken },
});
const googlesignupactions = (user, id, token, refreshToken) => ({
  type: googlesignup,
  payload: { user, id, token, refreshToken },
});
const refreshtokenAction = (user, id, token, refreshToken) => ({
  type: refreshtoken,
  payload: { user, id, token, refreshToken },
});
const addaddressAction = (address) => ({
  type: addaddress,
  payload: { address },
});
const getalladdressAction = (address) => ({
  type: getalladdress,
  payload: { address },
});
const getalltransactionsAction = (transaction) => ({
  type: getalltransactions,
  payload: { transaction },
});
const addtransactionsAction = (transaction) => ({
  type: addtransactions,
  payload: { transaction },
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
  const { address } = useSelector(
    (state) => state.useraddress
  );
  const { transaction } = useSelector(
    (state) => state.usertransaction
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

  const googleSignInUp = async (userData) => {
    try {
      const response = await api.auth.googleSignInUp(userData);
      console.log("googleSign in Response:", response);
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
  }

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
  const refreshtoken = async (data) => {
    try {
      const response = await api.auth.refreshToken(data);
      console.log("refresh token response:", response);
      dispatch(
        refreshtokenAction(
          response.data.data.newAccessToken,
          response.data.data.newRefreshToken
        )
      );
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const changepassword = async (data) => {
    try {
      const response = await api.auth.changePassword(data);
      console.log("Signin Response:", response);
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const addaddress = async (data) => {
    try {
      const response = await api.user.addAddress(data);
      console.log("add_address :", response);
      dispatch(
        addaddressAction(
          response.data.data,
        )
      );
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const addtransactions = async (credentials) => {
    try {
      const response = await api.user.addTransaction(credentials, 1, 10);
      console.log(" add_transactions:", response);
      dispatch(
        addtransactionsAction(
          response.data.data,
        )
      );
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const getalladdress = async (credentials) => {
    try {
      const response = await api.user.getAllAddress(credentials);
      console.log("get_all_address:", response);
      dispatch(
        getalladdressAction(
          response.data.data,
        )
      );
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const getalltransactions = async () => {
    try {
      const response = await api.user.getAllTransaction(1, 10);
      console.log("get_all_transactions Response:", response);
      dispatch(
        getalltransactionsAction(
          response.data.data,
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

  useEffect(() => {
  
  }, []);

  return (
    <AuthContext.Provider
      value={{ address, transaction, user, id, token, refreshToken, loading, googleSignInUp, signUp, login, logout, refreshtoken, changepassword, addaddress, addtransactions, getalltransactions, getalladdress }}
    >
      {children}
    </AuthContext.Provider>
  );
};
