import { useState, React } from "react";
import Input from "../Input";
import { useAuth } from "../../hooks/useAuth";
import Button from "../Button";
import { toast } from "react-toastify";
import { handleErrors, resetFormFields } from "../../utils/helper";
import "./styles.css";
import { useNavigate } from "react-router-dom";

function ChangePasswordComponent() {
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmNewPassword, setConfirmNewPassword] = useState("");
  const {changepassword, loading, logout} = useAuth();
  const navigate = useNavigate();


  const changePassword = async () => {
    try {    
        
      const response = await changepassword({
        currentPassword,
        newPassword,
        confirmNewPassword,
      });

      if (response !== null) {
        toast.success("Password Changed, Login Again");
        resetFormFields([setCurrentPassword, setNewPassword, setConfirmNewPassword ]);
        logout(false);
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  const handleOnclick = () => {
    navigate("/dashboard");
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
            label={"CurrentPassWord"}
            type={"currentPassword"}
            state={currentPassword}
            setState={setCurrentPassword}
            placeholder={"OldPass@123"}
          />
          <Input
            label={"NewPassword"}
            type={"newPassword"}
            state={newPassword}
            setState={setNewPassword}
            placeholder={"Example@123"}
          />
          <Input
            label={"ConfirmNewPassword"}
            type={"confirmNewPassword"}
            state={confirmNewPassword}
            setState={setConfirmNewPassword}
            placeholder={"Example@123"}
          />
          <Button
            loading={loading}
            text={"Change Password"}
            onClick={changePassword}
            orange={true}
          />
          <p className="p-login" onClick={handleOnclick}>
            Go Back? Click here
          </p>
        </form>
      </div>
    </>
  );
}

export default ChangePasswordComponent;
