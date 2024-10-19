using AuthHub.Api.Dtos;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.Users;

/// <summary>
/// Defines a contract for user-related.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Returns all users.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronouns operation, containing a <see cref="Result{TValue}"/>
    /// with a <see cref="IReadOnlyList{T}"/> of <see cref="UserResponse"/> if it is successful.
    /// </returns>
    Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync();
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="request">The data required to create a new user</param>
    /// <returns>
    /// A task that represents the asynchronouns operation
    /// </returns>
    Task<Result<Guid>> RegisterAsync(CreateUserRequest request);
    /// <summary>
    /// Retrieves the detail of a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// A task that represents the asynchronouns operation, containing a <see cref="Result{TValue}"/>
    /// with the <see cref="UserResponse"/> if the user is found.
    /// </returns>
    Task<Result<UserResponse>> GetByIdAsync(Guid id);
    /// <summary>
    /// Updates an existing user in the system.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="request">The updated user information</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a <see cref="Result{T}"/> 
    /// with the updated user's unique identifier <see cref="Guid"/> if the update is successful.
    /// </returns>
    Task<Result<Guid>> Update(Guid id, CreateUserRequest request);
}

