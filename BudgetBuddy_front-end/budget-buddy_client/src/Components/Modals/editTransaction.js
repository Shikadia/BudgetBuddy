import { DatePicker, Form, Input, Modal, Select } from "antd";
import moment from "moment";
import React, { useEffect } from "react";

const EditTransaction = ({ isVisible, transaction, onCancel, onFinish }) => {
  const [form] = Form.useForm();

  useEffect(() => {
    console.log("iiiiiiiiioooo", transaction)
    if (transaction) {
      form.setFieldsValue({
        ...transaction,
        date: moment(transaction.date, "DD-MM-YYYY HH:mm"), // Ensure date is a moment object
      });
    }
  }, [transaction, form]);

  const handleFinish = (values) => {
    onFinish(values, transaction.id);
    form.resetFields();
  };

  return (
    <Modal
      title="Edit Transaction"
      visible={isVisible}
      onCancel={onCancel}
      onOk={() => form.submit()}
    >
      <Form form={form} layout="vertical" onFinish={handleFinish}>
        <Form.Item
          label="Description"
          name="description"
          rules={[{ required: true, message: "Please input the description!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Amount"
          name="amount"
          rules={[{ required: true, message: "Please input the amount!" }]}
        >
          <Input type="number" />
        </Form.Item>
        <Form.Item
          label="Date"
          name="date"
          rules={[{ required: true, message: "Please select the date!" }]}
        >
          <DatePicker
            format="YYYY-MM-DD"
            className="custom-input"
            showTime={{ format: "HH:mm" }}
          />
        </Form.Item>
        <Form.Item
          label="Tag"
          name="categoryOrTag"
          rules={[{ required: true, message: "Please input the tag!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Type"
          name="type"
          rules={[{ required: true, message: "Please select the type!" }]}
        >
          <Select>
            <Select.Option value="Income">Income</Select.Option>
            <Select.Option value="Expense">Expense</Select.Option>
          </Select>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default EditTransaction;
