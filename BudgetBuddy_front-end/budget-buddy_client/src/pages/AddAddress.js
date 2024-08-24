import Header from "../Components/Header";
import AddAddressComponent from "../Components/AddAddress";

const AddAddress = () => {
  return (
    <div>
      <Header />
      <div className="wrapper">
        <AddAddressComponent />
      </div>
    </div>
  );
};

export default AddAddress;
