using Api.UnitTest.Setup;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Users.TestsData;

public class UserControllerCreateUserValidTestData : TheoryData<CreateUserRequest, Guid>
{
    public UserControllerCreateUserValidTestData()
    {
        Add(UserTestDataFactory.CreateSingleUserRequest(), Guid.NewGuid());
    }
}
