import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { RootsController } from '../../controllers/RootsController';
import { IRoot } from '../../model/RootsService';
import UpdateRootPopup from './UpdateRootPopup';

const RootDetailView = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [root, setRoot] = useState<IRoot | null>(null);
  const [showUpdatePopup, setShowUpdatePopup] = useState(false);

  // Define fetchRoot at the component level
  const fetchRoot = async () => {
    if (!id) return;
    const fetchedRoot = await RootsController.getRoot(id);
    if (!fetchedRoot || fetchedRoot.rid !== localStorage.getItem('rid')) {
      alert("You are not authorized to view this root.");
      navigate('/roots');
    } else {
      setRoot(fetchedRoot);
    }
  };

  const handleUpdate = async (name: string, password: string) => {
    if (!root || !id) return;
    await RootsController.updateRoot({ ...root, name, password });
    alert("Root updated.");
    fetchRoot();  // Refresh the root details
    setShowUpdatePopup(false);  // Close the popup
  };

  const handleDelete = async () => {
    if (!id) return;
    await RootsController.deleteRoot(id);
    alert("Root deleted.");
    navigate('/roots');
  };

  const handleSignOff = async () => {
    await RootsController.unauthRoot();
    alert("You have been signed off.");
    navigate('/roots');
  };

  useEffect(() => {
    fetchRoot();
  }, [id, navigate]);

  if (!root) return <div>Loading...</div>;

  return (
    <div>
      <ul>
        <li onClick={() => setShowUpdatePopup(true)}>{`[${root.rid}] ${root.name}`} - Click to edit</li>
      </ul>
      {showUpdatePopup && (
        <UpdateRootPopup
          root={root}
          onClose={() => setShowUpdatePopup(false)}
          onUpdate={handleUpdate}
          onSignOff={handleSignOff}
          onDelete={handleDelete}
        />
      )}
    </div>
  );
};

export default RootDetailView;