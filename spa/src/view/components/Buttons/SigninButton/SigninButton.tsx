// SubmitButton.tsx
import React from 'react';

interface SubmitButtonProps {
  onClick: () => void;
}

const SubmitButton: React.FC<SubmitButtonProps> = ({ onClick }) => {
  return (
    <button className="submit-button" onClick={onClick}>
      Sign In
    </button>
  );
};

export default SubmitButton;