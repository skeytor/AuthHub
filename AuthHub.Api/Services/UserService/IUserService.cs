using AuthHub.Api.Dtos;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.UserService;

public interface IUserService
{
    Task<Result<IReadOnlyCollection<UserResponse>>> GetAllUsers();
    Task<Result<Guid>> Create(CreateUserRequest request);
}

