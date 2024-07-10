using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;

namespace AuthHub.Api.Services.UserService;

public sealed class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<Guid> Create(UserRequest request)
    {
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

    public async Task<IReadOnlyCollection<UserResponse>> GetAllUsers()
    {
        var users = await userRepository.GetAllAsync();
        IReadOnlyCollection<UserResponse> result = users
            .Select(user => new UserResponse(user.Id, user.FirstName, user.LastName, user.Email))
            .ToList()
            .AsReadOnly();
        return result;
    }
}
