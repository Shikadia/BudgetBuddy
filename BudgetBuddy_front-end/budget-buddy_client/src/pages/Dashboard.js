import React, { useEffect, useState } from "react";
import Header from "../Components/Header";
import Cards from "../Components/Cards";
import { Modal } from "antd";
import AddExpense from "../Components/Modals/addExpense";
import AddIncome from "../Components/Modals/addIncome";
import moment from "moment";
import { toast } from "react-toastify";
import TransactionTable from "../Components/TansactionsTable";
import { useSelector } from "react-redux";
import { useAuth } from "../hooks/useAuth";
import { handleErrors } from "../utils/helper";
import Spinner from "../Components/Loader/spinner";

function Dashboard() {
  const { addtransactions, loading, getalltransactions} = useAuth();
  const [isExpenseModalVisible, setIsExpenseModalVisible] = useState(false);
  const [isIncomeModalVisible, setIsIncomeModalVisible] = useState(false);
  const [currentBalance, setCurrentBalance] = useState(0);
  const [transactions, setTransactions] = useState([]);
  const [income, setIncome] = useState(0);
  const [expenses, setExpenses] = useState(0);
  const transacttate = useSelector((state) => state.usertransaction)

  useEffect(() => {
    console.log("checking transactions: ", transactions);
    // Fetch transactions and update state on page reload
    const x = transacttate.transaction.listOfTransactions.pageItems
    const w = transacttate.transaction
    console.log("checking transactions1: ", x);
    console.log("checking state: ", w);
    fetchTransactions();
    // console.log("checking transactions1: ", x);
  }, []);

  const processChartData = () => {
    const balanceData = [];
    const spendingData = {};

    transactions.forEach((transaction) => {
      const monthYear = moment(transaction.date).format("MMM YYYY");
      const tag = transaction.tag;

      if (transaction.type === "Income") {
        if (balanceData.some((data) => data.month === monthYear)) {
          balanceData.find((data) => data.month === monthYear).balance +=
            transaction.amount;
        } else {
          balanceData.push({ month: monthYear, balance: transaction.amount });
        }
      } else {
        if (balanceData.some((data) => data.month === monthYear)) {
          balanceData.find((data) => data.month === monthYear).balance -=
            transaction.amount;
        } else {
          balanceData.push({ month: monthYear, balance: -transaction.amount });
        }

        if (spendingData[tag]) {
          spendingData[tag] += transaction.amount;
        } else {
          spendingData[tag] = transaction.amount;
        }
      }
    });

    const spendingDataArray = Object.keys(spendingData).map((key) => ({
      category: key,
      value: spendingData[key],
    }));

    return { balanceData, spendingDataArray };
  };

  const { balanceData, spendingDataArray } = processChartData();
  const showExpenseModal = () => {
    setIsExpenseModalVisible(true);
  };

  const showIncomeModal = () => {
    setIsIncomeModalVisible(true);
  };

  const handleExpenseCancel = () => {
    setIsExpenseModalVisible(false);
  };

  const handleIncomeCancel = () => {
    setIsIncomeModalVisible(false);
  };

  const onFinish = (values, type) => {
    const newTransaction = {
      type: type,
      dateTime: moment(values.date).format("YYYY-MM-DD"),
      amount: parseFloat(values.amount),
      tag: values.tag,
      description: values.name,
    };

    addTransaction(newTransaction);
  };

  const fetchTransactions = async () => {
    try {
      const response = await getalltransactions();

      if (response && response.data && response.data.data && response.data.data.listOfTransactions) {
        const fetchedTransactions = response.data.data.listOfTransactions.pageItems;
        setTransactions(fetchedTransactions);
        setIncome(response.data.data.totalIncome);
        setExpenses(response.data.data.totalExpense);
        setCurrentBalance(response.data.data.totalAmount);

      }
    } catch (error) {
      console.error("Error fetching transactions:", error);
      handleErrors(error);
    }
  };


  const addTransaction = async (newTransaction) => {
    try {
      const response = await addtransactions(newTransaction);
      const x = response.data.data.listOfTransactions.pageItems;
      console.log("transactions add transactions: ", x);
      console.log(response.data.data.totalAmount);
      setTransactions(x);
      setCurrentBalance(response.data.data.totalAmount);
      fetchTransactions();

      if (response !== null) {
        toast.success("Transaction successful!");
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  const balanceConfig = {
    data: balanceData,
    xField: "month",
    yField: "balance",
  };

  const spendingConfig = {
    data: spendingDataArray,
    angleField: "value",
    colorField: "category",
  };

  const cardStyle = {
    boxShadow: "0px 0px 30px 8px rgba(227, 227, 227, 0.75)",
    margin: "2rem",
    borderRadius: "0.5rem",
    minWidth: "400px",
    flex: 1,
  };

  return (
    <div>
      <Header />
      {loading ? (
        <Spinner/>
      ) : (
        <>
          <Cards
            showExpenseModal={showExpenseModal}
            showIncomeModal={showIncomeModal}
            currentBalance={currentBalance}
            income={income}
            expenses={expenses}
            cardStyle={cardStyle}
            // reset={reset}
          />
          <AddExpense
            isExpenseModalVisible={isExpenseModalVisible}
            handleExpenseCancel={handleExpenseCancel}
            onFinish={onFinish}
          />
          <AddIncome
            isIncomeModalVisible={isIncomeModalVisible}
            handleIncomeCancel={handleIncomeCancel}
            onFinish={onFinish}
          />
          <TransactionTable transactions={transacttate.transaction.listOfTransactions.pageItems} />
        </>
      )}
    </div>
  );
}

export default Dashboard;
