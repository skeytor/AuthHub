using Api.UnitTest.Setup;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Users.TestsData;

public class UserResponseListValidTestData : TheoryData<List<UserResponse>>
{
    public UserResponseListValidTestData()
    {
        Add(UserTestDataFactory.GenerateFakeUsersResponse(1));
        Add(UserTestDataFactory.GenerateFakeUsersResponse(4));
        Add(UserTestDataFactory.GenerateFakeUsersResponse(10));
    }
}