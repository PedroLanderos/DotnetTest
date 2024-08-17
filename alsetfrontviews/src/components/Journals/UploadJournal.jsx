import React, { useState, useContext, useEffect } from "react";
import axios from "axios";
import { useNavigate } from 'react-router-dom'; 
import { AuthContext } from '../AuthContext';  //use the main context to preserv the current id
import './UploadJournal.css';

const UploadJournal = () => {
    //use the value par as a flag to show and send data
    const [description, setDescription] = useState('');
    const [file, setFile] = useState(null);
    const { researcherId, setResearcherId } = useContext(AuthContext); 
    const navigate = useNavigate(); //init a react router function to include the posiblity to navegate from this component

    //the event from the upload button that calls the backend api
    const handleLogin = async (e) => {
        e.preventDefault();
        //if the file is empty end the proccess 
        if (!file) {
            console.error('No file selected.');
            return;
        }
        
        //the data is send by multipart/form-data form, so an option could be create and object and attach the values from the main form 
        const formData = new FormData();
        formData.append('ResearcherId', researcherId);
        formData.append('Description', description);
        formData.append('UploadedFile', file);

        try {
            //call the endpoint to create a new register from the journals table (see backend apis documentation)
            const response = await axios.post('https://localhost:7221/Journals/CreateNew', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });

            if (response.status === 200) {
                console.log('Upload successful:', response.data);
                //if the repsonse is 200 (response ok) navegate to researches endpoint to see all the journals
                navigate('/researchers');  
            }
        } catch (error) {
            console.error('Upload failed:', error);
        }
    };

    //as for a reason the context get lost after going through the view button a log was needed in order to see the real time value
    useEffect(() => {
        if (researcherId) {
            console.log('id context:', researcherId);
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