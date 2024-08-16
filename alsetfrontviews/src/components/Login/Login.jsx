import React from "react"
import { FaUser } from "react-icons/fa";
import './Login.css'

const Login = () => {
    return (
        <div className="wrapper">
            <form action="">
                <h1>Login</h1>
                <div className="input-box">
                    <input type="text" placeholder="Name" required/>
                    <FaUser className="icon"/>
                </div>
                <button type="submit">Login</button>
                <div className="register-link">
                    <p>Don't have an account? <a href="#">Register</a></p>
                </div>
            </form>
        </div>
    )
}

export default Login;