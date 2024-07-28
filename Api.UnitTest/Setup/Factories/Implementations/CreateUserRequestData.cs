using AuthHub.Api.Dtos;
using Bogus;

namespace Api.UnitTest.Setup.Factories.Implementations;

internal class CreateUserRequestData : ITestData<CreateUserRequest>
{
    private Faker<CreateUserRequest> Faker { get; set; }
    public CreateUserRequestData()
    {
        Faker = new Faker<CreateUserRequest>()
            .CustomInstantiator(f =>
                new CreateUserRequest(
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    f.Internet.UserName(),
                    f.Internet.Email(),
                    f.Internet.Password(),
                    f.Random.Number(1, 7)));
    }

    public List<CreateUserRequest> Multiple(int n)
    {
        return Faker.Generate(n);
    }

    public CreateUserRequest Single()
    {
        return Faker.Generate();
    }
}
