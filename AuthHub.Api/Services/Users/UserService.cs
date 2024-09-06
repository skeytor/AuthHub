using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.Services.Users;

public sealed class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher) : IUserService
{
    public async Task<Result<Guid>> CreateAsync(CreateUserRequest request)
    {
        if (await userRepository.EmailExistsAsync(request.Email))
        {
            return Result.Failure<Guid>(
                Error.Conflict("User.AlreadyEmail", $"Email {request.Email} is already"));
        }
        if (await userRepository.UserNameExistsAsync(request.UserName))
        {
            return Result.Failure<Guid>(
                Error.Conflict("User.Username", $"Username: {request.UserName} is already"));
        }
        User user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.UserName,
            Email = request.Email,
            RoleId = request.RoleId
        };
        user.Password = passwordHasher.HashPassword(user, request.Password);
        var userCreated = await userRepository.InsertAsync(user);
        await unitOfWork.SaveChangesAsync();
        return userCreated.Id;
    }

    public async Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync()
    {
        var users = await userRepository.GetAllAsync();
        IReadOnlyList<UserResponse> result = users
            .Select(user => new UserResponse(user.Id, user.FirstName, user.LastName, user.Email))
            .ToList()
            .AsReadOnly();
        return Result.Success(result);
    }

    public async Task<Result<UserResponse>> GetByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return Result.Failure<UserResponse>(
                Error.NotFound("User.Id", $"User with ID: {id} was not found"));
        }
        return new UserResponse(user.Id, user.FirstName, user.LastName, user.Email);
    }

    public async Task<Result<Guid>> Update(Guid id, CreateUserRequest request)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return Result.Failure<Guid>(
                Error.NotFound("User.Id", $"User with ID: {id} was not found"));
        }
        if (await userRepository.EmailExistsAsync(user.Email)) 
        {
            return Result.Failure<Guid>(
                Error.Conflict("User.Email", $"Email: {request.Email} is already"));
        }
        if (await userRepository.UserNameExistsAsync(user.Username))
        {
            return Result.Failure<Guid>(
                        Error.NotFound("User.Username", $"Userbane: {request.UserName} is already"));
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
