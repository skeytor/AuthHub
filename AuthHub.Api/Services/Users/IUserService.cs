using AuthHub.Api.Dtos;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.Users;

/// <summary>
/// Defines a contract that defines the operations
/// </summary>
public interface IUserService
{
    Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync();
    Task<Result<Guid>> CreateAsync(CreateUserRequest request);
    Task<Result<UserResponse>> GetByIdAsync(Guid id);
    Task<Result<Guid>> Update(Guid id, CreateUserRequest request);
}

