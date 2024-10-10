using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Errors;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.Services.Users;

/// <summary>
/// Implements <see cref="IUserService"/> interface to handle user-related operations.
/// </summary>
/// <param name="userRepository">The repository to handle user data access.</param>
/// <param name="unitOfWork">The unit of work for committing database changes.</param>
/// <param name="passwordHasher">The standard service for hashing user passwords.</param>
public sealed class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher) : IUserService
{
    public async Task<Result<Guid>> RegisterAsync(CreateUserRequest request)
    {
        if (await userRepository.EmailExistsAsync(request.Email))
        {
            return Result.Failure<Guid>(UserError.EmailAlready(request.Email));
        }
        if (await userRepository.UserNameExistsAsync(request.UserName))
        {
            return Result.Failure<Guid>(UserError.UserNameAlready(request.UserName));
        }
        User user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.UserName,
            Email = request.Email,
            IsActive = true,
            RoleId = request.RoleId
        };
        user.Password = passwordHasher.HashPassword(user, request.Password);
        var userCreated = await userRepository.InsertAsync(user);
        await unitOfWork.SaveChangesAsync();
        return userCreated.Id;
    }

    public async Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync()
    {
        IReadOnlyList<User> users = await userRepository.GetAllAsync();
        return users
            .Select(x => new UserResponse(x.Id, x.FirstName, x.LastName, x.Email))
            .ToList();
    }

    public async Task<Result<UserResponse>> GetByIdAsync(Guid id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return Result.Failure<UserResponse>(UserError.NotFound(id));
        }
        return new UserResponse(user.Id, user.FirstName, user.LastName, user.Email);
    }

    public async Task<Result<Guid>> Update(Guid id, CreateUserRequest request)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return Result.Failure<Guid>(UserError.NotFound(id));
        }
        if (await userRepository.EmailExistsAsync(request.Email)) 
        {
            return Result.Failure<Guid>(UserError.EmailAlready(request.Email));
        }
        if (await userRepository.UserNameExistsAsync(request.UserName))
        {
            return Result.Failure<Guid>(UserError.UserNameAlready(request.UserName));
        }
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Username = request.UserName;
        user.Email = request.Email;
        user.RoleId = request.RoleId;
        user.Password = passwordHasher.HashPassword(user, request.Password);
        await unitOfWork.SaveChangesAsync();
        return user.Id;
    }

}
