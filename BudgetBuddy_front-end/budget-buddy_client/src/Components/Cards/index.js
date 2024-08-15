import React from "react";
import "./styles.css";
import { Card, Row } from "antd";
import Button from "../Button";

function Cards({ showExpenseModal, showIncomeModal , currentBalance, income, expenses}) {
  return (
    <div>
      <Row className="my-card_row">
        <Card className="my-card" title="Current Balance">
          <p>${currentBalance}</p>
          <Button text="reset balance" blue={true} />
        </Card>
        <Card className="my-card" title="Income">
          <p>${income}</p>
          <Button text="Add Income" blue={true} onClick={showIncomeModal}/>
        </Card>
        <Card className="my-card" title="Expense">
          <p>${expenses}</p>
          <Button text="Add Expense" blue={true} onClick={showExpenseModal}/>
        </Card>
      </Row>
    </div>
  );
}

export default Cards;
