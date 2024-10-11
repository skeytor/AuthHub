using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories;

/// <summary>
/// Defines a contract for performing data access operations related to users.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Inserts a new user into the data store.
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
    /// <summary>
    /// Checks whether an email already exists in the data store.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c>
    /// if the email exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> EmailExistsAsync(string email);
    /// <summary>
    /// Checks whether a username already exists in the data store.
    /// </summary>
    /// <param name="userName">The username to check.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the username exists;
    /// otherwise, <c>false</c>.
    /// </returns>
    Task<bool> UserNameExistsAsync(string userName);
    /// <summary>
    /// Gets a set of permissions associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A task that represents the asynchronouns operation. The task result contains a <see cref="HashSet{string}"/>
    /// of permissions.
    /// </returns>
    Task<HashSet<string>> GetPermissions(Guid userId);
    /// <summary>
    /// Updates an existing user in the data store.
    /// </summary>
    /// <param name="user">The user entity with updated information.</param>
    void Update(User user);
}
