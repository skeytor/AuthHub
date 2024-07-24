﻿using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using Bogus;

namespace Api.UnitTest.Setup;

internal static class TestData
{
    public static List<User> GenerateFakeUsers(int count)
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
    public static List<Role> GenerateFakeRoles(int count)
    {
        Faker<Role> roleFaker = new Faker<Role>()
            .RuleFor(r => r.Name, f => f.Name.JobTitle())
            .RuleFor(r => r.Description, f => f.Name.JobDescriptor());
        return roleFaker.Generate(count);
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
