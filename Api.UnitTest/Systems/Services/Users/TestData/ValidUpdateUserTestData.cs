
using Api.UnitTest.Helpers;
using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

internal class ValidUpdateUserTestData : TheoryData<CreateUserRequest, User>
{
    public ValidUpdateUserTestData()
    {
        Add(CreateUserTestData.Generate(), UserTestData.Generate());
        Add(CreateUserTestData.Generate(), UserTestData.Generate());
        Add(CreateUserTestData.Generate(), UserTestData.Generate());
    }
}
