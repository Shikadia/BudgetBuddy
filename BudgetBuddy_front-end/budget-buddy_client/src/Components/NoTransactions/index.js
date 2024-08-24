import React from "react";
import transactions from "../../assets/transactions.svg";
import "./styles.css";

function NoTransactionsComponent() {
  return (
    <div className="noTransaction_div">
      <img src={transactions} className="noTransaction_div_img" />
      <p className="noTransaction_div_ptag">
        You Have No Transactions Currently
      </p>
    </div>
  );
}

export default NoTransactionsComponent;
