import React, { useEffect, useState } from "react";
import axios from "axios";
import './PDFViewer.css'; // Asegúrate de crear este archivo CSS

const PDFViewer = () => {
    const [filePath, setFilePath] = useState('');
    const query = new URLSearchParams(window.location.search);
    const journalId = query.get('id'); // Obtener la ID del journal de la URL

    useEffect(() => {
        const fetchFilePath = async () => {
            try {
                const response = await axios.get(`https://localhost:7221/GetFilePath/${journalId}`);
                setFilePath(response.data); // Establecer la ruta del archivo
            } catch (error) {
                console.error('Error fetching file path:', error);
            }
        };

        if (journalId) {
            fetchFilePath();
        }
    }, [journalId]);

    return (
        <div className="pdf-viewer-wrapper">
            <h1>PDF Viewer</h1>
            <div className="pdf-container">
                {filePath && (
                    <embed
                        src={`https://localhost:7221/${filePath}#toolbar=0`}
                        type="application/pdf"
                        className="pdf-embed"
                    />
                )}
            </div>
        </div>
    );
};

export default PDFViewer;
