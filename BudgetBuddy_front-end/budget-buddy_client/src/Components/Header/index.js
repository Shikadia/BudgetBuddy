import "./index.css";

function Header() {
  function logoutFunc() {
    alert("Logout!");
  }

  return (
    <div className="navbar">
      <p className="navbar-heading">BudgetBuddy.</p>
      <p onClick={logoutFunc} className="navbar-link">
        Logout
      </p>
    </div>
  );
}

export default Header;
