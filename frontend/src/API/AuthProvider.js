import React, { createContext, useState } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(null);
  const [userID, setUserID] = useState(null);
  const [orgaID, setOrgaID] = useState(null);
  const [role, setRole] = useState(null);
  const [orgaName, setOrgaName] = useState(null);
  const [refreshHandler, setRefreshHandler] = useState(null);

  const logOut = () => {
    setToken(null);
    setUserID(null);
    setOrgaID(null);
    setRole(null);
    setOrgaName(null);
  }


  return (
    <AuthContext.Provider value={{ token, setToken, userID, setUserID, orgaID, setOrgaID, refreshHandler, setRefreshHandler, role, setRole, orgaName, setOrgaName }}>
      {children}
    </AuthContext.Provider>
  );
};


export default AuthContext;
