using Api.UnitTest.Setup;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.User;

public class SetupUserControllerValidTestData : TheoryData<List<UserResponse>>
{
    public SetupUserControllerValidTestData()
    {
        Add(UserTestDataFactory.GenerateFakeUsersResponse(1));
        Add(UserTestDataFactory.GenerateFakeUsersResponse(4));
        Add(UserTestDataFactory.GenerateFakeUsersResponse(10));
    }
}