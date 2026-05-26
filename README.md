# Gem Powerplant

A modular C# solution for managing powerplant operations and production planning with a clean architecture approach.

## Project Structure

This solution follows a layered architecture pattern:

- **Gem.Powerplant.Api** - REST API layer with controllers and HTTP endpoints
- **Gem.Powerplant.Application** - Business logic layer with services and DTOs
- **Gem.Powerplant.Domain** - Core domain models and enums
- **Gem.Powerplant.Tests** - Unit tests for the solution

## Getting Started

### Prerequisites

- .NET 6.0 or higher
- Visual Studio Code or Visual Studio
- Git
- Docker (optional, for containerized deployment)

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd gem-spaas-powerplant
   ```

2. Navigate to the project root

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

### Running the Application

#### Local Development

Start the API:

```bash
dotnet run --project Gem.Powerplant.Api
```

The API will be available at `https://localhost:5001` (or configured port in `appsettings.json`).

#### Docker

Build the Docker image:

```bash
docker build -t gem-powerplant:latest .
```

Run the container:

```bash
docker run -p 5001:5001 gem-powerplant:latest
```

### Running Tests

Execute unit tests:

```bash
dotnet test Gem.Powerplant.Tests
```

Run specific test class:

```bash
dotnet test Gem.Powerplant.Tests --filter "DispatchServiceTests"
```

## API Endpoints

### Production Plan

**POST** `/ProductionPlan`

Create a production plan based on powerplant specifications and load requirements.

**Request Body:**

```json
{
  "load": 480,
  "fuels": {
    "gas(euro/MWh)": 13.4,
    "kerosine(euro/MWh)": 50.8,
    "co2(euro/ton)": 20,
    "wind(%)": 60
  },
  "powerplants": [
    {
      "name": "gasfiredbig1",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    },
    {
      "name": "windpark1",
      "type": "windturbine",
      "efficiency": 1,
      "pmin": 0,
      "pmax": 150
    }
  ]
}
```

**Response:**

```json
{
  "powerplants": [
    {
      "name": "windpark1",
      "p": 150
    },
    {
      "name": "gasfiredbig1",
      "p": 330
    }
  ]
}
```

## Configuration

Application settings are managed in:

-

appsettings.json

- Production settings
-

appsettings.Development.json

- Development settings

Example `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## Architecture

### Domain Layer (`Gem.Powerplant.Domain`)

Contains core entities and business rules:

- PowerPlant models
- Enums for PowerPlant types (gasfired, turbojet, windturbine)
- Fuel specifications
- Business validation logic

### Application Layer (`Gem.Powerplant.Application`)

Implements business logic:

- **IDispatchService** - Orchestrates production planning and power dispatch
- **IProductionService** - Handles individual powerplant production calculations
- DTOs for API contracts
- Service implementations
- Business logic for optimal powerplant selection

### API Layer (`Gem.Powerplant.Api`)

Exposes HTTP endpoints:

- ProductionPlanController - Handles production plan requests
- Request/response handling
- Logging and error handling
- Swagger/OpenAPI documentation

### Tests (`Gem.Powerplant.Tests`)

Unit tests for all layers:

- DispatchServiceTests - Tests for production plan computation
- Service layer validation
- Edge case handling

## Development Workflow

1. Create a feature branch:

   ```bash
   git checkout -b feature/your-feature
   ```

2. Make your changes

3. Run tests:

   ```bash
   dotnet test
   ```

4. Commit:

   ```bash
   git commit -am 'Add feature'
   ```

5. Push:

   ```bash
   git push origin feature/your-feature
   ```

6. Create a Pull Request

## Powerplant Types

- **gasfired** - Gas-fired power plants with efficiency and cost considerations
- **turbojet** - Turbojet engines with lower efficiency
- **windturbine** - Wind turbines with variable power based on wind percentage

## Algorithm Overview

The dispatch algorithm:

1. Calculates the cost per MWh for each powerplant
2. Sorts powerplants by cost (merit order)
3. Allocates load to powerplants starting with cheapest
4. Respects minimum (pmin) and maximum (pmax) load constraints
5. Considers wind availability for wind turbines
6. Optimizes for minimum total cost

## Troubleshooting

### Port Already in Use

If port 5001 is already in use, configure it in `appsettings.json`:

```json
{
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://localhost:5002"
      }
    }
  }
}
```

### Certificate Errors

For development, you may need to trust the HTTPS certificate:

```bash
dotnet dev-certs https --trust
```

## Performance Considerations

- The dispatch algorithm runs in O(n log n) time due to sorting
- Suitable for powerplant counts up to several thousand
- Consider caching for repeated requests with same parameters

## License

[Add your license information here]

## Support

For issues or questions, please contact the development team or open an issue in the repository.

```

```
````
