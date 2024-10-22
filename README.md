
# AuthHub

This project provides a secure and dynamic solution for authentication and authorization, focusing on role-based access control with permission-based policies. The key feature is its flexibility, allowing the addition of new roles without modifying the source code. This is achieved by implementing a CustomAuthorization attribute to manage policy-based authorization dynamically.


## Key Modules

It include:

* User Management: Allows CRUD operations on users.
* Role and Permission Management: Defines roles and assigns permissions.
* Token Management: Implements JWT for user authentication and refresh tokens.
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
### User Secrets Setup
In this project, User Secrets is used to store sensitive information like the JWT Secret Key and Database Connection Strings. To set up User Secrets:

1.- Navigate to the project directory:
```bash
  cd src/YourProjectName
```
2.- Initialize User Secrets:
```bash
  dotnet user-secrets init
```
3.- Add the necessary secrets (such as connection strings, JWT keys, etc.):
```bash
dotnet user-secrets set "ConnectionStrings:Database" "Server=authhub.database;Database=AuthHub;User ID=sa;Password=P@ssword123;TrustServerCertificate=True"
dotnet user-secrets set "OptionsToken:SecretKey" "YourJWTSecretKey((a4:B4Z*qp@Lr]-hvwqZ`Y3q(;7o@"
dotnet user-secrets set "OptionsToken:Issuer" "http://localhost"
dotnet user-secrets set "OptionsToken:Audience" "http://localhost"

```
### Running the project with Docker Compose
1.- Clone the repository: 
```bash
  https://github.com/skeytor/AuthHub.git
```

2.- Go to the project directory

```bash
  cd your-repo
```

3.- Build and run the Docker containers:

```bash
  docker-compose up --build
  cd tests/YourTestProjectName
```
4.- Access to the API by http://localhost:8080/swagger/index.html

This will start up the following containers:

* API: The ASP.NET Core Web API running on port 8080
* Database: SQL Server container (Is not nessesary install SQL Server in your computer, because it will be in a container)

### Running the tests
This project includes both unit tests and integration tests to ensure the functionality of the system.

Integration tests rely on TestContainers to spin up a real SQL Server instance and test the database interactions.

Follow the steps below to run the tests:

1.- Ensure Docker is running on your system, as the integration tests will use Docker Compose to set up the test environment.

2.- Navigate to the test project directory:
```bash
  cd tests/YourTestProjectName
```
3.- Run the unit tests:
```bash
  dotnet test
```