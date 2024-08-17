import React, { useState, useContext, useEffect } from "react";
import axios from "axios";
import { useNavigate } from 'react-router-dom'; 
import { AuthContext } from '../AuthContext'; 
import './UploadJournal.css';

const UploadJournal = () => {
    const [description, setDescription] = useState('');
    const [file, setFile] = useState(null);
    const { researcherId, setResearcherId } = useContext(AuthContext); 
    const navigate = useNavigate(); 

    const handleLogin = async (e) => {
        e.preventDefault();
        if (!file) {
            console.error('No file selected.');
            return;
        }

        const formData = new FormData();
        formData.append('ResearcherId', researcherId);
        formData.append('Description', description);
        formData.append('UploadedFile', file);

        try {
            const response = await axios.post('https://localhost:7221/Journals/CreateNew', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });

            if (response.status === 200) {
                console.log('Upload successful:', response.data);
                navigate('/researchers'); // Redirige a la página de Researchers
            }
        } catch (error) {
            console.error('Upload failed:', error);
        }
    };

    // Log el ID guardado cada vez que researcherId cambie
    useEffect(() => {
        if (researcherId) {
            console.log('ID guardado en el contexto:', researcherId);
        }
    }, [researcherId]);

    return (
        <div className="login-wrapper">
            <form onSubmit={handleLogin}>
                <h1>Upload file</h1>
                <div className="input-box">
                    <input 
                        type="text" 
                        placeholder="Description" 
                        value={description} 
                        onChange={(e) => setDescription(e.target.value)} 
                        required 
                    />
                </div>
                <div className="file-upload-box">
                    <button type="button" onClick={() => document.getElementById('fileInput').click()}>
                        Select File
                    </button>
                    <input 
                        type="file" 
                        id="fileInput" 
                        style={{ display: 'none' }} 
                        accept=".pdf" 
                        onChange={(e) => setFile(e.target.files[0])}
                    />
                    {file && <p>Selected file: {file.name}</p>}
                </div>
                <button type="submit">Upload</button>
            </form>
        </div>
    );
}

export default UploadJournal;