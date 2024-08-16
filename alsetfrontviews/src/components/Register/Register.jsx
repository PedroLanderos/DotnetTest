import React, { useState } from "react";
import { FaUser } from "react-icons/fa";
import './Register.css';
import { useNavigate } from 'react-router-dom'; // Importa useNavigat

const Register = () => {
    const [name, setName] = useState("");
    const navigate = useNavigate(); // Usa useNavigate
    const handleSubmit = async (e) => {
        e.preventDefault();

        // Realizar la solicitud al endpoint
        try {
            const response = await fetch('https://localhost:7221/Researchers/Create', { // Asegúrate de ajustar la URL
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ Name: name }),
            });

            if (response.ok) {
                navigate('/Login');
                console.log('Registro exitoso');
            } else {
                console.error('Error al registrar');
            }
        } catch (error) {
            console.error('Error al realizar la solicitud:', error);
        }
    };

    return (
        <div className="register-wrapper">
            <form onSubmit={handleSubmit}>
                <h1>Register</h1>
                <div className="input-box">
                    <input
                        type="text"
                        placeholder="Name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                    />
                    <FaUser className="icon" />
                </div>
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default Register;
