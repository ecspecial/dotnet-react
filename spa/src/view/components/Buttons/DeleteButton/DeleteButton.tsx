import React from 'react';

interface DeleteButtonProps {
  onDelete: () => void;
}

const DeleteButton: React.FC<DeleteButtonProps> = ({ onDelete }) => {
  return (
    <button className="delete-button" onClick={onDelete}>
      Delete
    </button>
  );
};

export default DeleteButton;