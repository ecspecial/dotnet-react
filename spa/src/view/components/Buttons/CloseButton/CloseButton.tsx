// CloseButton.jsx
import React from 'react';

interface CloseButtonProps {
  onClose: () => void;
}

const CloseButton: React.FC<CloseButtonProps> = ({ onClose }) => {
  return (
    <button className="close-button" onClick={onClose}>
      Close
    </button>
  );
};

export default CloseButton;