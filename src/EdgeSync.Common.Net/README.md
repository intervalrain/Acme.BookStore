# EdgeSync.Common.Net

A .NET library that provides standardized exception handling following RFC 9457 Problem Details specification.

## Features

- **EdgeSync Exception Handling**: Custom exceptions automatically convert to appropriate HTTP responses with RFC 9457 Problem Details
- **201 Created Support**: Simple attribute-based approach for returning 201 Created status codes
- **Framework Support**: Works with both pure ASP.NET Core and ABP Framework applications

## Installation

Add reference to your project:

```xml
<ProjectReference Include="..\EdgeSync.Common.Net\EdgeSync.Common.Net.csproj" />
```

## Quick Start

### For ASP.NET Core Applications

```csharp
// In Program.cs or Startup.cs
builder.Services.AddEdgeSyncExceptionHandling();

// Or with custom options
builder.Services.ConfigureEdgeSyncExceptionHandling(options =>
{
    options.ProblemTypeBaseUrl = "https://yourdomain.com/errors/";
    options.IncludeExceptionDetails = builder.Environment.IsDevelopment();
});
```

### For ABP Framework Applications

```csharp
// In your Module class
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAbpEdgeSyncExceptionHandling();
}
```

## Usage

### Using EdgeSync Exceptions in Controllers/Application Services

```csharp
public class UserAppService : ApplicationService
{
    // Returns 201 Created using attribute
    [CreatedResponse]
    public async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        if (string.IsNullOrEmpty(input.Name))
        {
            throw new EdgeSyncBadRequestException("User name is required");
        }
        
        var user = new UserDto { Id = 123, Name = input.Name };
        // Status will automatically be set to 201 Created
        return user;
    }

    public UserDto GetUser(int id)
    {
        var user = _repository.FindById(id);
        if (user == null)
        {
            // This will automatically return 404 with Problem Details
            throw new EdgeSyncNotFoundException($"User with ID {id} not found");
        }
        return user;
    }

    public void DeleteUser(int id)
    {
        if (!HasPermission())
        {
            // This will automatically return 403 with Problem Details
            throw new EdgeSyncForbiddenException("You don't have permission to delete users");
        }
        // Delete logic...
    }
}
```

## Attributes

| Attribute | Description |
|-----------|-------------|
| `[CreatedResponse]` | Automatically sets HTTP status to 201 Created for successful resource creation |

## Exception Types

| Exception | HTTP Status | Use Case |
|-----------|------------|----------|
| `EdgeSyncBadRequestException` | 400 | Invalid request data |
| `EdgeSyncAuthorizationException` | 401 | User not authenticated |
| `EdgeSyncForbiddenException` | 403 | User lacks permissions |
| `EdgeSyncNotFoundException` | 404 | Resource doesn't exist |
| `EdgeSyncConflictException` | 409 | Duplicate or conflicting resource |
| `EdgeSyncUnprocessableEntityException` | 422 | Business rule validation failure |

## Problem Details Response Format

All error responses follow RFC 9457 Problem Details:

```json
{
  "type": "https://example.com/probs/not-found",
  "title": "Not Found",
  "status": 404,
  "detail": "User with ID 123 not found",
  "instance": "/api/users/123"
}
```

## Configuration Options

```csharp
services.ConfigureEdgeSyncExceptionHandling(options =>
{
    // Base URL for problem type URIs
    options.ProblemTypeBaseUrl = "https://api.example.com/errors/";
    
    // Include stack trace in development
    options.IncludeExceptionDetails = environment.IsDevelopment();
    
    // Handle all exceptions, not just EdgeSync ones
    options.HandleAllExceptions = true;
    
    // Add custom exception mappings
    options.CustomProblemTypeMappings[typeof(CustomException)] = "custom-error";
});
```

## Best Practices

1. **Use [CreatedResponse] for 201 Status**: When creating resources, use the `[CreatedResponse]` attribute to automatically set 201 Created status

2. **Use Exceptions for All Error Scenarios**: Throw EdgeSync exceptions from anywhere in your business logic to get appropriate HTTP responses with RFC 9457 Problem Details

3. **Consistent Error Messages**: Keep error messages user-friendly and consistent across your API

4. **Exception-First Approach**: Prefer throwing exceptions over manual status code handling for cleaner, more maintainable code

## License

MIT