import "./index.css";
import { useAuth } from "../../hooks/useAuth";
import { useNavigate } from "react-router-dom";

function Header() {
  const { logout } = useAuth();

  return (
    <div className="navbar">
      <p className="navbar-heading">BudgetBuddy.</p>
      <p onClick={logout} className="navbar-link">
        Logout
      </p>
    </div>
  );
}

export default Header;
