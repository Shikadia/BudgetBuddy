import React from "react";
import "./styles.css";
import { Card, Row } from "antd";
import Button from "../Button";
import crying from "../../assets/moneydonefinish.gif";
import bigBoy from "../../assets/MoneyOnMymind.gif";

function Cards({ showExpenseModal, showIncomeModal , income, expenses}) {

  const safeIncome = income ?? 0;
  const safeExpenses = expenses ?? 0;

  const formatedIncome = safeIncome.toLocaleString('en-NG', {
    style: 'currency',
    currency: 'NGN',
  });
  const formatedExpense = safeExpenses.toLocaleString('en-NG', {
    style: 'currency',
    currency: 'NGN',
  });

  const isOverSpent = safeIncome - safeExpenses < 5000 || safeIncome === 0;
  return (
    <div>
      <Row className="my-card_row">
        <Card className="my-card-1">
        {isOverSpent ? (<div className="crying-emoji">
          <img src={crying} alt="E done Red" className="crying-gif"/>
        </div>) : (
          <img src={bigBoy} alt="Make we Ball" className="crying-gif"/>
        )}
        </Card>
        <Card className="my-card" title="Income">
          <p>{formatedIncome}</p>
          <Button text="Add Income" blue={true} onClick={showIncomeModal}/>
        </Card>
        <Card className="my-card" title="Expense">
          <p>{formatedExpense}</p>
          <Button text="Add Expense" blue={true} onClick={showExpenseModal}/>
        </Card>
      </Row>
    </div>
  );
}

export default Cards;
