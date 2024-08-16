import React, { useState, useContext } from "react";
import axios from "axios";
import { FaUser } from "react-icons/fa";
import { useNavigate } from 'react-router-dom'; // Importa useNavigate
import { AuthContext } from '../AuthContext'; // Importa el contexto
import './Login.css';

const Login = () => {
    const [id, setId] = useState('');
    const { setResearcherId } = useContext(AuthContext); // Usa el contexto
    const navigate = useNavigate(); // Usa useNavigate

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.get(`https://localhost:7221/Researchers/Details/${id}`);
            if (response.status === 200) {
                setResearcherId(id); // Almacena el ID en el contexto
                console.log('Login successful:', response.data);
                navigate('/researchers'); // Redirige a la página de Researchers
            }
        } catch (error) {
            console.error('Login failed:', error);
        }
    };

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
                    <p>Don't have an account? <a href="#">Register</a></p>
                </div>
            </form>
        </div>
    );
}

export default Login;
