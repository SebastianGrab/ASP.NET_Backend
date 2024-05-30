import React, { createContext, useState } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(null);
  const [userID, setUserID] = useState(null);
  const [orgaID, setOrgaID] = useState(null);

  return (
    <AuthContext.Provider value={{ token, setToken, userID, setUserID, orgaID, setOrgaID }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
