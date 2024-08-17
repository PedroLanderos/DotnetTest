import React, { useEffect, useState } from "react";
import axios from "axios";
import './Journal.css';

const Journals = () => {
    const [journals, setJournals] = useState([]);

    useEffect(() => {
        const fetchJournals = async () => {
            try {
                //using axios to do https requests.
                //get all the journals by using the endpoint from the backend 
                const response = await axios.get('https://localhost:7221/Journals'); 
                //log to debug data
                console.log('Response data:', response.data); 
                
                //Array of objects (JSON)
                if (Array.isArray(response.data)) {
                    setJournals(response.data);
                } else {
                    console.error('Data error:', typeof response.data);
                }
            } catch (error) {
                console.error('Journals error::', error);
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
                    {Array.isArray(journals) && journals.length > 0 ? ( //show the response into tables (all the journals) by using a map structure (index and value) 
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
