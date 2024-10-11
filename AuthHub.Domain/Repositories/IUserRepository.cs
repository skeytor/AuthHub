using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories;

/// <summary>
/// Defines a contract for performing data access operations related to users.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Inserts a new user into the database.
    /// </summary>
    /// <param name="user">The user entity to insert.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the inserted user entity.
    /// </returns>
    Task<User> InsertAsync(User user);
    /// <summary>
    /// Returns a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the user entity if found;
    /// otherwise, <c>null</c>
    /// </returns>
    Task<User?> GetByIdAsync(Guid id);
    /// <summary>
    /// Returns all users from the data store.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operations. 
    /// The task result contains <see cref="IReadOnlyList{T}"/> of <see cref="User"/>
    /// </returns>
    Task<IReadOnlyList<User>> GetAllAsync();
    /// <summary>
    /// Returns a <see cref="User"/> by their username.
    /// </summary>
    /// <param name="userName">The username of the user</param>
    /// <returns>
    /// A task that represents the asynchronouns operation. The task result contains a <see cref="User"/> if it is found;
    /// otherwise, <c>null</c>
    /// </returns>
    Task<User?> GetByUserNameAsync(string userName);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UserNameExistsAsync(string userName);
    Task<HashSet<string>> GetPermissions(Guid userId);
    void Update(User user);
}
