using Api.UnitTest.Setup;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services;

public class SetupUserServiceValidUsersTestData : TheoryData<List<User>>
{
    public SetupUserServiceValidUsersTestData()
    {
        Add(UserTestDataFactory.CreateMultiple(4));
        Add(UserTestDataFactory.CreateMultiple(12));
        Add(UserTestDataFactory.CreateMultiple(1));
    }
}

public class SetupUserServiceGuidUserTestData : TheoryData<Guid, User>
{
    public SetupUserServiceGuidUserTestData()
    {
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingle());
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingle());
        Add(Guid.NewGuid(), UserTestDataFactory.CreateSingle());
    }
}
