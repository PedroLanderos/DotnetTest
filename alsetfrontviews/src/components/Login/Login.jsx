import React, { useState, useContext, useEffect } from "react";
import axios from "axios";
import { FaUser } from "react-icons/fa";
import { useNavigate, Link } from 'react-router-dom';
import { AuthContext } from '../AuthContext'; //use the created context to save the currentID after login
import './Login.css';

const Login = () => {
    const [id, setId] = useState('');
    //an object to storage and set the researcher that invoke the login
    const { researcherId, setResearcherId } = useContext(AuthContext); 
    const navigate = useNavigate(); //init a react router function to include the posiblity to navegate from this component

    //main button event to handle the login
    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            //get the data from the id using the researchers endpoint (see back end documentation)
            const response = await axios.get(`https://localhost:7221/Researchers/Details/${id}`);
            if (response.status === 200) {
                setResearcherId(id); 
                console.log('Login successful:', response.data);
                navigate('/researchers');  
            }
        } catch (error) {
            console.error('Login failed:', error);
        }
    };
    useEffect(() => {
        if (researcherId) {
            console.log('ID guardado en el contexto:', researcherId);
        }
    }, [researcherId]); 

    return (
        <div className="login-wrapper">
            <form onSubmit={handleLogin}>
                <h1>Login</h1>
                <div className="input-box">
                    <input 
                        type="text" 
                        placeholder="ID" 
                        value={id} 
                        onChange={(e) => setId(e.target.value)} 
                        required 
                    />
                    <FaUser className="icon"/>
                </div>
                <button type="submit">Login</button>
                <div className="register-link">
                    <p>Don't have an account? <Link to="/register">Register</Link></p> {/* Usa Link en lugar de <a> */}
                </div>
            </form>
        </div>
    );
}

export default Login;
