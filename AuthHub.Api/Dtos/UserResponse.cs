namespace AuthHub.Api.Dtos;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
    );
