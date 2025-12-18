# IAmSMK

ASP.NET Core Web API for vehicle allocation management.

## Features

- **Allocation Endpoint**: Process vehicle allocation requests with jobs, vehicles, and depots
- **Name Endpoint**: Simple test endpoint
- **Swagger Documentation**: Interactive API documentation available in development mode
- **Tailscale Support**: Configured for Tailscale network access on port 5207

## Endpoints

### Allocation API
- `GET /api/allocation` - Health check endpoint
- `POST /api/allocation` - Submit allocation request with jobs, vehicles, and depots

### Name API
- `GET /name` - Connection test
- `POST /name` - Name query endpoint

## Requirements

- .NET 10.0 SDK
- Tailscale (for network access)

## Running the API

```bash
dotnet run
```

The API will start on port 5207 and display available access URLs including Tailscale domain information.

## Development

Swagger UI is available at `/swagger` when running in development mode.
