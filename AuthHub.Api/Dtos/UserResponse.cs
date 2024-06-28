namespace AuthHub.Api.Dtos;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string LastName,
    string Email
    );
