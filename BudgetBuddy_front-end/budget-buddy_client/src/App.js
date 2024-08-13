import logo from './logo.svg';
import {BrowserRouter as React, Route, Router, Routes} from "react-router-dom";
import StoreProvider from './store';
import './App.css';
import SignUpSignIn from "./pages/SignUp";

function App() {
  return ( 
    <StoreProvider>
      <Routes>
        <Route path="/" element={<SignUpSignIn />} />
      </Routes>
    </StoreProvider>
  );
}

export default App;
