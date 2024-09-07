using AuthHub.Api.Dtos;
using Bogus;

namespace Api.UnitTest.Helpers;

public class UserResponseTestData
{
    private static readonly Faker<UserResponse> _testData = new Faker<UserResponse>()
        .UseSeed(123)
        .CustomInstantiator(f => new UserResponse(
            f.Random.Guid(),
            f.Person.FirstName,
            f.Person.LastName,
            f.Person.Email));
    public static List<UserResponse> Generate(short quantity) => _testData.Generate(quantity);
    public static UserResponse Generate() => _testData.Generate();
}
