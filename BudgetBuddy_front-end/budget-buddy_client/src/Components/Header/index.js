import "./index.css";
import { useAuth } from "../../hooks/useAuth";
import { Navigate, useNavigate } from "react-router-dom";
import { useEffect } from "react";

function Header() {
  const { logout, user , loading} = useAuth();
  const navigate = useNavigate();
  

  useEffect(() => {
    if (user !== null && !loading) {
      navigate("/dashboard")
    }
  }, [user, loading, navigate])

  return (
    <div className="navbar">
      <p className="navbar-heading">BudgetBuddy.</p>
      {user && (
        <p onClick={logout} className="navbar-link">
          Logout
        </p>
      )}
    </div>
  );
}

export default Header;
