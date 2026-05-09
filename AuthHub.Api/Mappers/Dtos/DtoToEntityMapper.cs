using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;

namespace AuthHub.Api.Mappers.Dtos;

internal static class DtoToEntityMapper
{
    internal static User MapToUserEntity(this CreateUserRequest request)
    {
        return new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Username = request.UserName,
            IsActive = true,
            RoleId = request.RoleId,
        };
    }
}
