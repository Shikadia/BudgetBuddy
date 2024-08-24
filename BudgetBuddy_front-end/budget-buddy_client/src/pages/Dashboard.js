import React, { useEffect, useState } from "react";
import Header from "../Components/Header";
import Cards from "../Components/Cards";
import AddExpense from "../Components/Modals/addExpense";
import AddIncome from "../Components/Modals/addIncome";
import { toast } from "react-toastify";
import TransactionTable from "../Components/TansactionsTable";
import { useSelector } from "react-redux";
import { useAuth } from "../hooks/useAuth";
import { handleErrors } from "../utils/helper";
import Spinner from "../Components/Loader/spinner";
import ChartsComponent from "../Components/Charts";
import NoTransactionsComponent from "../Components/NoTransactions";

function Dashboard() {
  const { addtransactions, loading, getalltransactions } = useAuth();
  const [isExpenseModalVisible, setIsExpenseModalVisible] = useState(false);
  const [isIncomeModalVisible, setIsIncomeModalVisible] = useState(false);
  const [currentBalance, setCurrentBalance] = useState(0);
  const [transactions, setTransactions] = useState([]);
  const [income, setIncome] = useState(0);
  const [expenses, setExpenses] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [transactionEdited, setTransactionEdited] = useState(false)
  const [totalTransactions, setTotalTransactions] = useState(0);
  const [transactionAdded, setTransactionAdded] = useState(false);
  const [resetTriggered, setResetTriggered] = useState(false);
  const transacttate = useSelector((state) => state.usertransaction);

  useEffect(() => {
    fetchTransactions();
  }, [currentPage, pageSize, transactionAdded, resetTriggered, transactionEdited]);

  const handlePageChange = (page, size) => {
    console.log("handle page change: ", page, size);
    setCurrentPage(page);
    setPageSize(size);
    fetchTransactions(page, size);
  };

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
      date: values.date.format("YYYY-MM-DD HH:mm"),
      amount: parseFloat(values.amount),
      tag: values.tag,
      description: values.name,
    };

    addTransaction(newTransaction);
  };

  const fetchTransactions = async (page, size) => {
    try {
      const response = await getalltransactions(page, size);
      console.log("fetchTransaction", response)
      if (
        response &&
        response.data &&
        response.data.data &&
        response.data.data.listOfTransactions
      ) {
        const fetchedTransactions =
          response.data.data.listOfTransactions;
        setTransactions(fetchedTransactions);
        setIncome(response.data.data.totalIncome);
        setExpenses(response.data.data.totalExpense);
        setCurrentBalance(response.data.data.totalAmount);
        setTotalTransactions(response.data.data.listOfTransactions.Length);
      }
      else {

      }
    } catch (error) {}
  };

  const addTransaction = async (newTransaction) => {
    try {
      const response = await addtransactions(
        newTransaction,
        currentPage,
        pageSize
      );

      if (response !== null) {
        setTransactionAdded(prev => !prev);
        toast.success("Transaction successful!");
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  let sortedTransactions = transactions.sort((a, b) => {
    return new Date(a.dateTime) - new Date(b.dateTime);
  });

  return (
    <div>
      <Header  onReset={() => setResetTriggered(prev => !prev)} currentBalance={currentBalance}/>
      {loading ? (
        <Spinner />
      ) : (
        <>
          <Cards
            showExpenseModal={showExpenseModal}
            showIncomeModal={showIncomeModal}
            currentBalance={currentBalance}
            income={income}
            expenses={expenses}
            cardStyle="card-style"
          />
          {transactions && transactions.length !== 0 ? (
            <ChartsComponent  sortedTransactions={sortedTransactions}/>
          ) : (
            <NoTransactionsComponent />
          )}
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
          {transacttate &&
          transacttate.transaction &&
          transacttate.transaction.listOfTransactions ? (
            <TransactionTable
              transactions={
                transacttate.transaction.listOfTransactions
              }
              onPageChange={handlePageChange}
              totalTransactions={totalTransactions}
              currentPage={currentPage}
              pageSize={pageSize}
              onEdited={() => setTransactionEdited(prev => !prev)}
            />
          ) : (
            <TransactionTable
              transactions={[]}
              onPageChange={handlePageChange}
              totalTransactions={totalTransactions}
              currentPage={1}
              pageSize={5}
              onEdited={() => setTransactionEdited(prev => !prev)}

            />
          )}
        </>
      )}
    </div>
  );
}

export default Dashboard;
