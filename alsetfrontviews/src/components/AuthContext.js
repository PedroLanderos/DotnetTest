import React, { createContext, useState } from 'react';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [researcherId, setResearcherId] = useState(null);

    return (
        <AuthContext.Provider value={{ researcherId, setResearcherId }}>
            {children}
        </AuthContext.Provider>
    );
};
