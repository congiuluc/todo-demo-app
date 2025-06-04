# Todo Demo App

A demonstration repository showcasing a Todo application built with .NET Minimal API.

## Project Overview

This repository contains a simple yet complete Todo application featuring:

- RESTful API built with ASP.NET Core Minimal API
- CRUD operations for managing todo items
- Swagger UI for API documentation and testing
- In-memory data store for simplicity

## Repository Structure

```
todo-demo-app/
├── src/
│   └── TodoApi/           # Main API project
│       ├── Models/        # Data models
│       ├── Program.cs     # API endpoints and configuration
│       ├── README.md      # Detailed API documentation
│       └── TodoApi.csproj # Project file
├── .vscode/              # VS Code configuration
└── README.md             # This file
```

## Quick Start

### Prerequisites

- .NET 9.0 SDK or later
- Any IDE or text editor (VS Code recommended)

### Running the Application

1. Clone this repository:
   ```bash
   git clone https://github.com/congiuluc/todo-demo-app.git
   cd todo-demo-app
   ```

2. Navigate to the API project and run it:
   ```bash
   cd src/TodoApi
   dotnet run
   ```

3. Access the application:
   - **Swagger UI**: https://localhost:5001/swagger
   - **API Base URL**: https://localhost:5001

## API Documentation

For detailed API documentation, endpoints, and usage examples, see the [TodoApi README](src/TodoApi/README.md).

## Features

- **GET /todos** - Retrieve all todo items
- **GET /todos/{id}** - Get a specific todo item
- **POST /todos** - Create a new todo item
- **PUT /todos/{id}** - Update an existing todo item
- **DELETE /todos/{id}** - Delete a todo item

## Development

This project uses:
- **ASP.NET Core Minimal API** for lightweight API development
- **OpenAPI/Swagger** for API documentation
- **In-memory storage** for simplicity (data resets on restart)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This is a demonstration project for educational purposes.