# Todo Minimal API

A simple Todo API built with ASP.NET Core Minimal API featuring:

- CRUD operations for Todo items
- Swagger UI enabled for both development and release environments
- In-memory data store

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later

### Running the API

1. Clone the repository
2. Navigate to the project directory:
   ```
   cd TodoApi
   ```
3. Run the API:
   ```
   dotnet run
   ```
4. Access the Swagger UI:
   - Development: https://localhost:5001/swagger
   - Production: https://[your-host]/swagger

## API Endpoints

- **GET /todos** - Get all todo items
- **GET /todos/{id}** - Get a specific todo item by ID
- **POST /todos** - Create a new todo item
- **PUT /todos/{id}** - Update an existing todo item
- **DELETE /todos/{id}** - Delete a todo item

## Sample Todo Item

```json
{
  "id": 0,
  "title": "Complete the project",
  "description": "Finish implementing the Todo API",
  "isComplete": false
}
```

## Notes

- This API uses an in-memory data store which resets when the application is restarted
- Swagger UI is enabled in both development and production environments for API documentation and testing
