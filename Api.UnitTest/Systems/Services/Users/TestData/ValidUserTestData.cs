using Api.UnitTest.Helpers;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

internal class ValidUserTestData : TheoryData<Guid, User>
{
    public ValidUserTestData()
    {
        Add(Guid.NewGuid(), UserTestData.Generate());
        Add(Guid.NewGuid(), UserTestData.Generate());
        Add(Guid.NewGuid(), UserTestData.Generate());
    }
}
