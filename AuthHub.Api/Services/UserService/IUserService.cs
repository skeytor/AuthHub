using AuthHub.Api.Dtos;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.UserService;

public interface IUserService
{
    Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync();
    Task<Result<Guid>> CreateAsync(CreateUserRequest request);
    Task<Result<UserResponse>> GetByIdAsync(Guid id);
}

