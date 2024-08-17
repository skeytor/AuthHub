using Api.UnitTest.Setup.Factories;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class UserListValidTestData : TheoryData<List<User>>
{
    public UserListValidTestData()
    {
        Add(UserTestDataFactory.CreateManyUsers(2));
        Add(UserTestDataFactory.CreateManyUsers(5));
        Add(UserTestDataFactory.CreateManyUsers(6));
    }
}
