using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class UserGuidInvalidTestData : TheoryData<Guid, User?>
{
    public UserGuidInvalidTestData()
    {
        Add(Guid.NewGuid(), null);
        Add(Guid.NewGuid(), null);
        Add(Guid.NewGuid(), null);
    }
}