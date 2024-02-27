// SubmitButton.tsx
import React from 'react';

interface UpdateButtonProps {
  onClick: () => void;
}

const UpdateButton: React.FC<UpdateButtonProps> = ({ onClick }) => {
  return (
    <button className="submit-button" onClick={onClick}>
      Update
    </button>
  );
};

export default UpdateButton;