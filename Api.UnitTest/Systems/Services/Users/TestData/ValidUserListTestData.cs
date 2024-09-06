
using Api.UnitTest.Helpers;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

internal class ValidUserListTestData : TheoryData<List<User>>
{
    public ValidUserListTestData()
    {
        Add(UserTestData.Generate(4));
        Add(UserTestData.Generate(5));
        Add(UserTestData.Generate(1));
        Add(UserTestData.Generate(2));
    }
}
