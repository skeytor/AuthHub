using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

internal class InvalidUserTestData : TheoryData<Guid, User?>
{
    public InvalidUserTestData()
    {
        Add(Guid.NewGuid(), null);
        Add(Guid.NewGuid(), null);
        Add(Guid.NewGuid(), null);
    }
}