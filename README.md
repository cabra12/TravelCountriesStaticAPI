# Travel Logistics Route Planner

A full-stack route-planning application that determines the shortest sequence of North American countries a driver must travel through to reach a destination.

The core of the project is a C#/.NET Web API that models country borders as a graph and uses Breadth-First Search (BFS) to determine the shortest route. A React and TypeScript frontend was also created to provide an interactive interface for the API.

## Live Application

**Frontend:** `https://travel-logistics-ui-gsc9exabbjhfgcg5.northcentralus-01.azurewebsites.net/`

**Web API:** `https://travel-logistics-api-gzh7chc3gvg4eye8.northcentralus-01.azurewebsites.net/`

### Example API Requests

```text
https://travel-logistics-api-gzh7chc3gvg4eye8.northcentralus-01.azurewebsites.net/BLZ
```

The frontend also supports country codes directly in the URL:

```text
https://travel-logistics-ui-gsc9exabbjhfgcg5.northcentralus-01.azurewebsites.net/PAN
```

## API Usage

### Get API Information

```http
GET /
```

Returns basic information about the API, supported country codes, and usage.

### Find a Route

```http
GET /{destination}
```

The starting country defaults to `USA`.

### Optional Starting Country

The routing service supports other starting countries through an optional query parameter:

```http
GET /PAN?start=MEX
```


## Routing Algorithm

Country borders are represented as an unweighted, undirected graph.

Each country is a node and each shared border is an edge. Since crossing any border represents one hop, Breadth-First Search is used to find a route with the minimum number of border crossings.

For a request to travel from `USA` to `PAN`, BFS produces:

```text
USA -> MEX -> GTM -> HND -> NIC -> CRI -> PAN
```

### BFS Process

1. Add the starting country to a queue data structure (FIFO).
2. Track visited countries to prevent processing a country multiple times.
3. Explore neighboring countries level by level.
4. Record the country from which each newly discovered country was reached.
5. Once the destination is found, follow those predecessor relationships backward to reconstruct the route.
6. Reverse the reconstructed path to produce the final route from start to destination.

This implementation queues country codes rather than copying entire paths during the search.

## Assumptions and Design Decisions

### Shortest Border-Crossing Route

The application interprets the requested route as the route requiring the fewest border crossings. Because every border crossing has equal weight, BFS guarantees a shortest path in the graph.

### USA as the Default Origin

API starting location defaults to `USA`.

### Bidirectional Borders

Country borders are treated as bidirectional. If country A borders country B, travel is assumed to be possible from A to B and from B to A.

### Case-Insensitive Country Codes

Country lookups are case-insensitive. The frontend also normalizes typed country codes to uppercase.

### In-Memory Border Data

The supplied geographic dataset is small and static, so it is stored in memory rather than introducing an external database.

The backend uses a `FrozenDictionary` for the country map because the border relationships do not change while the application is running.

### API Response Structure

Successful routes are returned as an object:

```json
{
  "start": "USA",
  "destination": "BLZ",
  "route": ["USA", "MEX", "BLZ"]
}
```

## Error Handling

Invalid requests are handled through HTTP responses.

Examples include:

- `400 Bad Request` when required input is empty.
- `404 Not Found` when no route can be found for the supplied countries.
- `200 OK` when the starting country and destination are both `USA`.

The frontend displays API error messages to the user and separately handles network failures when the backend cannot be reached.

## Testing, CI/CD & Deployment

The project includes automated tests for both application layers:

- **Backend:** xUnit tests cover the BFS routing logic and API controller behavior.
- **Frontend:** Vitest and React Testing Library verify user interactions, validation, API integration behavior, and route rendering.

GitHub Actions runs the test suites as part of CI. The frontend and ASP.NET Core API are deployed independently to Azure App Service through automated GitHub Actions workflows.

## Architecture

The application separates HTTP handling, routing logic, geographic data, and presentation.

```text
React / TypeScript Frontend
            |
            | HTTP
            v
ASP.NET Core Web API
            |
            v
     RouteController
            |
            v
      RoutingService
            |
            v
     BorderRepository
```

## Technology Stack

| Area | Technologies |
|---|---|
| Backend | C#, .NET 10, ASP.NET Core |
| Frontend | React, TypeScript, Vite |
| Testing | xUnit, Vitest, React Testing Library |
| Deployment | Azure App Service, GitHub Actions |