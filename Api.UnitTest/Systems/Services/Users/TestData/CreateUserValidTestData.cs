using Api.UnitTest.Setup.Factories;
using Api.UnitTest.Setup.Factories.Implementations;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class CreateUserValidTestData : TheoryData<CreateUserRequest>
{
    public CreateUserValidTestData()
    {
        /*var testData = new TestDataFactory<CreateUserRequestData, CreateUserRequest>().Create();
        Add(testData.Single());
        Add(testData.Single());
        Add(testData.Single());
        Add(testData.Single());*/
        Add(UserTestDataFactory.CreateSingleUserRequest());
        Add(UserTestDataFactory.CreateSingleUserRequest());
        Add(UserTestDataFactory.CreateSingleUserRequest());
    }

}
