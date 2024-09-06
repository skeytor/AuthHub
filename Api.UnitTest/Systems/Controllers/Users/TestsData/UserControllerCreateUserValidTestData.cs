using Api.UnitTest.Helpers;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Users.TestsData;

internal class UserControllerCreateUserValidTestData : TheoryData<CreateUserRequest, Guid>
{
    public UserControllerCreateUserValidTestData()
    {
        Add(CreateUserTestData.Generate(), Guid.NewGuid());
        Add(CreateUserTestData.Generate(), Guid.NewGuid());
        Add(CreateUserTestData.Generate(), Guid.NewGuid());
    }
}
