import { Button, DatePicker, Form, Input, Modal, Select } from "antd";
import React, { useState } from "react";

const AddExpense = ({
  isExpenseModalVisible,
  handleExpenseCancel,
  onFinish,
}) => {
  const [form] = Form.useForm();
  const [tags, setTags] = useState(["food", "education", "gaming", "bills"]); // Initial tags
  const [newTag, setNewTag] = useState("");

  const handleFinish = (values) => {
    onFinish(values, "expense");
    form.resetFields();
  };
  const handleAddTag = (value) => {
    if (!tags.includes(value)) {
      setTags([...tags, value]); // Add new tag if it doesn't exist
    }
    setNewTag(""); // Reset the input
  };


  return (
    <Modal
      style={{ fontWeight: 600 }}
      title="Add Expense"
      open={isExpenseModalVisible}
      onCancel={handleExpenseCancel}
      footer={null}
    >
      <Form form={form} layout="vertical" onFinish={handleFinish}>
        <Form.Item
          label="Name"
          name="name"
          rules={[
            {
              required: true,
              message: "Please input the name of the transaction!",
            },
          ]}
        >
          <Input type="text" className="custom-input" />
        </Form.Item>

        <Form.Item
          label="Amount"
          name="amount"
          rules={[
            {
              required: true,
              message: "Please input the expense amount!",
            },
          ]}
        >
          <Input type="number" className="custom-input" />
        </Form.Item>

        <Form.Item
          label="Date"
          name="date"
          rules={[
            {
              required: true,
              message: "Please select the expense date!",
            },
          ]}
        >
          <DatePicker
            className="custom-input"
            format="YYYY-MM-DD"
            showTime={{ format: "HH:mm" }}
          />
        </Form.Item>

        <Form.Item
          label="Tag"
          name="tag"
          rules={[
            {
              required: true,
              message: "Please select a tag!",
            },
          ]}
        >
           <Select
            className="select-input-2"
            placeholder="Select or type a tag"
            onSearch={setNewTag}
            value={newTag}
            onChange={handleAddTag}
            showSearch
            dropdownRender={(menu) => (
              <div>
                {menu}
                {newTag && !tags.includes(newTag) && (
                  <div
                    style={{
                      display: 'flex',
                      justifyContent: 'space-between',
                      padding: 8,
                      cursor: 'pointer',
                    }}
                    onMouseDown={(e) => e.preventDefault()}
                    onClick={() => handleAddTag(newTag)}
                  >
                    <span>Add "{newTag}"</span>
                  </div>
                )}
              </div>
            )}
          >
            {tags.map((tag) => (
              <Select.Option key={tag} value={tag}>
                {tag}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item>
          <Button className="btn btn-blue" type="primary" htmlType="submit">
            Add Expense
          </Button>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default AddExpense;
