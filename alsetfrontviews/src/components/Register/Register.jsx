import React, { useState } from "react";
import { FaUser } from "react-icons/fa";
import './Register.css';
import { useNavigate } from 'react-router-dom'; 

const Register = () => {
    const [name, setName] = useState("");
    const navigate = useNavigate(); //init a react router function to include the posiblity to navegate from this component
    
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('https://localhost:7221/Researchers/Create', {//invoke the endpoint to create a new user based on the naem
                method: 'POST', 
                headers: {
                    'Content-Type': 'application/json', //the body is in json
                },
                body: JSON.stringify({ Name: name }), //transform the input into json
            });

            if (response.ok) {
                navigate('/Login');
            } else {
                console.error('Error');
            }
        } catch (error) {
            console.error('Error:', error);
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
