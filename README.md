# Gem Powerplant

A modular C# solution for managing powerplant operations with a clean architecture approach.

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

### Installation

1. Clone the repository
2. Navigate to the project root
3. Restore dependencies:
   ```bash
   dotnet restore

### Running the Application
### Start the API:
```bash
dotnet run --project Gem.Powerplant.Api
The API will be available at
   ```bash
   https://localhost:5001 
(or configured port in appsettings.json).

Running Tests
Execute unit tests:
   ```bash
   dotnet test Gem.Powerplant.Tests
Configuration
Application settings are managed in:

appsettings.json - Production settings
appsettings.Development.json - Development settings
API Documentation
See Gem.Powerplant.Api.http for example API requests.

License
[Add your license information here]


