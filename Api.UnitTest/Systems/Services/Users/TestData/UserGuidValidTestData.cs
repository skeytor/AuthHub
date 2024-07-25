using Api.UnitTest.Setup.Factories;
using Api.UnitTest.Setup.Factories.Implementations;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class UserGuidValidTestData : TheoryData<Guid, User>
{
    public UserGuidValidTestData()
    {
        var testData = new TestDataFactory<UserData, User>().Create();
        Add(Guid.NewGuid(), testData.Single());
        Add(Guid.NewGuid(), testData.Single());
        Add(Guid.NewGuid(), testData.Single());
    }
}
