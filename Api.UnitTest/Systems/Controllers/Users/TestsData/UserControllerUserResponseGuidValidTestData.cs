using Api.UnitTest.Setup.Factories;
using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Users.TestsData;

public class UserControllerUserResponseGuidValidTestData : TheoryData<Guid, UserResponse>
{
    public UserControllerUserResponseGuidValidTestData()
    {
        Add(Guid.NewGuid(), UserTestDataFactory.CreateManyUserResponses(1).First());
        Add(Guid.NewGuid(), UserTestDataFactory.CreateManyUserResponses(1).First());
        Add(Guid.NewGuid(), UserTestDataFactory.CreateManyUserResponses(1).First());
    }

}
