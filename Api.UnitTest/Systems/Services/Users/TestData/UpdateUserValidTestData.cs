using Api.UnitTest.Setup.Factories;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Users.TestData;

public class UpdateUserValidTestData : TheoryData<Guid, User>
{
    public UpdateUserValidTestData()
    {
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingleUser());
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingleUser());
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingleUser());
    }
}
