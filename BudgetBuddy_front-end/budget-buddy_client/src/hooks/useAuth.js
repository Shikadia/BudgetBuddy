import { useContext } from "react";
import { AuthContext } from "../store";

export const useAuth = () => {
    return useContext(AuthContext);
};