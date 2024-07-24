using Api.UnitTest.Setup;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class UserListValidTestData : TheoryData<List<User>>
{
    public UserListValidTestData()
    {
        Add(UserTestDataFactory.CreateMultiple(4));
        Add(UserTestDataFactory.CreateMultiple(12));
        Add(UserTestDataFactory.CreateMultiple(1));
    }
}
