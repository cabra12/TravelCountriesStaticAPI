import '@testing-library/jest-dom/vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import App from './App';

// Mock the global fetch API so we don't make real network requests during tests
globalThis.fetch = vi.fn();

describe('App Component - Travel Logistics', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    window.history.pushState = vi.fn();
  });

  it('renders default state correctly', () => {
    render(<App />);
    
    expect(screen.getByText('Travel Logistics')).toBeInTheDocument();
    expect(screen.getByLabelText(/Start Country/i)).toHaveValue('USA');
    expect(screen.getByLabelText(/Destination/i)).toHaveValue('');
  });

  it('converts lowercase typed input to uppercase automatically', () => {
    render(<App />);
    
    const destInput = screen.getByLabelText(/Destination/i);
    fireEvent.change(destInput, { target: { value: 'mex' } });
    
    expect(destInput).toHaveValue('MEX');
  });

  it('shows validation error and prevents fetch if destination is missing', () => {
    render(<App />);
    
    const submitButton = screen.getByText('Find Route');
    fireEvent.click(submitButton);
    
    expect(screen.getByText('Start and destination are required.')).toBeInTheDocument();
    expect(globalThis.fetch).not.toHaveBeenCalled();
  });

  it('fetches and displays route successfully, and updates the URL', async () => {
    const mockResponse = { route: ['USA', 'MEX', 'GTM', 'HND', 'NIC', 'CRI', 'PAN'] };
    (globalThis.fetch as any).mockResolvedValueOnce({
      ok: true,
      json: async () => mockResponse,
    });

    render(<App />);
    
    const destInput = screen.getByLabelText(/Destination/i);
    fireEvent.change(destInput, { target: { value: 'PAN' } });
    
    const submitButton = screen.getByText('Find Route');
    fireEvent.click(submitButton);

    await waitFor(() => {
        expect(screen.getByText('USA ➔ MEX ➔ GTM ➔ HND ➔ NIC ➔ CRI ➔ PAN')).toBeInTheDocument();
    });
    
    expect(globalThis.fetch).toHaveBeenCalledWith('https://travel-logistics-api-gzh7chc3gvg4eye8.northcentralus-01.azurewebsites.net/PAN');
    expect(window.history.pushState).toHaveBeenCalledWith({}, '', '/PAN');
  });

  it('displays API error message on failure', async () => {
    const mockErrorResponse = { message: 'No route found between these locations' };
    (globalThis.fetch as any).mockResolvedValueOnce({
      ok: false,
      json: async () => mockErrorResponse,
    });

    render(<App />);
    
    const destInput = screen.getByLabelText(/Destination/i);
    fireEvent.change(destInput, { target: { value: 'XYZ' } });
    
    const submitButton = screen.getByText('Find Route');
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText('No route found between these locations')).toBeInTheDocument();
    });
  });
});