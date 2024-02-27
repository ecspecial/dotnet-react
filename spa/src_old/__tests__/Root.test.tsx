import React from 'react';
import axios from 'axios';
import { render, fireEvent, waitFor, screen } from '@testing-library/react';
import Root from '../components/root/Root'; // Assuming your component file is named Root.tsx

jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Root component', () => {
  beforeEach(() => {
    // Mock axios get and post methods
    mockedAxios.get.mockResolvedValueOnce({
      data: [{ rid: '1', name: 'Root1', password: 'password1' }]
    }).mockResolvedValueOnce({
      data: { rid: '1', name: 'Root1', password: 'password1' } // Mock response for fetching single root by ID
    });
  });

  afterEach(() => {
    // Clear mocks after each test
    jest.clearAllMocks();
  });

  test('renders root component', async () => {
    render(<Root />);
    // Check if the component renders successfully
    const rootElement = screen.getByText(/Root App/i);
    expect(rootElement).toBeInTheDocument();

    // Wait for roots to be fetched
    await waitFor(() => {
      const rootItem = screen.getByText(/rid: \[ 1 \]/i);
      expect(rootItem).toBeInTheDocument();
    });
  });

  test('fetches single root by ID', async () => {
    render(<Root />);
  
    // Simulate authorization
    localStorage.setItem('bearerToken', 'mockBearerToken'); // Mock bearer token
    localStorage.setItem('rid', '1'); // Mock root ID
    localStorage.setItem('name', 'Root1'); // Mock root name
    fireEvent.click(screen.getByText(/Authorize/i)); // Click authorize button
  
    const findSingleRootButton = screen.getByText(/Find Single Root/i);
    fireEvent.click(findSingleRootButton);
  
    // Wait for the single root to be fetched
    await waitFor(() => {
      expect(screen.getByText(/rid: \[ 1 \]/i)).toBeInTheDocument();
    });
  
    // Separately wait for the edit button to ensure its presence is also verified
    await waitFor(() => {
      expect(screen.getByText(/Edit/i)).toBeInTheDocument();
    });
  });

  test('open and close create modal', async () => {
    render(<Root />);
    // Simulate opening the create modal
    fireEvent.click(screen.getByText(/Create Root/i));
    expect(screen.getByText(/Submit/i)).toBeInTheDocument();
  
    // Simulate closing the create modal
    fireEvent.click(screen.getByText(/Cancel/i));
    await waitFor(() => {
      expect(screen.queryByText(/Submit/i)).not.toBeInTheDocument();
    });
  });

  test('refresh roots list', async () => {
    render(<Root />);
    // Simulate clicking refresh button
    fireEvent.click(screen.getByText(/Refresh Roots/i));
  });

  test('open and close authorization popup', async () => {
    render(<Root />);
    
    // Open the authorization popup by clicking the "Authorize" button.
    // Use getByRole for a more specific target if the buttons have unique roles or titles, or use the index from findAllByText.
    const authorizeButtons = await screen.findAllByText(/Authorize/i);
    fireEvent.click(authorizeButtons[0]); // Assuming the first button is to open the popup
  
    // Check if the popup is visible by looking for a unique element within it, such as the input for "ID"
    expect(screen.getByLabelText(/ID:/i)).toBeInTheDocument();
  
    // Close the authorization popup by clicking the "Cancel" button.
    fireEvent.click(screen.getByText(/Cancel/i));
  });
});