import React, { useState } from 'react';
import { RootsController } from '../../../controllers/RootsController';

interface CreateRootPopupProps {
    onClose: () => void;
    onRootCreated: () => void;
  }
  
  const CreateRootPopup: React.FC<CreateRootPopupProps> = ({ onClose, onRootCreated }) => {
  const [name, setName] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = async () => {
    if (!name || !password) {
      alert('Please fill in all fields');
      return;
    }
    await RootsController.createRoot({ name, password });
    onRootCreated();
    onClose();
  };

  return (
    <div className="popup">
      <div className="popup-inner">
        <label htmlFor="name">Name:</label>
        <input
          id="name"
          type="text"
          value={name}
          onChange={e => setName(e.target.value)}
        />
        <label htmlFor="password">Password:</label>
        <input
          id="password"
          type="password"
          value={password}
          onChange={e => setPassword(e.target.value)}
        />
        <button onClick={handleSubmit}>Create</button>
        <button onClick={onClose}>Close</button>
      </div>
    </div>
  );
};

export default CreateRootPopup;