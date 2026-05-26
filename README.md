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
