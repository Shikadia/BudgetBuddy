import { Navigate } from "react-router-dom";
import { useSelector } from "react-redux";

function ProtectedRoute({element}) {
    const authState = useSelector((state) => state.auth)

    if (authState.user === null) {
        return <Navigate to="/" />;     
    }
    return element;
}

export default ProtectedRoute;