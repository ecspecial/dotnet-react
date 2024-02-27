import React from 'react';
import { render, fireEvent, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import Grid from '../components/grid/Grid';
import { MemoryRouter } from 'react-router-dom'; // Import MemoryRouter

describe('Grid Component', () => {
    test('shows create task form on button click', () => {
        render(
            <MemoryRouter>
              <Grid />
            </MemoryRouter>
          );
      
      // Initially, the form should not be visible
      expect(screen.queryByPlaceholderText('Task Name')).not.toBeInTheDocument();
      expect(screen.queryByPlaceholderText('Task Description')).not.toBeInTheDocument();
  
      // Simulate clicking the 'Create New Task' button
      fireEvent.click(screen.getByText('Create New Task'));
  
      // The form should now be visible
      expect(screen.getByPlaceholderText('Task Name')).toBeInTheDocument();
      expect(screen.getByPlaceholderText('Task Description')).toBeInTheDocument();
    });
  
    test('hides form and clears fields when cancel is clicked', () => {
        render(
            <MemoryRouter>
              <Grid />
            </MemoryRouter>
          );
      
      // Open the form
      fireEvent.click(screen.getByText('Create New Task'));
  
      // Fill in the form fields
      fireEvent.change(screen.getByPlaceholderText('Task Name'), { target: { value: 'Test Task' } });
      fireEvent.change(screen.getByPlaceholderText('Task Description'), { target: { value: 'Test Description' } });
  
      // Now cancel the creation
      fireEvent.click(screen.getByText('Cancel'));
  
      // Form should be hidden now
      expect(screen.queryByPlaceholderText('Task Name')).not.toBeInTheDocument();
      expect(screen.queryByPlaceholderText('Task Description')).not.toBeInTheDocument();
  
      // If form is opened again, fields should be cleared
      fireEvent.click(screen.getByText('Create New Task'));
      expect((screen.getByPlaceholderText('Task Name') as HTMLInputElement).value).toBe('');
      expect((screen.getByPlaceholderText('Task Description') as HTMLInputElement).value).toBe('');
    });
  
    // Add more tests as needed for different interactions
  });