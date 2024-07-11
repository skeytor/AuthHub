using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.UserService;

public sealed class UserService(IUserRepository userRepository) : IUserService
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
            Password = request.Password,
            RoleId = request.RoleId
        };
        User userCreated = await userRepository.CreateAsync(user);
        return userCreated.Id;
    }

    public async Task<Result<IReadOnlyCollection<UserResponse>>> GetAllUsers()
    {
        var users = await userRepository.GetAllAsync();
        IReadOnlyCollection<UserResponse> result = users
            .Select(user => new UserResponse(user.Id, user.FirstName, user.LastName, user.Email))
            .ToList()
            .AsReadOnly();
        return Result.Success(result);
    }
}
