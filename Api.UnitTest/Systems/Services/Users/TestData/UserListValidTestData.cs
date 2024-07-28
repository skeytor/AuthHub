using Api.UnitTest.Setup.Factories;
using Api.UnitTest.Setup.Factories.Implementations;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class UserListValidTestData : TheoryData<List<User>>
{
    public UserListValidTestData()
    {
        /*var testData = new TestDataFactory<UserData, User>().Create();
        Add(testData.Multiple(6));
        Add(testData.Multiple(1));
        Add(testData.Multiple(4));
        Add(testData.Multiple(10));*/
        Add(UserTestDataFactory.CreateManyUsers(2));
        Add(UserTestDataFactory.CreateManyUsers(5));
        Add(UserTestDataFactory.CreateManyUsers(6));
    }
}
