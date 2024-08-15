import React from "react";
import "./styles.css";
import Spinner from "../Loader/spinner";

function Button({ text, onClick, blue, loading, orange }) {
  const buttonClass = blue ? "btn btn-blue" : orange ? "btn btn-orange" : "btn";
  return (
    <div
      disabled={loading}
      className={buttonClass}
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
