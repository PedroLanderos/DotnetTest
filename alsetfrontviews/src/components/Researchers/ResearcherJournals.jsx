import React, { useEffect, useState } from "react";
import axios from "axios";
import './ResearcherJournal.css';
import { useParams, useNavigate } from "react-router-dom";

//shows only the journals from a specific researcher  
const ResearcherJournals = () => {
    const [journals, setJournals] = useState([]);
    const [isSubscribed, setIsSubscribed] = useState(false);
    const { currentResearcherId, researcherId } = useParams();
    const navigate = useNavigate(); //init a react router function to include the posiblity to navegate from this component

    //onload
    useEffect(() => {
        if (!currentResearcherId) {
            console.error('Current researcher ID is not available');
            return;
        }
        //if the current researcher is not subscribed then won't be able to see the journalies
        const checkSubscription = async () => {
            try {
                //(see backend documentation to get deeper into the explanation)
                //the response 200 is a the expected so we don't need data from the endpoint
                const response = await axios.get(`https://localhost:7221/Subscriptions/IsSubscribed/${currentResearcherId}/${researcherId}`);
                setIsSubscribed(response.status === 200); 
                fetchJournals();
            } catch (error) {
                console.error('error:', error);
                setIsSubscribed(false);
            }
        };
        //see backend documentation to get deeper into the explanation
        const fetchJournals = async () => {
            try {
                //the response is an array of objects that contains data from the journal of the pointed researcher
                const response = await axios.get(`https://localhost:7221/Journals/GetJournalsByResearcher/${researcherId}`);
                if (Array.isArray(response.data)) {
                    setJournals(response.data);
                } else {
                    console.error('Data received:', typeof response.data);
                }
            } catch (error) {
                console.error('Error:', error);
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
                                            onClick={() => navigate(`/pdf-viewer?id=${journal.journalId}`)}
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
