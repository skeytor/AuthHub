
# AuthHub

This project provides a secure and dynamic solution for authentication and authorization, focusing on role-based access control with permission-based policies. The key feature is its flexibility, allowing the addition of new roles without modifying the source code. This is achieved by implementing a CustomAuthorization attribute to manage policy-based authorization dynamically.


## Key Modules

It include:

- **User Management:** Allows CRUD operations on users.
- **Role and Permission Management:** Defines roles and assigns permissions.
- **Token Management:** Implements JWT for user authentication and refresh tokens.
## Technologies Used

 - [ASP.NET Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
 - [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/install)
 - [Docker](https://www.docker.com/)
 - [Docker Compose](https://docs.docker.com/compose/)
 - [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
 - [Json Web Token (JWT)](https://jwt.io/)

## Setup
### Prerequisites

To run this project, you need the following installed on your system:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://docker.com)
- [Git](https://git-scm.com/downloads)

### Running the project with Docker Compose
1. Clone the repository: 
```bash
  git clone https://github.com/skeytor/AuthHub.git
```

2. Go to the project directory

```bash
  cd AuthHub
```

3. Initialize User Secrets:
```bash
  cd AuthHub/AuthHub.Api
  dotnet user-secrets init
```

4. Add the necessary secrets (such as connection strings, JWT keys, etc.):
```bash
dotnet user-secrets set "ConnectionStrings:Database" "Server=authhub.database;Database=AuthHub;User ID=sa;Password=P@ssword123;TrustServerCertificate=True"
dotnet user-secrets set "OptionsToken:SecretKey" "YourJWTSecretKey((a4:B4Z*qp@Lr]-hvwqZ`Y3q(;7o@"
dotnet user-secrets set "OptionsToken:Issuer" "http://localhost"
dotnet user-secrets set "OptionsToken:Audience" "http://localhost"
```

5. Get back to the main folder
```bash
  cd AuthHub
```

6. Build and run the Docker containers:

```bash
  docker-compose up --build
```
7. Access to the API via http://localhost:8080/swagger/index.html

This will start up the following containers:

- **API:** The ASP.NET Core Web API running on port 8080
- **Database:** SQL Server container (SQL Server is not required on your local machine as it runs inside a container)

## Tests
This project includes both unit tests and integration tests to ensure the functionality.

Tests libraries:

- [xUnit](https://xunit.net/)
- [Moq](https://documentation.help/Moq/)
- [Bogus](https://github.com/bchavez/Bogus)

### Running the Unit Tests
Follow the steps below to run the unit tests:

1. Navigate to the test project directory:
```bash
  cd AuthHub/Api.UnitTest
```

2. Run the unit tests:
```bash
  dotnet test
```

### Running the Integration Tests
Integration tests rely on [TestContainers](https://dotnet.testcontainers.org/) to spin up a real SQL Server instance and test the database interactions.

When running integration tests, the database is seeded with sample data. Refer to the `SampleData.cs` and `SampleDataInitializer.cs` classes for more information.

Follow the steps below to run the integration testing:

1. Ensure Docker is running on your system, as the integration tests will use Docker Compose to set up the test environment.

2. Navigate to the test project directory:
```bash
  cd AuthHub/AuthHub.Api.IntegrationTest
```
3. Run the unit tests:
```bash
  dotnet test
```

## Usage
As mentioned, we are using policy-based authorization to secure the API endpoints. The permissions are managed dynamically through an enum with the `[Flags]` attribute, which allows combining multiple permissions. This provides flexibility in assigning permissions to roles without needing to modify the source code when new roles are introduced.

This is the `Permission` enum.
```
[Flags]
public enum Permissions
{
    None = 0,
    CanViewRoles = 1,
    CanManageRoles = 2,
    CanViewUsers = 4,
    CanManageUsers = 8,
    Forecast = 16,
    /* Define your permissions */
    ...
    All = int.MaxValue,
}
```

### Steps to Add a new Permission
1. Define the new permission in the `Permissions` enum.
2. **Create and apply a database migration** to update the permissions in the database.
3. Use the new permission in your controllers by adding the `[CustomAuthorize]` attribute.

This process allows you to dynamically manage roles without modifying the source code when new roles are required.

### Adding Permissions to Controllers
After defining permissions in the `Permissions` enum and updating the database by migration, you can secure your controllers by `[CustomAuthorize]` attribute.

Example of usage:

```
[ApiController]
[Route("[controller]")]
[CustomAuthorize(Permissions.Forecast)]  // Restricts access to users with the 'Forecast' permission
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = 
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("Get Weather Forecast.");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
```

The `[CustomAuthorize]` attribute can be applied at the class or method level, allowing you to restrict access based on permissions. This ensures that only users with the required permission(s) can access the specified endpoint.

You can also combine multiple permissions by using the `|` operator like this:

```
[CustomAuthorize(Permissions.CanViewRoles | Permissions.CanManageUsers)]
```
This way, the endpoint can be accessed by users with either permission.

## Credits
This approach to policy-based authorization with dynamic permissions is based on the work of [Jason Taylor](https://youtu.be/BVJVhceN3N4?si=BjSaukADZyZLhfHh), whose ideas and implementation were essential to the development of this project.