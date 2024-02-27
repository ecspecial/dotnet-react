import React, { useState } from 'react';
import { IRoot } from '../../model/RootsService';
import SignoffButton from '../components/Buttons/SignoffButton/SignoffButton';
import CloseButton from '../components/Buttons/CloseButton/CloseButton';
import UpdateButton from '../components/Buttons/UpdateButton/UpdateButton';
import DeleteButton from '../components/Buttons/DeleteButton/DeleteButton';

interface UpdateRootPopupProps {
  root: IRoot;
  onClose: () => void;
  onUpdate: (name: string, password: string) => void;
  onSignOff: () => void;
  onDelete: () => void; // Add onDelete prop
}

const UpdateRootPopup: React.FC<UpdateRootPopupProps> = ({ root, onClose, onUpdate, onSignOff, onDelete }) => {
  const [name, setName] = useState(root.name);
  const [password, setPassword] = useState('');

  return (
    <div className="popup">
      <div className="popup-inner">
        <label htmlFor="name">Name:</label>
        <input id="name" type="text" value={name} onChange={e => setName(e.target.value)} />
        <label htmlFor="password">Password:</label>
        <input id="password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
        <UpdateButton onClick={() => onUpdate(name, password)} />
        <DeleteButton onDelete={onDelete} />  {/* Use the DeleteButton here */}
        <SignoffButton onClick={onSignOff} />
        <CloseButton onClose={onClose} />
      </div>
    </div>
  );
};

export default UpdateRootPopup;