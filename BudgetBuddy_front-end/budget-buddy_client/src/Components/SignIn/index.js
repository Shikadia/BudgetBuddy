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

function SignInComponent({ onToggleForm }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { login, loading } = useAuth();

  const navigate = useNavigate();

  const loginUsingEmail = async () => {
    const validationSchema = getSignValidationSchema();
    try {
      await validationSchema.validate({
        email,
        password,
      });

      const response = await login({
        email,
        password,
      });

      if (response !== null) {
        toast.success("Sign-in successful!");
        resetFormFields([setEmail, setPassword]);
        navigate("/dashboard");
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  return (
    <>
      <div className="signup-wrapper">
        <h2 className="signup-title">
          Login To <span className="signup-title_span">BudgetBuddy</span>
        </h2>
        <form>
          <Input
            label={"Email"}
            type={"email"}
            state={email}
            setState={setEmail}
            placeholder={"chineduotuka@gmail.com"}
          />
          <Input
            label={"Password"}
            type={"password"}
            state={password}
            setState={setPassword}
            placeholder={"Example@123"}
          />
          <Button
            loading={loading}
            text={"Log In using Email & Password"}
            onClick={loginUsingEmail}
          />
          <p className="signup-wrapper_p_tag">or</p>
          <Button loading={loading} text={"Google Login"} orange={true} />
          <p className="p-login" onClick={onToggleForm}>
            Do Not Have An Account? Click here
          </p>
        </form>
      </div>
    </>
  );
}

export default SignInComponent;
