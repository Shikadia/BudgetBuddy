import { React, useState } from "react";
import "./style.css";
import Input from "../Input";
import Button from "../Button";
import { toast } from "react-toastify";
import { useAuth } from "../../hooks/useAuth";
import {
  getSignUpValidationSchema,
  handleErrors,
  resetFormFields,
} from "../../utils/helper";

function SignUpComponent({onToggleForm}) {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [isLoading, setLoading] = useState(false);
  const [phoneNumber, setPhoneNumber] = useState("");
  const [loginForm, setLoginForm] = useState(false);
  const { signUp, loading } = useAuth();

  const signUpwithEmail = async () => {
    const validationSchema = getSignUpValidationSchema();
    try {
      setLoading(true);
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
      });

      if (response !== null) {
        toast.success("Sign-up successful!");

        resetFormFields([
          setFirstName,
          setLastName,
          setEmail,
          setPassword,
          setConfirmPassword,
          setPhoneNumber,
        ]);

        setLoading(false);
      } else {
        setLoading(false);
      }
    } catch (error) {
      handleErrors(error);
    } finally {
      setLoading(false);
    }
  };
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
              loading={isLoading}
              text={"Sign Up using Email & Password"}
              onClick={signUpwithEmail}
            />
            <p className="signup-wrapper_p_tag">or</p>
            <Button loading={isLoading} text={"Google Sign Up"} blue={true} />
            <p className="p-login" onClick={onToggleForm}>
              Already Have an Account? Click here
            </p>
          </form>
        </div>
    </>
  );
}

export default SignUpComponent;
