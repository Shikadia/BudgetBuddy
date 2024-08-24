import { useState, React } from "react";
import Input from "../Input";
import { useAuth } from "../../hooks/useAuth";
import Button from "../Button";
import { toast } from "react-toastify";
import {
    emailValidationSchema,
  getSignValidationSchema,
  handleErrors,
  resetFormFields,
} from "../../utils/helper";
import "./styles.css";
import { useNavigate } from "react-router-dom";


function ForgotPasswordComponent() {
  const [emailAddress, setEmail] = useState("");
  const navigate = useNavigate();
  const {forgotPassword, loading } = useAuth();

  const forgotPasswordAsync = async () => {
     const validationSchema = emailValidationSchema();
    try {
      await validationSchema.validate({
        emailAddress,
      });
      const response = await forgotPassword({
        emailAddress,
      });

      if (response !== null) {
        toast.success("Token sent to: ", {emailAddress});
        localStorage.setItem("forgotPasswordEmail", emailAddress); 
        resetFormFields([setEmail]);
        navigate("/resetPassword");
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  const handleOnclick = () => {
    navigate("/");
  }

  return (
    <>
      <div className="signup-wrapper">
        <h2 className="signup-title">
          Forgot Your Password  To <span className="signup-title_span">BudgetBuddy ?</span>
        </h2>
        <form>
          <Input
            label={"Email"}
            type={"emailAddress"}
            state={emailAddress}
            setState={setEmail}
            placeholder={"chineduotuka@gmail.com"}
          />
          <Button
            loading={loading}
            text={"Send Request"}
            onClick={forgotPasswordAsync}
            blue={true}
          />
          <p className="p-login" onClick={handleOnclick}>
            Go Back? Click here
          </p>
        </form>
      </div>
    </>
  );
}

export default ForgotPasswordComponent;
