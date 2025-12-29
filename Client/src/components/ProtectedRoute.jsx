import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

function ProtectedRoute({ children, role = null }) {
    const { isAuthenticated } = useAuth();

    if (!isAuthenticated)
        return <Navigate to="/login" replace />;

    return children;
}

export default ProtectedRoute;