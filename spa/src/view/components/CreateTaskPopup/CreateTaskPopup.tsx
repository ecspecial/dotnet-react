// CreateTaskPopup.js
import React, { useState } from 'react';
import { TaskController } from '../../../controllers/TaskController';
import CloseButton from '../Buttons/CloseButton/CloseButton';

interface CreateTaskPopupProps {
    rid: string;
    onClose: () => void;
    onTaskCreated: () => void;
  }
  
  const CreateTaskPopup: React.FC<CreateTaskPopupProps> = ({ rid, onClose, onTaskCreated }) => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');

  const handleSubmit = async () => {
    if (!name || !description) {
      alert('Please fill in all fields');
      return;
    }
    await TaskController.createTaskBase({ rid, name, description });
    onTaskCreated();
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
        <label htmlFor="password">Description:</label>
        <input
          id="description"
          type="text"
          value={description}
          onChange={e => setDescription(e.target.value)}
        />
        <button onClick={handleSubmit}>Create</button>
        <CloseButton onClose={onClose} />
      </div>
    </div>
  );
};

export default CreateTaskPopup;