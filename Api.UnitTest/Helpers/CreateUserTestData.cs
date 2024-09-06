using AuthHub.Api.Dtos;
using Bogus;

namespace Api.UnitTest.Helpers;

public class CreateUserTestData
{
    private static readonly Faker<CreateUserRequest> _testData = new Faker<CreateUserRequest>()
        .UseSeed(123)
        .CustomInstantiator(f => new CreateUserRequest(
            f.Person.FirstName,
            f.Person.LastName,
            f.Person.UserName,
            f.Person.Email,
            f.Internet.Password(),
            f.Random.Number(1, 5)));
    
    public static List<CreateUserRequest> Generate(short quantity) =>
        _testData.Generate(quantity);
    public static CreateUserRequest Generate() => _testData.Generate();
}
