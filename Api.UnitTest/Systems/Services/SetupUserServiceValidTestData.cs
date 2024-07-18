using Api.UnitTest.Setup;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services;

public class SetupUserServiceValidTestData : TheoryData<List<User>>
{
    public SetupUserServiceValidTestData()
    {
        Add(TestData.GenerateFakeUsers(4));
        Add(TestData.GenerateFakeUsers(12));
        Add(TestData.GenerateFakeUsers(1));
    }
}
