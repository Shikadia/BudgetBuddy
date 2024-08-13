import React, { useState } from "react";
import "./style.css";
import Input from "../Input";
import Button from "../Button";

function SignUpSignInComponent() {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setpassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);


  return (
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
          label={"Password"}
          type={"password"}
          state={password}
          setState={setpassword}
          placeholder={"Example@123"}
        />
        <Input
          label={"Confirm Password"}
          type={"password"}
          state={confirmPassword}
          setState={setConfirmPassword}
          placeholder={"Example@123"}
        />
        <Button text={"Sign Up"}/>
        <p className="signup-wrapper_p_tag">or</p>
        <Button text={"Google Sign Up"} blue={true}/>
      </form>
    </div>
  );
}

export default SignUpSignInComponent;
