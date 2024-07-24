using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Setup;

internal class ITestDataFactory
{
    public User User { get; set; } = null!;
    public CreateUserRequest CreateUserRequest { get; set; } = null!;
}
