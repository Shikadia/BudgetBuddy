import Header from "../Components/Header";
import SignUpSignInComponent from "../Components/SignUpSignIn";

const SignUpSignIn = () => {
  return (
    <div>
      <Header />
      <div className="wrapper">
        <SignUpSignInComponent/>
      </div>
    </div>
  );
};

export default SignUpSignIn;
