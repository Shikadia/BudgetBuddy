import { createContext, useEffect } from "react";
import { api } from "../api";
import { useNavigate } from "react-router-dom";
import { handleApiErrors } from "../utils/errorHandler";
import { useDispatch, useSelector } from "react-redux";

import { toast } from "react-toastify";
import { jwtDecode } from "jwt-decode";

const login = "login";
const logout = "logout";
const refreshtoken = "refreshtoken";
const addaddress = "addaddress";
const getalladdress = "getalladdress";
const getalltransactions = "getalltransactions";
const addtransactions = "addtransactions";
const googlesignup = "googlesignup";
const reset = "reset";

const loginAction = (user, id, token, refreshToken) => ({
  type: login,
  payload: { user, id, token, refreshToken },
});
const googlesignupactions = (user, id, token, refreshToken) => ({
  type: googlesignup,
  payload: { user, id, token, refreshToken },
});
const refreshtokenAction = (user, id, newAccessToken, newRefreshToken) => ({
  type: refreshtoken,
  payload: { user, id, newAccessToken, newRefreshToken },
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
const resetAction =() => ({
  type: reset,
});

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { user, id, token, refreshToken, loading } = useSelector(
    (state) => state.auth
  );
  const { address } = useSelector((state) => state.useraddress);
  const { transaction } = useSelector((state) => state.usertransaction);

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
  const forgotPassword = async (userData) => {
    console.log(process.env);
    console.log(userData);
    try {
      const response = await api.auth.forgotPassword(userData);
      console.log("Forgot Password Response:", response);
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const resetPassword = async (userData) => {
    console.log(process.env);
    try {
      const response = await api.auth.resetPassword(userData);
      console.log("resetPassword Response:", response);
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const resendOtp = async (userData) => {
    console.log(process.env);
    try {
      const response = await api.auth.resendOtp(userData);
      console.log("resendOtp Response:", response);
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const confirmEmail = async (userData) => {
    console.log(process.env);
    try {
      const response = await api.auth.confirmEmail(userData);
      console.log("confirmEmail Response:", response);
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
        googlesignupactions(
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
  const refreshtokenAsync = async (data) => {
    try {
      const response = await api.auth.refreshToken(data);
      const newAccessToken = response.data.data.newAccessToken;
      const newRefreshToken = response.data.data.newRefreshToken;
      console.log("newRefresh Token check: ",newRefreshToken)
      dispatch(
        refreshtokenAction(
          user,
          data.id,
          newAccessToken,
          newRefreshToken
        )
      );
      return response;
    } catch (error) {
      // handleApiErrors(error);
      console.log("refresh erroe: ", error)
      logout(false)
      return null;
    }
  };
  const changepassword = async (data) => {
    try {
      const response = await api.auth.changePassword(data, token);
      console.log("change password Response:", response);
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const addaddress = async (data) => {
    try {
      const response = await api.user.addAddress(data, token);
      console.log("add_address :", response);
      dispatch(addaddressAction(response.data.data));
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const addtransactions = async (credentials, currentPage = 1, pageSize = 20) => {
    try {
      const response = await api.user.addTransaction(credentials, currentPage, pageSize, token);
      console.log(" add_transactions:", response);
      dispatch(addtransactionsAction(response.data.data));
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const getalladdress = async (credentials) => {
    try {
      const response = await api.user.getAllAddress(credentials, token);
      dispatch(getalladdressAction(response.data.data));
      return response;
    } catch (error) {
      handleApiErrors(error);
      return null;
    }
  };
  const getalltransactions = async (page = 1, size= 20) => {
    try {
      const response = await api.user.getAllTransaction(page, size, token);
      console.log("get all transaction: ", response)
      dispatch(getalltransactionsAction(response.data.data));
      return response;
    } catch (error) {
      console.log("get all transaction: ", error)
      logout(false)
      return null;
    }
  };

  const logout = (showToast = true) => {
    if (showToast) {
      toast.success("Hope to see you again");
    }    navigate("/");
    dispatch(logoutAction());
    localStorage.removeItem("confirmEmail");
    localStorage.removeItem("persist:root");
    localStorage.removeItem("LoggedInEmail");
    localStorage.removeItem("IsGoogle");
  };
  const reset = async () => {
    try{
      const response = await api.user.reset(id, token);
      dispatch(resetAction());
      return response;
    }catch (error){
      handleApiErrors(error);
      return null;
    }
    
  }
  const editTransaction = async (data) => {
    try{
      const response = await api.user.editTransaction(data, token);
      return response
    }catch (error){
      handleApiErrors(error);
      return null;
    }
  }

  useEffect(() => {
   try{
    console.log("useEffect triggered");
    if (token) {
      const decodedToken = jwtDecode(token);
      const expirationTime = decodedToken.exp * 1000;

      const checkTokenExpiration = () => {
        const currentTime = Date.now();
        if (currentTime >= expirationTime) {
          console.log("Token has expired: ", expirationTime);
          refreshtokenAsync({ userId: id, refreshToken });
          console.log("refreshToken2 check - Current Time: ", refreshToken);
        } else {
          // If token hasn't expired, set up the next check in 2 seconds
          console.log("Token has not expired: ", expirationTime);
          setTimeout(checkTokenExpiration, 180000);
        }
      };

      checkTokenExpiration();
      return () => {};
    }
   } catch (error)
   {
    logout(false);
   }
  }, [token]);

  return (
    <AuthContext.Provider
      value={{
        address,
        transaction,
        user,
        id,
        token,
        refreshToken,
        loading,
        reset,
        refreshtokenAsync,
        resetPassword,
        resendOtp,
        confirmEmail,
        forgotPassword,
        googleSignInUp,
        signUp,
        login,
        logout,
        changepassword,
        addaddress,
        addtransactions,
        getalltransactions,
        getalladdress,
        editTransaction,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
