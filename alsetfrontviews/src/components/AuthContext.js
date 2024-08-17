import React, { createContext, useState, useEffect } from 'react';

// Crea el contexto
export const AuthContext = createContext();

// Proveedor del contexto
export const AuthProvider = ({ children }) => {
    const [researcherId, setResearcherId] = useState(null); // Estado para almacenar el ID

    // Log para verificar los cambios en researcherId
    useEffect(() => {
        console.log('Current researcher ID:', researcherId);
    }, [researcherId]);

    return (
        <AuthContext.Provider value={{ researcherId, setResearcherId }}>
            {children}
        </AuthContext.Provider>
    );
};
