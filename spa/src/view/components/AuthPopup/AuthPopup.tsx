import React, { useState } from 'react';
import { RootsController } from '../../../controllers/RootsController';
import { useNavigate } from 'react-router-dom';
import { IRoot } from '../../../model/RootsService';
import SigninButton from '../Buttons/SigninButton/SigninButton';
import CloseButton from '../Buttons/CloseButton/CloseButton';
import SignoffButton from '../Buttons/SignoffButton/SignoffButton';
import './AuthPopup.css';

interface AuthPopupProps {
  root: IRoot;
  onClose: () => void;
  onAuthChange: () => void;
}

const AuthPopup: React.FC<AuthPopupProps> = ({ root, onClose, onAuthChange }) => {
  const navigate = useNavigate();

  const [password, setPassword] = useState('');
  const isAuthorized = localStorage.getItem('rid') === root.rid && localStorage.getItem('bearerToken');

  const handleAuth = async () => {
    if (!isAuthorized && password) {
      const success = await RootsController.authRoot({ ...root, password });
      if (success) {
        alert('Authorization success');
        onAuthChange();
        navigate(`/roots/${root.rid}`); // Redirect on successful auth
      } else {
        alert('Authorization failed');
      }
    } else if (!password) {
      alert('Please provide a password');
    }
  };

  const handleUnauth = async () => {
    // Assuming RootsController has a method unauthRoot
    await RootsController.unauthRoot();
    alert('Successfully signed off');
    onAuthChange();
    onClose();
  };

  return (
    <div className="popup">
      <div className="popup-inner">
        <h2>{isAuthorized ? 'Sign Off' : 'Authorize'}</h2>
        <input type="text" value={root.rid} readOnly />
        <div>
          {isAuthorized ? (
            <div className='auth-button-group'>
              <SignoffButton onClick={handleUnauth} />
              <CloseButton onClose={onClose} />
            </div>
          ) : (
            <>
              <input 
                type="password" 
                value={password} 
                onChange={(e) => setPassword(e.target.value)} 
                placeholder="Password" 
              />
              <div className='auth-button-group'>
                <SigninButton onClick={handleAuth} />
                <CloseButton onClose={onClose} />
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default AuthPopup;