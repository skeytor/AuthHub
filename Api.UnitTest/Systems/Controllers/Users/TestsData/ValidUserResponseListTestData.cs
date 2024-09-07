using Api.UnitTest.Helpers;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Users.TestsData;

internal class ValidUserResponseListTestData : TheoryData<List<UserResponse>>
{
    public ValidUserResponseListTestData()
    {
        Add(UserResponseTestData.Generate(4));
        Add(UserResponseTestData.Generate(1));
        Add(UserResponseTestData.Generate(5));
    }
}