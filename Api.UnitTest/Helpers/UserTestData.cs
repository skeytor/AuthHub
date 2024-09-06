using AuthHub.Domain.Entities;
using Bogus;

namespace Api.UnitTest.Helpers;

public class UserTestData
{
    private static readonly Faker<User> Faker = new Faker<User>()
        .UseSeed(123)
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.FirstName, f => f.Person.FirstName)
        .RuleFor(p => p.LastName, f => f.Person.LastName)
        .RuleFor(p => p.Email, f => f.Person.Email)
        .RuleFor(p => p.Username, f => f.Person.UserName)
        .RuleFor(p => p.IsActive, f => f.Random.Bool())
        .RuleFor(p => p.Password, f => f.Internet.Password())
        .RuleFor(p => p.RoleId, f => f.Random.Number(1, 5));

    public static List<User> Generate(short quantity) => Faker.Generate(quantity);
    public static User Generate() => Faker.Generate();
}
