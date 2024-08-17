import React, { createContext, useState, useEffect } from 'react';

//creates the context
export const AuthContext = createContext();

// gives the context
export const AuthProvider = ({ children }) => {
    const [researcherId, setResearcherId] = useState(null); // Estado para almacenar el ID

    //as the value get undefined after a certaing component, it was helpful to log the real time value to noticed when does it gets undefined
    useEffect(() => {
        console.log('Current researcher id:', researcherId);
    }, [researcherId]);

    return (
        <AuthContext.Provider value={{ researcherId, setResearcherId }}>
            {children}
        </AuthContext.Provider>
    );
};
