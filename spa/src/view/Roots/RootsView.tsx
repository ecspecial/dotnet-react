import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { RootsController } from '../../controllers/RootsController';
import { IRoot } from '../../model/RootsService';
import AuthPopup from '../components/AuthPopup/AuthPopup';
import CreateRootPopup from '../components/CreateRootPopup/CreateRootPopup';
import './RootsView.css';

interface RootsViewProps {
  onAuthChange: () => void;
  isAuthenticated: boolean;
}

const RootsView: React.FC<RootsViewProps> = ({ onAuthChange, isAuthenticated }) => {
  const navigate = useNavigate();
  
  const [roots, setRoots] = useState<IRoot[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [showAuthPopup, setShowAuthPopup] = useState<boolean>(false);
  const [selectedRoot, setSelectedRoot] = useState<IRoot | null>(null);
  const [showCreatePopup, setShowCreatePopup] = useState(false);
  const { id } = useParams();

  const fetchRoots = async () => {
    try {
      setLoading(true);
      let rootsData = await RootsController.fetchRoots({ limit: 10, offset: 0 });
      if (isAuthenticated && id) {
        rootsData = rootsData.filter(root => root.rid === id);
      }
      setRoots(rootsData);
    } catch (error) {
      console.error('Error fetching roots:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRoots();
  }, [isAuthenticated, id]);

  const handleRootClick = (root: IRoot) => {
    if (isAuthenticated && root.rid === localStorage.getItem('rid')) {
      navigate(`/roots/${root.rid}`);
    } else {
      setSelectedRoot(root);
      setShowAuthPopup(true);
    }
  };

  const handleCreateRoot = () => {
    setShowCreatePopup(true);
  };

  const handleRootCreated = () => {
    setShowCreatePopup(false);
    // Refresh the roots list after a new root is created
    fetchRoots();
  };

  return (
    <div className='roots'>
      <h2>Roots</h2>
      <button onClick={handleCreateRoot}>Create Root</button>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <ul>
          {roots.map((root) => (
            <li key={root.rid} onClick={() => handleRootClick(root)}>
              [{root.rid}] {root.name}
            </li>
          ))}
        </ul>
      )}
      {showAuthPopup && selectedRoot && (
        <AuthPopup 
          root={selectedRoot}
          onClose={() => setShowAuthPopup(false)}
          onAuthChange={onAuthChange}
        />
      )}
      {showCreatePopup && (
        <CreateRootPopup
          onClose={() => setShowCreatePopup(false)}
          onRootCreated={handleRootCreated}
        />
      )}
    </div>
  );
};

export default RootsView;