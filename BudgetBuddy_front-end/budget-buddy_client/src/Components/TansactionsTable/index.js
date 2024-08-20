import { Select, Table } from "antd";
import React, { useState } from "react";
import Input from "../Input";
import { Option } from "antd/es/mentions";

function TransactionTable({ transactions }) {
  console.log("Transactions table:", transactions)
  const [search, setSearch] = useState("");
  const [typeFilter, setTypeFilter] = useState("");
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
      dataIndex: "dateTime",
      key: "dateTime",
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
  ];
  let filteredTransactions = transactions.filter((item) => {
    console.log("item: ", item);
    return (
      item.description && item.description.toLowerCase().includes(search.toLowerCase()) &&
      item.type.includes(typeFilter)
    );
  });
  return (
    <>
      <input
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        placeholder="Search by Name"
      />
      <Select
        className="select-input"
        onChange={(value) => setTypeFilter(value)}
        value={typeFilter}
        placeholder="Filter"
        allowClear
      >
        <Option value="">All</Option>
        <Option value="income">Income</Option>
        <Option value="expense">Expense</Option>
      </Select>
      <Table dataSource={filteredTransactions} columns={columns} />
    </>
  );
}

export default TransactionTable;
