import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { AuthContext } from '../AuthContext'; // Importa el contexto
import './Researchers.css';
import { useNavigate } from 'react-router-dom';

const Researchers = () => {
    const [researchers, setResearchers] = useState([]);
    const { researcherId, setResearcherId } = useContext(AuthContext); // Usa el contexto para obtener el ID del investigador
    const navigate = useNavigate();

    const handleViewJournals = (id) => {
        setResearcherId(researcherId);
        navigate(`/researcher-journals/${researcherId}/${id}`); // Redirige a la nueva vista
    };

    useEffect(() => {
        const fetchResearchers = async () => {
            try {
                const response = await axios.get('https://localhost:7221/Researchers'); // Endpoint del backend
                console.log('Response data:', response.data); // Log para verificar el formato de la respuesta
                
                // Verificar si la respuesta es un array
                if (Array.isArray(response.data)) {
                    setResearchers(response.data);
                } else {
                    console.error('Expected an array but received:', typeof response.data);
                }
            } catch (error) {
                console.error('Error fetching researchers:', error);
            }
        };

        fetchResearchers();
    }, []);

    const handleSubscribe = async (id) => {
        try {
            const response = await axios.post('https://localhost:7221/Subscriptions/Create', {
                SubscriberId: researcherId,
                SubscribedToId: id
            });

            if (response.status === 200) {
                console.log(`Successfully subscribed to researcher with ID: ${id}`);
            } else {
                console.error('Failed to subscribe:', response.status);
            }
        } catch (error) {
            console.error('Error subscribing to researcher:', error);
        }
    };

    return (
        <div className="researchers-wrapper">
            <div className="header">
                <h1>Researchers</h1>
                <button 
                    className="subscribe-button" 
                    onClick={() => navigate('/upload-journal')}
                >
                    Upload
                </button>
            </div>
            <table className="table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Joined Date</th>
                        <th>Total Journals Uploaded</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {Array.isArray(researchers) && researchers.length > 0 ? (
                        researchers.map(researcher => (
                            <tr key={researcher.researcherId}>
                                <td>{researcher.researcherId}</td>
                                <td>{researcher.name}</td>
                                <td>{new Date(researcher.joinedDate).toLocaleDateString()}</td>
                                <td>{researcher.totalJournalsUploaded}</td>
                                <td>
                                    <button 
                                        className="subscribe-button" 
                                        onClick={() => handleSubscribe(researcher.researcherId)}
                                    >
                                        Subscribe
                                    </button>
                                </td>
                                <td>
                                    <button 
                                        className="view-button" 
                                        onClick={() => handleViewJournals(researcher.researcherId)}
                                    >
                                        View Journals
                                    </button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="6">No researchers found</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default Researchers;
