import { useState, React } from "react";
import Input from "../Input";
import { useAuth } from "../../hooks/useAuth";
import Button from "../Button";
import { toast } from "react-toastify";
import { handleErrors, resetFormFields } from "../../utils/helper";
import "./styles.css";
import { useNavigate } from "react-router-dom";

function AddAddressComponent() {
  const [name, setName] = useState("");
  const [city, setCity] = useState("");
  const [state, setState] = useState("");
  const {addaddress, loading} = useAuth();
  const navigate = useNavigate();


  const addAddress = async () => {
    try {    
        
      const response = await addaddress({
        name,
        city,
        state,
      });

      if (response !== null) {
        toast.success("Address Added");
        resetFormFields([setName, setCity, setState]);
        navigate("/dashboard");
      }
    } catch (error) {
      handleErrors(error);
    }
  };

  const handleOnclick = () => {
    navigate("/dashboard");
  };

  return (
    <>
      <div className="signup-wrapper">
        <h2 className="signup-title">
          Forgot Your Password To{" "}
          <span className="signup-title_span">BudgetBuddy ?</span>
        </h2>
        <form>
          <Input
            label={"Address Line"}
            type={"name"}
            state={name}
            setState={setName}
            placeholder={"Flat 12, 49 Awofodu Street"}
          />
          <Input
            label={"City"}
            type={"city"}
            state={city}
            setState={setCity}
            placeholder={"Lagos, Warri etc"}
          />
          <Input
            label={"State"}
            type={"state"}
            state={state}
            setState={setState}
            placeholder={"Lagos"}
          />
          <Button
            loading={loading}
            text={"Add Your Address"}
            onClick={addAddress}
            orange={true}
          />
          <p className="p-login" onClick={handleOnclick}>
            Go Back? Click here
          </p>
        </form>
      </div>
    </>
  );
}

export default AddAddressComponent;
