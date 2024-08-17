import React, { useEffect, useState } from "react";
import axios from "axios";
import './PDFViewer.css'; 

const PDFViewer = () => {
    const [filePath, setFilePath] = useState('');
    const query = new URLSearchParams(window.location.search);
    //the id from the journal is send by the url in the main app 
    const journalId = query.get('id'); 

    //onload
    useEffect(() => {
        const fetchFilePath = async () => {
            try {
                //invoke the endpoint that gives the route of the id archive 
                const response = await axios.get(`https://localhost:7221/GetFilePath/${journalId}`);
                setFilePath(response.data); 
            } catch (error) {
                console.error('error: ', error);
            }
        };

        if (journalId) {
            fetchFilePath();
        }
    }, [journalId]);

    return (
        <div className="pdf-viewer-wrapper">
            <div className="pdf-container">
                {filePath && (
                    <embed
                        src={`https://localhost:7221/${filePath}#toolbar=0`} //delete the tool bar so the user is not able to download the pdf
                        type="application/pdf"
                        className="pdf-embed"
                    />
                )}
            </div>
        </div>
    );
};

export default PDFViewer;
