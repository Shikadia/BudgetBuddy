import React from "react";
import "./styles.css";
import Spinner from "../Loader/spinner";

function Button({ text, onClick, blue, loading }) {
  return (
    <div
      disabled={loading}
      className={blue ? "btn btn-blue" : "btn"}
      onClick={!loading ? onClick : null}
    >
      {loading ? (
        <>
          <Spinner />
          <span>loading...</span>
        </>
      ) : (
        text
      )}
    </div>
  );
}

export default Button;
