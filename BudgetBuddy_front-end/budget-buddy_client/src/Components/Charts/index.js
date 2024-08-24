import React from "react";
import {
  LineChart,
  Line,
  PieChart,
  Pie,
  Cell,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";
import "./styles.css";

function ChartsComponent({ sortedTransactions }) {
  const data = sortedTransactions.reduce((acc, item) => {
    const existing = acc.find((d) => d.date === item.date);

    if (existing) {
      existing[item.type.toLowerCase()] += item.amount;
    } else {
      acc.push({
        date: item.date,
        income: item.type.toLowerCase() === "income" ? item.amount : 0,
        expense: item.type.toLowerCase() === "expense" ? item.amount : 0,
      });
    }

    return acc;
  }, []);

  const spendingData = sortedTransactions
    .filter((transaction) => transaction.type.toLowerCase() === "expense")
    .map((transaction) => ({
      categoryOrTag: transaction.categoryOrTag,
      amount: transaction.amount,
    }));

  const finalSpending = Object.values(
    spendingData.reduce((acc, obj) => {
      if (!acc[obj.categoryOrTag]) {
        acc[obj.categoryOrTag] = {
          categoryOrTag: obj.categoryOrTag,
          amount: obj.amount,
        };
      } else {
        acc[obj.categoryOrTag].amount += obj.amount;
      }
      return acc;
    }, {})
  );
  const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042"];

  const barChartConfig = {
    width: 500,
    height: 300,
    data,
    margin: {
      top: 5,
      right: 30,
      left: 20,
      bottom: 5,
    },
  };

  return (
    <div className="charts-wrapper">
      <div style={{ width: "100%", height: 400 }}>
        <ResponsiveContainer>
          <LineChart {...barChartConfig}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="date" />
            <YAxis />
            <Tooltip />
            <Legend />
            <Line dataKey="income" fill="#52c41a" />
            <Line dataKey="expense" fill="#ff4d4f" />
          </LineChart>
        </ResponsiveContainer>
      </div>
      <div style={{ width: "50%", height: 400, display: 'inline-block', verticalAlign: 'top' }}>
        <ResponsiveContainer>
          <PieChart>
            <Pie
              data={finalSpending}
              dataKey="amount"
              nameKey="categoryOrTag"
              cx="50%"
              cy="50%"
              outerRadius={140}
              innerRadius={50}
              fill="#8884d8"
              label
            >
              {finalSpending.map((entry, index) => (
                <Cell
                  key={`cell-${index}`}
                  fill={COLORS[index % COLORS.length]}
                />
              ))}
            </Pie>
            <Tooltip />
            <Legend verticalAlign="bottom" height={36}/> 
          </PieChart>
        </ResponsiveContainer>
        </div>
    </div>
  );
}

export default ChartsComponent;
