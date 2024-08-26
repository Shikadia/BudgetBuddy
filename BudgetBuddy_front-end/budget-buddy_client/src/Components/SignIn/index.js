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
import {
  useGoogleLogin,
} from "@react-oauth/google";

function SignInComponent({ onToggleForm }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { googleSignInUp, login, loading } = useAuth();
  const role = "customer";
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
        console.log("thiis", email)
        localStorage.setItem("LoggedInEmail", email); 
        resetFormFields([setEmail, setPassword]);
        navigate("/dashboard");
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  const googleSignin = useGoogleLogin({
      
    onSuccess: async (credentialResponse) => {
      console.log("this is: ", credentialResponse);
      
      const response = await googleSignInUp({
        token: credentialResponse.access_token,
        role
      });

      if (response !== null) {
        toast.success("Sign-up successful!");
        console.log("sssss", response)
        setEmail(response.data.data.email)
        localStorage.setItem("LoggedInEmail", response.data.data.email); 
        localStorage.setItem("IsGoogle", true);
        navigate("/dashboard");
      }    
    },
    onError: (error) => {
      toast.error("Google sign-in failed.");
    },
    scope: 'openid email profile' 
});

const handleForgotPassword = () => {
  navigate("/forgotPassword");
}

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
          <Button
            loading={loading}
            text={"Google Sign In"}
            onClick={googleSignin}
            orange={true}
          />
          <p className="p-login" onClick={onToggleForm}>
            Do Not Have An Account? Click here
          </p>
          <p className="p-login" onClick={handleForgotPassword}>
            Forgot Password? Click here
          </p>
        </form>
      </div>
    </>
  );
}

export default SignInComponent;
