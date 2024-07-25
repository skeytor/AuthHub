using Api.UnitTest.Setup.Factories;
using Api.UnitTest.Setup.Factories.Implementations;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Users.TestsData;

public class UserResponseListValidTestData : TheoryData<List<UserResponse>>
{
    public UserResponseListValidTestData()
    {
        var testData = new TestDataFactory<UserResponseData, UserResponse>().Create();
        Add(testData.Multiple(1));
        Add(testData.Multiple(4));
        Add(testData.Multiple(5));
    }
}