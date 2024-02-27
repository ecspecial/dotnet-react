// src/components/NavBar.tsx
import React from 'react';
import { NavLink } from 'react-router-dom';
import './NavBar.css';

interface NavBarProps {
    isAuthenticated: boolean;
    rid: string | null;
  }
  
  const NavBar: React.FC<NavBarProps> = ({ isAuthenticated, rid }) => {
    return (
      <nav className="navigation">
        <NavLink to="/roots" className={({ isActive }) => (isActive ? 'active' : '')} end>roots</NavLink>
        {isAuthenticated && rid && (
          <NavLink to={`/roots/${rid}/tasks`} className={({ isActive }) => (isActive ? 'active' : '')} end>tasks</NavLink>
        )}
        <NavLink to="/help" className={({ isActive }) => (isActive ? 'active right' : 'right')} end>help</NavLink>
      </nav>
    );
};

export default NavBar;