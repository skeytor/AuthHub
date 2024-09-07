using Api.UnitTest.Helpers;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Users.TestsData;

internal class UserControllerUserResponseValidTestData : TheoryData<Guid, UserResponse>
{
    public UserControllerUserResponseValidTestData()
    {
        Add(Guid.NewGuid(), UserResponseTestData.Generate());
        Add(Guid.NewGuid(), UserResponseTestData.Generate());
        Add(Guid.NewGuid(), UserResponseTestData.Generate());
    }
}
