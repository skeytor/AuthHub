using Api.UnitTest.Setup.Factories;
using Api.UnitTest.Setup.Factories.Implementations;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Users.TestsData;

public class UserControllerCreateUserValidTestData : TheoryData<CreateUserRequest, Guid>
{
    public UserControllerCreateUserValidTestData()
    {
        var testData = new TestDataFactory<CreateUserRequestData, CreateUserRequest>().Create();
        Add(testData.Single(), Guid.NewGuid());
        Add(testData.Single(), Guid.NewGuid());
        Add(testData.Single(), Guid.NewGuid());
    }
}
