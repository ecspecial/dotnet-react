// SignoffButton.jsx
import React from 'react';

interface SignoffButtonProps {
  onClick: () => void;
}

const SignoffButton: React.FC<SignoffButtonProps> = ({ onClick }) => {
  return (
    <button className='signoff-button' onClick={onClick}>
      Sign Off
    </button>
  );
};

export default SignoffButton;