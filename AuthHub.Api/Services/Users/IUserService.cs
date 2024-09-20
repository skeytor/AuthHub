using AuthHub.Api.Dtos;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.Users;

/// <summary>
/// Defines a contract that defines user operations.
/// </summary>
public interface IUserService
{
    Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync();
    Task<Result<Guid>> RegisterAsync(CreateUserRequest request);
    Task<Result<UserResponse>> GetByIdAsync(Guid id);
    Task<Result<Guid>> Update(Guid id, CreateUserRequest request);
}

