using AuthHub.Domain.Entities;
using Bogus;

namespace Api.UnitTest.Setup.Factories.Implementations;

internal class UserData : ITestData<User>
{
    private Faker<User> Faker { get; set; }
    public UserData()
    {
        Faker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.IsActive, f => f.Random.Bool());
    }
    public List<User> Multiple(int n)
    {
        return Faker.Generate(n);
    }

    public User Single()
    {
        return Faker.Generate();
    }
}
