import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuthContext } from '../contexts/AuthContext';
import jwtDecoder from '../services/jwtDecoder';
import RestrictedPage from '../pages/RestrictedPage';

function ProtectedRoute({ children, role = null }) {
    const { isAuthenticated } = useAuthContext();

    if (!isAuthenticated)
        return <Navigate to="/login" replace />;

    if (role) {
        const { roles } = jwtDecoder();

        if (!roles || !roles.includes(role)) return <RestrictedPage />;
    }

    return React.cloneElement(children);
}

export default ProtectedRoute;