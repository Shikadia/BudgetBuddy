import Header from "../Components/Header";
import { useState } from "react";
import ForgotPasswordComponent from "../Components/ForgotPassword";

const ForgotPassword = () => {
  return (
    <div>
      <Header />
      <div className="wrapper">
        <ForgotPasswordComponent />
      </div>
    </div>
  );
};

export default ForgotPassword;
