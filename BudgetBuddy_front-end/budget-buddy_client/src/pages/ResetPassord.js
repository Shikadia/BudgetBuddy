import Header from "../Components/Header";
import { useState } from "react";
import ForgotPasswordComponent from "../Components/ForgotPassword";
import ResetPasswordComponent from "../Components/ResetPassword";

const ResetPassword = () => {
  return (
    <div>
      <Header />
      <div className="wrapper">
        <ResetPasswordComponent/>
      </div>
    </div>
  );
};

export default ResetPassword;
