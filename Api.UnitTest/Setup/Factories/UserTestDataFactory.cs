using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using Bogus;

namespace Api.UnitTest.Setup.Factories;

internal static class UserTestDataFactory
{
    public static List<User> CreateMultiple(int count)
    {
        Faker<User> userFaker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.IsActive, f => f.Random.Bool());
        return userFaker.Generate(count);
    }
    public static List<CreateUserRequest> CreateMultipleUserRequest(int count)
    {
        Faker<CreateUserRequest> faker = new Faker<CreateUserRequest>()
            .CustomInstantiator(f =>
                new CreateUserRequest(
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    f.Internet.UserName(),
                    f.Internet.Email(),
                    f.Internet.Password(),
                    f.Random.Number(1, 7)));
        return faker.Generate(count);
    }
    public static User CreateSingle()
    {
        return CreateMultiple(1).First();
    }

    public static CreateUserRequest CreateSingleUserRequest()
    {
        return CreateMultipleUserRequest(1).First();
    }
    public static List<UserResponse> GenerateFakeUsersResponse(int count)
    {
        var fakerUserResponse = new Faker<UserResponse>()
            .CustomInstantiator(f =>
                new UserResponse(
                    Guid.NewGuid(),
                    f.Name.FirstName(),
                    f.Internet.Email(),
                    f.Name.LastName()));
        return fakerUserResponse.Generate(count, nameof(fakerUserResponse));
    }
}
