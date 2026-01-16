import { jwtDecode } from 'jwt-decode';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { AuthContext } from './AuthContext';

// EXPORTS THE AUTH CONTEXT PROVIDER
export const AuthProvider = ({ children }) => {

    // USER STATE
    const [user, setUser] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem('user');

        if (token) {
            try {
                // DECODE TOKEN
                const decodedToken = jwtDecode(token);
                // CHECKS IF TOKEN IS EXPIRED, IF SO, LOGOUT USER
                if (decodedToken.exp * 1000 < Date.now()) {
                    logout();
                } else {
                    setUser(decodedToken);
                }
            } catch {
                logout();
            }
        }
    }, []);

    // LOGIN FUNCTION
    const login = (token) => {
        localStorage.setItem('token', token);
        const decodedToken = jwtDecode(token);
        setUser(decodedToken);
        navigate('/tasks');
    };

    // LOGOUT FUNCTION
    const logout = () => {
        localStorage.removeItem('token');
        setUser(null);
        navigate('/login');
    };

    return (
        <AuthContext.Provider value={{user, login, logout}}>
            {children}
        </AuthContext.Provider>
    );
};