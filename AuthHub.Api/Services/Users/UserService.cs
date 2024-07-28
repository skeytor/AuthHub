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
        if (await userRepository.ExistAsync(request.Email))
        {
            return Result.Failure<Guid>(
                Error.Conflict("User.AlreadyEmail", $"Email {request.Email} is already"));
        }
        User user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.UserName,
            Email = request.Email,
            RoleId = request.RoleId
        };
        string passwordHashed = passwordHasher
            .HashPassword(user, request.Password);
        user.Password = passwordHashed;
        var guid = await userRepository
            .CreateAsync(user)
            .ContinueWith(s => s.Result.Id);
        await unitOfWork.SaveChangesAsync();
        return guid;
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
}
