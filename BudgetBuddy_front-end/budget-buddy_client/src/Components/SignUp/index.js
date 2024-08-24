import { React, useState } from "react";
import "./style.css";
import Input from "../Input";
import Button from "../Button";
import { toast } from "react-toastify";
import { useAuth } from "../../hooks/useAuth";
import { useNavigate } from "react-router-dom";
import {
  getSignUpValidationSchema,
  handleErrors,
  resetFormFields,
} from "../../utils/helper";
import { useGoogleLogin } from "@react-oauth/google";

function SignUpComponent({ onToggleForm }) {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const { googleSignInUp, signUp, loading } = useAuth();
  const navigate = useNavigate();
  const role = "customer";

  const signUpwithEmail = async () => {
    const validationSchema = getSignUpValidationSchema();
    try {
      await validationSchema.validate({
        firstName,
        lastName,
        email,
        phoneNumber,
        password,
        confirmPassword,
      });

      const response = await signUp({
        firstName,
        lastName,
        email,
        phoneNumber,
        password,
        confirmPassword,
        role,
      });

      if (response !== null) {
        toast.success("Sign-up successful, use the OTP sent to your mail to confirm your Registration");
        localStorage.setItem("confirmEmail", email); 
        resetFormFields([
          setFirstName,
          setLastName,
          setEmail,
          setPassword,
          setConfirmPassword,
          setPhoneNumber,
        ]);
        navigate("/confirmEmail")
      }
    } catch (error) {
      handleErrors(error);
    }
  };

 const googleSignin =  useGoogleLogin({
    onSuccess: async (credentialResponse) => {
      console.log("this is: ", credentialResponse);
      navigate("/dashboard");

      const response = await googleSignInUp({
        token: credentialResponse.access_token,
        role
      });

      if (response !== null) {
        toast.success("Sign-up successful!");
        localStorage.setItem("IsGoogle", true);
        navigate("/dashboard");
      }
    },
    onError: (error) => {
      console.log("this is: ");
      console.error("Google login error:", error);
      toast.error("Google sign-in failed.");
    },
  })

  return (
    <>
      <div className="signup-wrapper">
        <h2 className="signup-title">
          Sign Up on <span className="signup-title_span">BudgetBuddy</span>
        </h2>
        <form>
          <Input
            label={"First Name"}
            type={"text"}
            state={firstName}
            setState={setFirstName}
            placeholder={"chinedu"}
          />
          <Input
            label={"Last Name"}
            type={"text"}
            state={lastName}
            setState={setLastName}
            placeholder={"otuka"}
          />
          <Input
            label={"Email"}
            type={"email"}
            state={email}
            setState={setEmail}
            placeholder={"chineduotuka@gmail.com"}
          />
          <Input
            label={"Phone Number"}
            type={"text"}
            state={phoneNumber}
            setState={setPhoneNumber}
            placeholder={"08135******"}
          />
          <Input
            label={"Password"}
            type={"password"}
            state={password}
            setState={setPassword}
            placeholder={"Example@123"}
          />
          <Input
            label={"Confirm Password"}
            type={"password"}
            state={confirmPassword}
            setState={setConfirmPassword}
            placeholder={"Example@123"}
          />
          <Button
            loading={loading}
            text={"Sign Up using Email & Password"}
            onClick={signUpwithEmail}
          />
          <p className="signup-wrapper_p_tag">or</p>
          <Button
            loading={loading}
            text={"Google Sign Up"}
            onClick={ googleSignin}
            orange={true}
          />
          <p className="p-login" onClick={onToggleForm}>
            Already Have an Account? Click here
          </p>
        </form>
      </div>
    </>
  );
}

export default SignUpComponent;
