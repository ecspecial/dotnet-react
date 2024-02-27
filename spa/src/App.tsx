// App.tsx
import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import NavBar from './view/components/NavBar/NavBar';
import './App.css';
import RootsView from './view/Roots/RootsView';
import HelpView from './view/Help/HelpView';
import TasksView from './view/Tasks/TasksView';
import RootDetailView from './view/Roots/RootsDetailView';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(!!localStorage.getItem('bearerToken'));
  const [rid, setRid] = useState(localStorage.getItem('rid'));
  
  const handleAuthChange = () => {
    setIsAuthenticated(!!localStorage.getItem('bearerToken'));
    setRid(localStorage.getItem('rid'));
  };

  return (
    <Router>
      <div>
        <NavBar isAuthenticated={isAuthenticated} rid={rid} />
        <div className="main-content">
          <Routes>
            <Route path="/roots" element={<RootsView onAuthChange={handleAuthChange} isAuthenticated={isAuthenticated} />} />
            <Route path="/roots/:id" element={<RootDetailView />} />
            {isAuthenticated && rid && (
              <Route path={`/roots/:rid/tasks`} element={<TasksView />} />
            )}
            <Route path="/help" element={<HelpView />} />
            <Route path="*" element={<Navigate replace to="/roots" />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;