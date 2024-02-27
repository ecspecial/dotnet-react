import React from 'react';
import { render, screen } from '@testing-library/react';
import App from '../App';

describe('App Navigation', () => {
  test('renders the main page content', () => {
    render(<App />);
    const mainContent = screen.getByText(/Root app/i);
    expect(mainContent).toBeInTheDocument();
  });

  test('renders NavBar and checks for links', () => {
    render(<App />);
    const navBar = screen.getByRole('navigation');
    expect(navBar).toBeInTheDocument();

    // Use getByRole to target links specifically
    expect(screen.getByRole('link', { name: /root/i })).toBeInTheDocument();
    expect(screen.getByRole('link', { name: /grid/i })).toBeInTheDocument();
    expect(screen.getByRole('link', { name: /tree/i })).toBeInTheDocument();
    expect(screen.getByRole('link', { name: /help/i })).toBeInTheDocument();
  });
});