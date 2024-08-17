import React, { useState, useContext, useEffect } from "react";
import axios from "axios";
import { FaUser } from "react-icons/fa";
import { useNavigate, Link } from 'react-router-dom'; // Importa Link
import { AuthContext } from '../AuthContext'; 
import './Login.css';

const Login = () => {
    const [id, setId] = useState('');
    const { researcherId, setResearcherId } = useContext(AuthContext); // Usa el contexto
    const navigate = useNavigate(); 

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

    // Log el ID guardado cada vez que researcherId cambie
    useEffect(() => {
        if (researcherId) {
            console.log('ID guardado en el contexto:', researcherId);
        }
    }, [researcherId]); // Ejecuta cuando researcherId cambie

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
