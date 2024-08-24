import { useState, React } from "react";
import Input from "../Input";
import { useAuth } from "../../hooks/useAuth";
import Button from "../Button";
import { toast } from "react-toastify";
import { handleErrors, resetFormFields } from "../../utils/helper";
import "./styles.css";
import { useNavigate } from "react-router-dom";

function ConfirmEmailComponent() {
  const [token, setToken] = useState("");
  const navigate = useNavigate();
  const { confirmEmail, loading } = useAuth();

  const confirmEmailAsync = async () => {
    try {    
        const emailAddress = localStorage.getItem("confirmEmail");
      const response = await confirmEmail({
        emailAddress,
        token,
      });

      if (response !== null) {
        toast.success("Email confirmed: ", { emailAddress });
        resetFormFields([setToken]);
        localStorage.removeItem("confirmEmail")
        navigate("/");
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  const handleOnclick = () => {
    navigate("/");
  };

  return (
    <>
      <div className="signup-wrapper">
        <h2 className="signup-title">
          Forgot Your Password To{" "}
          <span className="signup-title_span">BudgetBuddy ?</span>
        </h2>
        <form>
          <Input
            label={"Token"}
            type={"token"}
            state={token}
            setState={setToken}
            placeholder={"123456"}
          />
          <Button
            loading={loading}
            text={"Send Request"}
            onClick={confirmEmailAsync}
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

export default ConfirmEmailComponent;
