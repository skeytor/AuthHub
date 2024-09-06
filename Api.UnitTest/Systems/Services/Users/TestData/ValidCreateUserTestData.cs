
using Api.UnitTest.Helpers;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Services.Users.TestData;

internal class ValidCreateUserTestData : TheoryData<CreateUserRequest>
{
    public ValidCreateUserTestData()
    {
        Add(CreateUserTestData.Generate());
        Add(CreateUserTestData.Generate());
        Add(CreateUserTestData.Generate());
    }
}
