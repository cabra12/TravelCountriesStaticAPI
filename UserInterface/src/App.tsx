
import { useState, useEffect } from 'react';
import './App.css'

interface RouteResponse {
  start?: string;
  destination?: string;
  route: string[];
  message?: string; 
}
const API_BASE_URL = import.meta.env.VITE_API_URL;

function App() {
  const initialDestination = window.location.pathname.replace("/", "").toUpperCase();

  const [start, setStart] = useState("USA");
  const [destination, setDestination] = useState(initialDestination);
  const [route, setRoute] = useState<string[]>([]);
  const [error, setError] = useState("");

  const fetchRoute = async (targetDestination: string) => {
    if (!targetDestination.trim() || !start.trim()) {
      setError("Start and destination are required.");
      setRoute([]);
      return;
    }

    try {
      window.history.pushState({}, "", `/${targetDestination}${start === 'USA' ? '' : `?start=${start}`}`);
      
      const response = await fetch(`${API_BASE_URL}/${targetDestination}${start === 'USA' ? '' : `?start=${start}`}`);
      const data = await response.json() as RouteResponse;
      
      if (!response.ok) {
        setError(data.message || "Route not found");
        setRoute([]);
      } else {
        setRoute(data.route);
        setError("");
      }
    } catch (err) {
      setError("Network error connecting to API.");
    }
  };

  useEffect(() => {
    if (initialDestination) {
      fetchRoute(initialDestination);
    }
  }, []);

  const handleFindRoute = () => {
    fetchRoute(destination);
  };

  return (
    <>
      <div className="container">
        <div className="card">
          <h2 className="title">Travel Logistics</h2>

          <div className="input-group">
            <label htmlFor="start-input">Start Country (3-Letter Code):</label>
            <input 
              id="start-input"
              type="text" 
              value={start} 
              onChange={(e) => setStart(e.target.value.toUpperCase())}
              className="input-field"
              maxLength={3}
            />
          </div>

          <div className="input-group">
            <label htmlFor="destination-input">Destination (3-Letter Code):</label>
            <input 
              id="destination-input"
              type="text" 
              value={destination} 
              onChange={(e) => setDestination(e.target.value.toUpperCase())}
              className="input-field"
              maxLength={3}
            />
          </div>

          <button onClick={handleFindRoute} className="submit-button">
            Find Route
          </button>

          <div className="output-zone">
            {error && <span className="error-text">{error}</span>}
            {route.length > 0 && <span>{route.join(" ➔ ")}</span>}
          </div>
        </div>
      </div>
    </>
  )
}

export default App
