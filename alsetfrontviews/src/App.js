import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { AuthProvider } from './components/AuthContext'; 
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import PDFViewer from './components/PDF/PDFViewer';
import Researchers from './components/Researchers/Researchers';
import Journals from './components/Journals/Journal';
import ResearcherJournals from './components/Researchers/ResearcherJournals';
import UploadJournal from './components/Journals/UploadJournal';

function App() {
    return (
        <div>
            <AuthProvider>
                <Router>
                    <Routes>
                        <Route path="/login" element={<Login />} />
                        <Route path="/researcher-journals/:currentResearcherId/:researcherId" element={<ResearcherJournals />} />
                        <Route path="/researchers" element={<Researchers />} />
                        <Route path="/journals" element={<Journals />} />
                        <Route path="/register" element={<Register />} />
                        <Route path='upload-journal' element={< UploadJournal/>}/>
                    </Routes>
                </Router>
            </AuthProvider>
        </div>
    );
}

export default App;
