import "./index.css";
import { useAuth } from "../../hooks/useAuth";
import { useLocation, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import userImg from "../../assets/user.svg";

function Header({ onReset, currentBalance }) {
  const { logout, user, loading, reset } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();
  const [isGoogle, setIsGoole] = useState(false);
  const [email, setEmail] = useState(null);

  useEffect(() => {
    if (user !== null && !loading) {
      const mail = localStorage.getItem("LoggedInEmail");
      const isGoogle = localStorage.getItem("IsGoogle");
      console.log(isGoogle);
      setIsGoole(isGoogle);
      setEmail(mail);
      console.log("uuu", isGoogle);
      if (
        location.pathname === "/" ||
        location.pathname === "/login" ||
        location.pathname === "/register"
      ) {
        navigate("/dashboard");
      }
    } else {
      setEmail(null);
    }
  }, [user, loading, navigate, location.pathname]);

  const handleChangePassword = () => {
    navigate("/change-password");
  };

  const handleAddAddress = () => {
    navigate("/add-address");
  };
  const handleReset = async () => {
    var response = await reset();
    if (response !== null) {
      onReset();
    }
  };
  const balance =
    currentBalance !== undefined && currentBalance !== null
      ? currentBalance
      : 0;

  const balanceStyle = {
    color: balance < 5000 ? "red" : "darkGreen",
    backgroundColor: balance < 5000 ? "transparent" : "white",
    padding: "5px",
    borderRadius: "5px",
    textShadow: "1px 1px 2px black",
  };
  const formattedBalance = balance.toLocaleString("en-NG", {
    style: "currency",
    currency: "NGN",
  });

  return (
    <div className="navbar">
      <p className="navbar-heading">BudgetBuddy</p>
      {user && (
        <>
          <div className="navbar-balance" style={balanceStyle}>
            Current Balance: {formattedBalance}
          </div>
          <div className="header-logo-div">
            <div className="navbar-email">
              <img src={userImg} alt="User" className="user-icon" />
              <span>{email}</span>
              <div className="dropdown-content">
                {!isGoogle && (
                  <a onClick={handleChangePassword}>Change Password</a>
                )}
                <a onClick={handleAddAddress}>Add Address</a>
                <a onClick={handleReset}>Reset balance/Transactions</a>
                <a onClick={logout}>Logout</a>
              </div>
            </div>
          </div>
        </>
      )}
    </div>
  );
}

export default Header;
