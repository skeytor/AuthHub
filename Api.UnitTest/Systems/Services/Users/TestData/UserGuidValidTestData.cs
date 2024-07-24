using Api.UnitTest.Setup;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class UserGuidValidTestData : TheoryData<Guid, User>
{
    public UserGuidValidTestData()
    {
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingle());
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingle());
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingle());
    }
}
