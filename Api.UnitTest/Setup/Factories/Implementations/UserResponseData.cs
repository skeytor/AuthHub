using AuthHub.Api.Dtos;
using Bogus;

namespace Api.UnitTest.Setup.Factories.Implementations;

internal class UserResponseData : ITestData<UserResponse>
{
    private Faker<UserResponse> Faker { get; set; }
    public UserResponseData()
    {
        Faker = new Faker<UserResponse>()
            .CustomInstantiator(f =>
                new UserResponse(
                    Guid.NewGuid(),
                    f.Name.FirstName(),
                    f.Internet.Email(),
                    f.Name.LastName()));
    }
    public List<UserResponse> Multiple(int n)
    {
        return Faker.Generate(n);
    }

    public UserResponse Single()
    {
        return Faker.Generate();
    }
}
