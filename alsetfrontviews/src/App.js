import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { AuthProvider } from './components/AuthContext'; // Importa el AuthProvider
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import PDFViewer from './components/PDFvw/PDFViewer';
import Researchers from './components/Researchers/Researchers';
import Journals from './components/Journals/Journal';

function App() {
    return (
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/pdf-viewer" element={<PDFViewer />} />
                    <Route path="/researchers" element={<Researchers />} />
                    <Route path="/journals" element={<Journals />} />
                </Routes>
            </Router>
        </AuthProvider>
    );
}

export default App;
