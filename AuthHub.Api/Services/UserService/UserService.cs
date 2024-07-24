using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.Services.UserService;

public sealed class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher) : IUserService
{
    public async Task<Result<Guid>> Create(CreateUserRequest request)
    {
        bool checkUser = await userRepository.ExistAsync(request.Email);
        if (checkUser)
        {
            return Result
                .Failure<Guid>(Error.Conflict("User.AlreadyEmail", $"Email { request.Email } is already"));
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
            .HashPassword(user, request.Password); // hash password with Microsoft Identity IHashPassword provider
        user.Password = passwordHashed; 
        User userCreated = await userRepository.CreateAsync(user);
        await unitOfWork.SaveChangesAsync();
        return userCreated.Id;
    }

    public async Task<Result<IReadOnlyList<UserResponse>>> GetAllUsers()
    {
        var users = await userRepository.GetAllAsync();
        IReadOnlyList<UserResponse> result = users
            .Select(user => new UserResponse(user.Id, user.FirstName, user.LastName, user.Email))
            .ToList()
            .AsReadOnly();
        return Result.Success(result);
    }
}
