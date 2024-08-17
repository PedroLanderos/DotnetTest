import React, { useEffect, useState } from "react";
import axios from "axios";
import './ResearcherJournal.css';
import { useParams } from "react-router-dom";

const ResearcherJournals = () => {
    const [journals, setJournals] = useState([]);
    const [isSubscribed, setIsSubscribed] = useState(false);
    const { currentResearcherId, researcherId } = useParams(); // Obtén ambos IDs desde los parámetros de la ruta

    console.log('Current Researcher ID:', currentResearcherId);
    console.log('Researcher to subscribe ID:', researcherId);
    
    useEffect(() => {
        if (!currentResearcherId) {
            console.error('Current researcher ID is not available.');
            return;
        }

        const checkSubscription = async () => {
            try {
                const response = await axios.get(`https://localhost:7221/Subscriptions/IsSubscribed/${currentResearcherId}/${researcherId}`);
                if (response.status === 200) {
                    setIsSubscribed(true);
                } else {
                    setIsSubscribed(false);
                }
                fetchJournals();
            } catch (error) {
                console.error('Error checking subscription:', error);
                setIsSubscribed(false);
            }
        };

        const fetchJournals = async () => {
            try {
                const response = await axios.get(`https://localhost:7221/Journals/GetJournalsByResearcher/${researcherId}`);
                console.log('Journals response data:', response.data);
                
                if (Array.isArray(response.data)) {
                    setJournals(response.data);
                } else {
                    console.error('Expected an array but received:', typeof response.data);
                }
            } catch (error) {
                console.error('Error fetching journals:', error);
            }
        };

        checkSubscription();
    }, [researcherId, currentResearcherId]);
    
    return (
        <div className="wrapper">
            <h1>Journals of Researcher {researcherId}</h1>
            <table className="table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {isSubscribed ? (
                        journals.length > 0 ? (
                            journals.map(journal => (
                                <tr key={journal.journalId}>
                                    <td>{journal.journalId}</td>
                                    <td>{journal.fileName}</td>
                                    <td>
                                        <button 
                                            className="view-button" 
                                            onClick={() => window.open(`https://localhost:7221/Journals/ViewPDF/${journal.journalId}`, '_blank')}
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
                        )
                    ) : (
                        <tr>
                            <td colSpan="3">You are not subscribed to this researcher.</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default ResearcherJournals;
