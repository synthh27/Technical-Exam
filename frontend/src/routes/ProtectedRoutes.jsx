import { useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import { Navigate } from 'react-router-dom';

const ProtectedRoutes = ({ children }) => {
    
    // GET USER FROM CONTEXT
    const { user } = useContext(AuthContext);

    // IF NO USER, REDIRECT TO LOGIN
    if(!user) return <Navigate to="/login" replace />;

    // RENDER CHILDREN IF USER EXISTS
    return children;
}

export default ProtectedRoutes
