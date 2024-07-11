using AuthHub.Domain.Entities;
using Persistence.UnitTest.Data;

namespace Api.UnitTest.Services;

public class SetupUserServiceValidTestData : TheoryData<List<User>>
{
    public SetupUserServiceValidTestData()
    {
        Add(DataGenerator.GenerateFakeUsers(4));
        Add(DataGenerator.GenerateFakeUsers(1));
        Add(DataGenerator.GenerateFakeUsers(100));
        Add(DataGenerator.GenerateFakeUsers(75));
    }
}
