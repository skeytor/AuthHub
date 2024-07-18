using Api.UnitTest.Setup;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.User;

public class SetupUserControllerValidTestData : TheoryData<List<UserResponse>>
{
    public SetupUserControllerValidTestData()
    {
        Add(TestData.GenerateFakeUsersResponse(1));
        Add(TestData.GenerateFakeUsersResponse(4));
        Add(TestData.GenerateFakeUsersResponse(10));
    }
}