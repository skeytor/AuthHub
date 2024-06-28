using AuthHub.Api.Dtos;

namespace AuthHub.Api.Services.UserService;

public interface IUserService
{
    Task<IReadOnlyCollection<UserResponse>> GetAllUsers();
}

