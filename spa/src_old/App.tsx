import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Root from './components/root/Root';
import Grid from './components/grid/Grid';
import Help from './components/help/Help';
import './App.css';

// Компонент навигационного меню
const NavBar = () => (
  <nav className="navigation">
    <ul>
      <li><Link to="/roots">Root</Link></li>  {/* Ссылка на главную страницу */}
      <li><Link to="/roots/tasks/grid">Grid</Link></li>  {/* Ссылка на страницу с сеткой */}
      <li className="help"><Link to="/roots/help">Help</Link></li>  {/* Ссылка на страницу с помощью */}
    </ul>
  </nav>
);

function App() {
  return (
    <Router>
      <div>
        <NavBar />
        <div className="main-content">
          <Routes>
            <Route path="/roots" element={<Root />} />
            <Route path="/roots/tasks/grid" element={<Grid />} />
            <Route path="/roots/help" element={<Help />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
