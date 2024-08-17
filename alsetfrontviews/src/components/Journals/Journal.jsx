import React, { useEffect, useState } from "react";
import axios from "axios";
import './Journal.css';

const Journals = () => {
    const [journals, setJournals] = useState([]);

    useEffect(() => {
        const fetchJournals = async () => {
            try {
                const response = await axios.get('https://localhost:7221/Journals'); // Endpoint del backend
                console.log('Response data:', response.data); // Log para verificar el formato de la respuesta
                
                // Verificar si la respuesta es un array
                if (Array.isArray(response.data)) {
                    setJournals(response.data);
                } else {
                    console.error('Expected an array but received:', typeof response.data);
                }
            } catch (error) {
                console.error('Error fetching journals:', error);
            }
        };

        fetchJournals();
    }, []);

    const handleViewPDF = (id) => {
        // Redirigir al endpoint que muestra el PDF
        window.open(`https://localhost:7221/Journals/ViewPDF/${id}`, '_blank');
    };

    return (
        <div className="wrapper">
            <h1>Journals</h1>
            <table className="table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {Array.isArray(journals) && journals.length > 0 ? (
                        journals.map(journal => (
                            <tr key={journal.journalId}>
                                <td data-label="ID">{journal.journalId}</td>
                                <td data-label="Name">{journal.fileName}</td>
                                <td data-label="Actions">
                                    <button 
                                        className="view-button" 
                                        onClick={() => handleViewPDF(journal.journalId)}
                                    >
                                        View
                                    </button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="3">No journals found</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default Journals;
