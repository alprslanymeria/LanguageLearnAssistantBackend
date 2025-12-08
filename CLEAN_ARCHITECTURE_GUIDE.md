# Clean Architecture Implementation Guide

## Overview

This document describes the implementation of Clean Architecture principles in the LanguageLearnAssistantBackend project. The action methods from the LanguageLearnAssistant frontend repository have been refactored to follow Clean Architecture patterns with proper separation of concerns.

## Architecture Layers

### 1. Domain Layer (`App.Domain`)
- **Purpose**: Contains enterprise business rules and entities
- **Location**: `src/core/App.Domain`
- **Contents**:
  - Entities (Language, Practice, Flashcard, Reading, Writing, Listening)
  - Domain exceptions (to be added as needed)
  - Domain events (to be added as needed)

### 2. Application Layer (`App.Application`)
- **Purpose**: Contains application business rules, DTOs, service interfaces, and validators
- **Location**: `src/core/App.Application`
- **Structure**:
```
App.Application/
├── Contracts/
│   ├── Infrastructure/
│   │   └── IObjectMapper.cs
│   └── Persistence/
│       ├── IGenericRepository.cs
│       ├── ILanguageRepository.cs
│       └── IUnitOfWork.cs
├── Extensions/
│   └── ServiceExtension.cs
├── Features/
│   └── Languages/
│       ├── DTOs/
│       │   ├── LanguageDto.cs
│       │   ├── CompareLanguageIdRequest.cs
│       │   └── CompareLanguageIdResponse.cs
│       ├── Services/
│       │   ├── ILanguageService.cs
│       │   └── LanguageService.cs
│       ├── Validators/
│       │   └── CompareLanguageIdRequestValidator.cs
│       └── Mappings/
│           └── LanguageMappingConfig.cs
└── ServiceResult.cs
```

### 3. Infrastructure Layer (`App.Infrastructure`)
- **Purpose**: Contains external concerns (mapping, security, etc.)
- **Location**: `src/infastructure/App.Infrastructure`
- **Contents**:
  - Mapster configuration
  - Security services
  - External service implementations

### 4. Persistence Layer (`App.Persistence`)
- **Purpose**: Contains data access logic
- **Location**: `src/infastructure/App.Persistence`
- **Contents**:
  - Entity Framework DbContext
  - Repository implementations
  - Entity configurations
  - Migrations

### 5. API Layer (`App.API`)
- **Purpose**: Contains REST API controllers and middleware
- **Location**: `src/API/App.API`
- **Contents**:
  - Controllers
  - Exception handlers
  - Filters
  - Program.cs (DI configuration)

## Clean Architecture Principles Applied

### 1. Dependency Inversion
- High-level modules (Application) do not depend on low-level modules (Persistence, Infrastructure)
- Both depend on abstractions (interfaces in Application/Contracts)
- Example: `ILanguageService` (interface in Application) is implemented by `LanguageService` (in Application) which depends on `ILanguageRepository` (interface), implemented by `LanguageRepository` (in Persistence)

### 2. Inversion of Control
- Dependencies are injected through constructors
- No use of `new` keyword for dependencies
- Configured in `Program.cs` using extension methods

### 3. Single Responsibility Principle
- Each service has a single responsibility
- `LanguageService` only handles language-related business logic
- Validators are separate from services
- DTOs are separate from entities

### 4. Persistence Ignorance
- Application layer knows nothing about Entity Framework or database details
- Uses repository interfaces instead
- Services work with DTOs, not entities directly

### 5. Separation of Concerns
- Features are organized by domain concept (Languages, Flashcards, etc.)
- Each feature has its own DTOs, services, validators, and mappings
- Clear boundaries between layers

## Implemented Features

### Language Feature

#### Endpoints

1. **GET /api/languages**
   - Returns all available languages
   - Response: `List<LanguageDto>`
   - Status Codes: 200 (OK), 404 (Not Found), 500 (Internal Server Error)

2. **POST /api/languages/compare**
   - Compares a language ID with user's native language
   - Request Body: `CompareLanguageIdRequest`
   - Response: `CompareLanguageIdResponse`
   - Status Codes: 200 (OK), 400 (Bad Request), 404 (Not Found), 500 (Internal Server Error)
   - Note: Currently returns placeholder data until User entity is implemented

#### DTOs

**LanguageDto**
```csharp
public record LanguageDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string? ImageUrl { get; init; }
}
```

**CompareLanguageIdRequest**
```csharp
public record CompareLanguageIdRequest
{
    public string UserId { get; init; }
    public int LanguageId { get; init; }
}
```

**CompareLanguageIdResponse**
```csharp
public record CompareLanguageIdResponse
{
    public bool IsMatch { get; init; }
}
```

#### Validation

- `CompareLanguageIdRequestValidator`: Validates that UserId is not empty and LanguageId is greater than 0
- Automatic validation using FluentValidation with auto-validation middleware

#### Service Implementation

- `LanguageService`: Implements `ILanguageService`
- Uses `ILanguageRepository` for data access
- Returns `ServiceResult<T>` for consistent response handling
- Includes logging for debugging and monitoring

#### Repository Implementation

- `LanguageRepository`: Implements `ILanguageRepository`
- Inherits from `GenericRepository<Language, int>`
- Provides language-specific data access if needed

## Libraries and Packages Used

### Application Layer
- **FluentValidation** (12.1.1): For DTO validation
- **FluentValidation.DependencyInjectionExtensions** (12.1.1): For DI integration
- **Mapster** (7.4.0): For object-to-object mapping
- **Mapster.DependencyInjection** (1.0.1): For DI integration
- **Microsoft.EntityFrameworkCore** (10.0.0): For IQueryable extensions
- **Microsoft.Extensions.Logging.Abstractions** (10.0.0): For logging

### Infrastructure Layer
- **Mapster**: Object mapping configuration
- **MapsterMapper**: Mapper service implementation

### Persistence Layer
- **Entity Framework Core** (10.0.0): ORM
- **Microsoft.EntityFrameworkCore.SqlServer** (10.0.0): SQL Server provider

### API Layer
- **Microsoft.AspNetCore.OpenApi**: For OpenAPI/Swagger
- **SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions**: Automatic validation

## Dependency Injection Configuration

### ServiceExtension (Application Layer)
```csharp
public static IServiceCollection AddServices(this IServiceCollection services)
{
    // FluentValidation
    services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);

    // Feature Services
    services.AddScoped<ILanguageService, LanguageService>();

    return services;
}
```

### RepositoryExtension (Persistence Layer)
```csharp
public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
{
    // DbContext
    services.AddDbContext<AppDbContext>(options =>
    {
        var connString = configuration.GetConnectionString("SqlServer");
        options.UseSqlServer(connString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(PersistenceAssembly).Assembly.FullName);
        });
    });

    // Unit of Work and Generic Repository
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

    // Specific Repositories
    services.AddScoped<ILanguageRepository, LanguageRepository>();

    return services;
}
```

### Program.cs Configuration
```csharp
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services
    .AddCustomTokenAuth(builder.Configuration)
    .AddServices()
    .AddRepositories(builder.Configuration)
    .AddMapster(null, typeof(App.Application.ApplicationAssembly).Assembly);
```

## Mapping with Mapster

Mapster automatically maps entities to DTOs based on property names. Custom mappings can be configured using `IRegister` interface:

```csharp
public class LanguageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Language, LanguageDto>();
    }
}
```

## Error Handling

### ServiceResult Pattern

All services return `ServiceResult<T>` which provides:
- **Success Response**: Contains data and HTTP status code
- **Failure Response**: Contains error messages and HTTP status code
- **Status Checking**: `IsSuccess` and `IsFail` properties

Example:
```csharp
if (!languages.Any())
{
    return ServiceResult<List<LanguageDto>>.Fail("No languages found.", HttpStatusCode.NotFound);
}
return ServiceResult<List<LanguageDto>>.Success(languageDtos);
```

### Controller Error Handling

Controllers check the service result and return appropriate HTTP responses:
```csharp
var result = await _languageService.GetLanguagesAsync();

if (result.IsFail)
{
    return StatusCode((int)result.Status, new { errors = result.ErrorMessage });
}

return Ok(result.Data);
```

## Future Improvements

### Planned Features (from notes.txt)

1. **CRUD Operations**
   - Flashcard management (categories, words)
   - Reading management (books, sessions)
   - Writing management (books, sessions)
   - Listening management (categories, videos, sessions)

2. **Practice Management**
   - Session tracking
   - Progress monitoring

3. **Profile Management**
   - User profile operations
   - Language preferences

4. **MediatR + CQRS**
   - Implement Command Query Responsibility Segregation
   - Use MediatR for command/query handling
   - Planned for a separate branch after current features are complete

### TODO Items

1. **User Entity Implementation**
   - Currently missing from domain
   - Required for `CompareLanguageId` functionality
   - Identity integration needed

2. **Exception Handling**
   - Custom domain exceptions
   - Global exception handler improvements

3. **Caching**
   - Redis integration for language data (as seen in frontend actions)
   - Cache invalidation strategies

4. **Testing**
   - Unit tests for services
   - Integration tests for repositories
   - API tests for controllers

## Migration from Frontend Actions

The following frontend action methods from `LanguageLearnAssistant/src/actions/language.ts` have been migrated:

1. **GetLanguages()** → `GET /api/languages`
   - Original: Prisma query with Redis caching
   - New: Repository pattern with EF Core
   - Cache layer to be added later

2. **CompareLanguageId()** → `POST /api/languages/compare`
   - Original: Prisma query with Redis caching
   - New: Service method (placeholder until User entity exists)
   - Will be fully implemented once Identity is integrated

## Best Practices Followed

1. **Immutable DTOs**: Using C# records for DTOs
2. **Async/Await**: All I/O operations are asynchronous
3. **Dependency Injection**: Constructor injection for all dependencies
4. **Interface Segregation**: Small, focused interfaces
5. **Single Responsibility**: Each class has one reason to change
6. **Logging**: Structured logging throughout the application
7. **Validation**: FluentValidation for request validation
8. **API Documentation**: XML comments for Swagger/OpenAPI

## Running the Application

1. **Prerequisites**:
   - .NET 10.0 SDK
   - SQL Server
   - Connection string configured in `appsettings.json`

2. **Build**:
   ```bash
   dotnet build
   ```

3. **Run**:
   ```bash
   cd src/API/App.API
   dotnet run
   ```

4. **Access API**:
   - Swagger UI: `https://localhost:<port>/swagger` (if configured)
   - API: `https://localhost:<port>/api/languages`

## Conclusion

This implementation establishes a solid foundation for Clean Architecture in the backend. The Language feature serves as a template for implementing other features. The architecture is:

- **Maintainable**: Clear separation of concerns
- **Testable**: Dependencies are injected and can be mocked
- **Scalable**: Easy to add new features following the established pattern
- **Flexible**: Easy to swap implementations (e.g., change database provider)

All future features should follow the same structure demonstrated by the Language feature.
