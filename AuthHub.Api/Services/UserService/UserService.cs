using AuthHub.Api.Dtos;
using AuthHub.Domain.Repositories;

namespace AuthHub.Api.Services.UserService;

public sealed class UserService(IUserRepository userRepository) : IUserService
{
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
