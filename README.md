# IAmSMK

ASP.NET Core Web API featuring two main controllers: **NameController** for interactive name queries and **AllocationController** for vehicle allocation management.

## Controllers

### NameController

A playful name identification API that responds to specific queries about its identity.

#### GET Endpoint
- **Route**: `GET /name`
- **Response**: Returns a simple connection confirmation message
- **Example Response**:
  ```json
  "API is working! Connected successfully."
  ```

#### POST Endpoint
- **Route**: `POST /name`
- **Request Body**: 
  ```json
  {
    "input": "What is my name"
  }
  ```
- **Logic**:
  - If the `input` field exactly matches `"What is my name"` (case-sensitive), returns: `"I am SMK!"`
  - For any other input or missing input, returns a `400 Bad Request` with the message: `"Hahaha!!!"`

**Example Success Request**:
```bash
POST /name
Content-Type: application/json

{
  "input": "What is my name"
}
```

**Response** (200 OK):
```json
"I am SMK!"
```

**Example Failure Request**:
```bash
POST /name
Content-Type: application/json

{
  "input": "who are you"
}
```

**Response** (400 Bad Request):
```json
"Hahaha!!!"
```

### AllocationController

Vehicle allocation management API that processes allocation requests with jobs, vehicles, and depots.

#### GET Endpoint
- **Route**: `GET /api/allocation`
- **Response**: Returns API status and endpoint information
- **Example Response**:
  ```json
  {
    "message": "Allocation API is running and accessible",
    "endpoint": "/api/allocation",
    "method": "POST",
    "timestamp": "2024-01-01T00:00:00Z"
  }
  ```

#### POST Endpoint
- **Route**: `POST /api/allocation`
- **Request Body**: 
  ```json
  {
    "jobs": [
      {
        "jobId": 1,
        "customerId": 100,
        "startLat": 40.7128,
        "startLng": -74.0060
      }
    ],
    "vehicles": [
      {
        "vehicleId": 1,
        "capacity": 100
      }
    ],
    "depots": [
      {
        "depotId": 1,
        "lat": 40.7580,
        "lng": -73.9855
      }
    ]
  }
  ```
- **Response**: Returns the received data with counts
- **Example Response**:
  ```json
  {
    "receivedJobs": 1,
    "receivedVehicles": 1,
    "receivedDepots": 1,
    "jobs": [...],
    "vehicles": [...],
    "depots": [...]
  }
  ```

## Requirements

- .NET 10.0 SDK
- Tailscale (for network access)

## Running the API

```bash
dotnet run
```

The API will start on port 5207 and display available access URLs including Tailscale domain information.

## Development

Swagger UI is available at `/swagger` when running in development mode for testing both controllers.