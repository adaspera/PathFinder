# PathFinder - Public Transport Information System

PathFinder is a public transport information app that allows users to view stops from any provider in any city, with a work-in-progress trip route and time planner on an interactive map. The system leverages GTFS data from [MobilityDatabase](https://mobilitydatabase.org)  to provide accurate transit information.

## Features

- **Multi-City Support**: View transit stops from any provider in any supported city
- **Interactive Map**: Visualize transit stops and routes geographically
- **Real-time Data**: Fetches up-to-date GTFS data from MobilityDatabase
- **Trip Planning**: (Work in Progress) Plan routes with estimated travel times
- **City Search**: Fast search functionality powered by [Lucene](https://lucenenet.apache.org)

## Technology Stack

### Backend
- **.NET 9** with **Aspire** for orchestration
- **PostgreSQL** with PgWeb for database management
- **Docker** for containerization
- **Serilog** for comprehensive logging

### Frontend
- **React** with Vite for the client application
- **Leaflet** for interactive maps

## System Architecture

The project is organized into several components:

1. **Pathfinder.Server**: Contains the API endpoints and business logic
2. **Pathfinder.MigrationService**: Handles database migrations
3. **Pathfinder.Data**: Contains data models and DTOs
4. **Pathfinder.AppHost**: Orchestrates the application
5. **PathFinder.client**: React frontend application

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker Desktop
- Node.js (for frontend development)

### Installation

1. Clone the repository and setup:
    ```bash
    git clone https://github.com/yourusername/PathFinder.git
    cd PathFinder/PathFinder.client
    npm install
    
2. Configure environment variables:
   Add your MobilityDatabase refresh token in configurations
    ```bash
      dotnet user-secrets init
      dotnet user-secrets set "MobilityDb:RefreshToken" "your-refresh-token-here"
3. Start the application:
    ```bash
    cd ../PathFinder.AppHost
    dotnet run
