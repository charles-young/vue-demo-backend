# Vue Demo Backend

This project is a wrapper around the dummyjson.com recipes api. It provides additional configuration env based CORS settings.

In the future it could be extended with other functionality such as rate limiting, caching, centralized error handling, logging integration, health checks, or more.

## Explore the API

Visit https://localhost:5001/swagger to explore the API.

## Endpoints

 - GET /api/recipes - List all recipes (with optional ?limit=10&skip=0)
 - GET /api/recipes/{id} - Get a specific recipe
 - GET /api/recipes/search?q={query} - Search recipes
 - GET /api/recipes/tag/{tag} - Filter by tag
 - GET /api/recipes/meal-type/{mealType} - Filter by meal type

## Cors Configuration

Cors can be configured from appsettings.json.

```json
{
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000", "https://yourdomain.com"],
    "AllowedMethods": ["GET", "POST", "PUT", "DELETE"],
    "AllowedHeaders": ["Content-Type", "Authorization"],
    "AllowCredentials": true
  }
}
```

## Requirements

.NET 8.0 SDK or later
