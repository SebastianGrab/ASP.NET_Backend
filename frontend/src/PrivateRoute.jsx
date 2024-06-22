import React, { useContext } from 'react';
import { Route, Navigate } from 'react-router-dom';
import AuthContext from "./API/AuthProvider";
import { Outlet } from 'react-router-dom';

export const PrivateRoute = ({ element: Element, ...rest }) => {
    const { token } = useContext(AuthContext);

    return token ? <Outlet /> : <Navigate to="/" />;
};

export const AdminRoute = ({ element: Element, ...rest }) => {
    const { token, role } = useContext(AuthContext);
  
    return token && (role === 'Admin' || role === 'Leiter') ? <Outlet /> : <Navigate to="/:userId" />;
  };