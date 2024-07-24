using Api.UnitTest.Setup;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class CreateUserValidTestData : TheoryData<CreateUserRequest>
{
    public CreateUserValidTestData()
    {
        Add(UserTestDataFactory.CreateSingleUserRequest());
        Add(UserTestDataFactory.CreateSingleUserRequest());
        Add(UserTestDataFactory.CreateSingleUserRequest());
        Add(UserTestDataFactory.CreateSingleUserRequest());
        Add(UserTestDataFactory.CreateSingleUserRequest());
    }

}
