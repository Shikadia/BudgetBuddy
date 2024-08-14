import Header from "../Components/Header";
import SignUpComponent from "../Components/SignUp";
import SignInComponent from "../Components/SignIn";
import { useState } from "react";

const SignUpSignIn = () => {
    const [isSigninForm, setIsSigninForm] = useState(false);

    const toggleForm = () => {
        setIsSigninForm(!isSigninForm);
    }
  return (
    <div>
      <Header />
      <div className="wrapper">
        {isSigninForm ? <SignUpComponent onToggleForm={toggleForm}/> : <SignInComponent onToggleForm={toggleForm}/>}        
      </div>
    </div>
  );
};

export default SignUpSignIn;
