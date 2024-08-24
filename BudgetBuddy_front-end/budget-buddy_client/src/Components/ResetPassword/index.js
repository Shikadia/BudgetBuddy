import { useState, React } from "react";
import Input from "../Input";
import { useAuth } from "../../hooks/useAuth";
import Button from "../Button";
import { toast } from "react-toastify";
import {
  getSignValidationSchema,
  handleErrors,
  resetFormFields,
} from "../../utils/helper";
import "./styles.css";
import { useNavigate } from "react-router-dom";

function ResetPasswordComponent() {
  const [email, setEmail] = useState("");
  const [token, setToken] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, serConfirmPassword] = useState("");
  const navigate = useNavigate();
  const { resetPassword, loading, resendOtp } = useAuth();

  const resetPasswordAsync = async () => {
    // const validationSchema = getSignValidationSchema();
    try {
      //   await validationSchema.validate({
      //     email,
      //   });
      const resetEmail = localStorage.getItem("forgotPasswordEmail");
      const response = await resetPassword({
        email: resetEmail,
        token,
        newPassword,
        confirmPassword,
      });

      if (response !== null) {
        toast.success("password reset successful!");
        resetFormFields([setToken, setNewPassword, serConfirmPassword]);
        localStorage.removeItem("forgotPasswordEmail")
        navigate("/");
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  const handleBack = () => {
    navigate("/");
  };

  const handleResend = async () => {
    // const validationSchema = getSignValidationSchema();
    try {
      //   await validationSchema.validate({
      //     email,
      //   });

      const response = await resendOtp({
        email,
        purpose: "ConfirmEmail",
      });

      if (response !== null) {
        toast.success("OTP Sent to: ", { email });
        resetFormFields([
          setToken,
          setNewPassword,
          serConfirmPassword,
        ]);
      }
    } catch (error) {
      handleErrors(error);
    }
  };
  return (
    <>
      <div className="signup-wrapper">
        <h2 className="signup-title">
          Reset Your Password To{" "}
          <span className="signup-title_span">BudgetBuddy Here</span>
        </h2>
        <form>
          <Input
            label={"Token"}
            type={"text"}
            state={token}
            setState={setToken}
            placeholder={"1234567"}
          />
          <Input
            label={"NewPassword"}
            type={"password"}
            state={newPassword}
            setState={setNewPassword}
            placeholder={"Example@123"}
          />
          <Input
            label={"ConfirmPassword"}
            type={"password"}
            state={confirmPassword}
            setState={serConfirmPassword}
            placeholder={"Example@123"}
          />
          <Button
            loading={loading}
            text={"Send Request"}
            onClick={resetPasswordAsync}
          />
          <p className="p-login" onClick={handleBack}>
            Go to Login Page? Click here
          </p>
          <p className="p-login" onClick={handleResend}>
            Did not recieve an OTP ? Click here to resend OTP
          </p>
        </form>
      </div>
    </>
  );
}

export default ResetPasswordComponent;
