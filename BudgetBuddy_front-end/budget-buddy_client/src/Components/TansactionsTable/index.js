import { Button, DatePicker, Form, Modal, Radio, Select, Table } from "antd";
import React, { useState } from "react";
import Input from "../Input";
import { Option } from "antd/es/mentions";
import searchImg from "../../assets/search.svg";
import "./styles.css";
import { unparse } from "papaparse";
import { useAuth } from "../../hooks/useAuth";
import EditTransaction from "../Modals/editTransaction";
import { toast } from "react-toastify";

function TransactionTable({
  transactions,
  onPageChange,
  currentPage,
  pageSize,
  onEdited,
}) {
  const [search, setSearch] = useState("");
  const [typeFilter, setTypeFilter] = useState("");
  const [sortKey, setSortKey] = useState("");
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [currentTransaction, setCurrentTransaction] = useState(null);
  const { editTransaction } = useAuth();

  const columns = [
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
    },
    {
      title: "Type",
      dataIndex: "type",
      key: "type",
    },
    {
      title: "Date",
      dataIndex: "date",
      key: "date",
    },
    {
      title: "Amount",
      dataIndex: "amount",
      key: "amount",
    },
    {
      title: "Tag",
      dataIndex: "categoryOrTag",
      key: "categoryOrTag",
    },
    {
      title: "Action",
      key: "action",
      render: (_, record) => (
        <Button onClick={() => showEditModal(record)}>Edit</Button>
      ),
    },
  ];
  let filteredTransactions = transactions.filter((item) => {
    console.log("filtered", transactions)
    return (
      item.description &&
      item.description.toLowerCase().includes(search.toLowerCase()) &&
      item.type.includes(typeFilter)
    );
  });
  let sortedTransactions = filteredTransactions.sort((a, b) => {
    if (sortKey === "date") {
      return new Date(a.dateTime) - new Date(b.dateTime);
    } else if (sortKey === "amount") {
      return a.amount - b.amount;
    } else {
      return 0;
    }
  });
  const showEditModal = (transaction) => {
    setCurrentTransaction(transaction);
    setIsEditModalVisible(true);
  };

  const handleEditCancel = () => {
    setIsEditModalVisible(false);
    setCurrentTransaction(null);
  };

  const onFinish = async (values, id) => {
    const editedTransaction = {
      id: id,
      type: values.type,
      date: values.date.format("YYYY-MM-DD HH:mm"),
      amount: parseFloat(values.amount),
      tag: values.categoryOrTag,
      description: values.description,
    };
    const response = await editTransaction(editedTransaction);
    if (response !== null) {
      toast.success("Successfuly edited");
      onEdited();
      handleEditCancel();
    }
    handleEditCancel();
  };

  const handleTableChange = (pagination) => {
    onPageChange(pagination.current, pagination.pageSize);
  };

  const exportCsv = () => {
    const csv = unparse({
      fields: ["description", "amount", "date", "categoryOrTag", "type"],
      data: transactions,
    });
    const blob = new Blob([csv], { type: "text/csv;charset=utf-8;" });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = "transactions.csv";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <div
      style={{
        width: "95%",
        padding: "0rem 2rem",
      }}
    >
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          gap: "1rem",
          alignItems: "center",
          marginBottom: "1rem",
        }}
      >
        <div className="input-flex">
          <img src={searchImg} width="16" alt="search image" />
          <input
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Search by Name"
          />
        </div>

        <Select
          className="select-input"
          onChange={(value) => setTypeFilter(value)}
          value={typeFilter}
          placeholder="Filter"
          allowClear
        >
          <Option value="">All</Option>
          <Option value="Income">Income</Option>
          <Option value="Expense">Expense</Option>
        </Select>
      </div>
      <div className="my-table">
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            width: "100%",
            marginBottom: "1rem",
          }}
        >
          <h2>Your Budget Buddy Transactions</h2>
          <Radio.Group
            className="input-radio"
            onChange={(e) => setSortKey(e.target.value)}
            value={sortKey}
          >
            <Radio.Button value="">No Sort</Radio.Button>
            <Radio.Button value="date">Sort by Date</Radio.Button>
            <Radio.Button value="amount">Sort by Amount</Radio.Button>
          </Radio.Group>
          <div
            style={{
              display: "flex",
              justifyContent: "center",
              gap: "1rem",
              width: "400px",
            }}
          >
            <button className="btn" onClick={exportCsv}>
              Export to CSV
            </button>
          </div>
        </div>
        <Table
          dataSource={sortedTransactions}
          columns={columns}
          pagination={{
            current: currentPage,
            pageSize: pageSize,
            total: transactions.length, // You can adjust this if you have a total count from the backend
            showSizeChanger: true,
            pageSizeOptions: ["5", "10", "20", "50"], // Customize as needed
          }}
          onChange={handleTableChange}
        />
      </div>
      <EditTransaction
        isVisible={isEditModalVisible}
        transaction={currentTransaction}
        onCancel={handleEditCancel}
        onFinish={onFinish}
      />
    </div>
  );
}

export default TransactionTable;
