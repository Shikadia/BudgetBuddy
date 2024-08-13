import React from "react";
import "./styles.css";
import { type } from "@testing-library/user-event/dist/type";

function Input({ label, state, setState, placeholder, type }) {
  return (
    <div className="input-wrapper">
      <p className="input-label">{label}</p>
      <input
        value={state}
        type={type}
        placeholder={placeholder}
        onChange={(e) => setState(e.target.value)}
        className="custom-input"
      />
    </div>
  );
}

export default Input;
