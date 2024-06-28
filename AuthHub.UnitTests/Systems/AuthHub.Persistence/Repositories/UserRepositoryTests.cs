using AuthHub.Domain.Entities;
using AuthHub.Persistence;
using AuthHub.Persistence.Repositories;
using AuthHub.UnitTests.Helpers;

namespace AuthHub.UnitTests.Systems.AuthHub.Persistence.Repositories;

public class UserRepositoryTests
{

    [Fact]
    public async Task CreateAsync_Should_ReturnUserId_WhenValueIsNotNull()
    {
        // Arrange
        var dbContext = AppDbContextFactoryTest.CreateDatabaseInMemory();
        UserRepository userRepository = new(dbContext);
        var user = new User()
        {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            Email = Faker.Internet.Email(),
            Id = Guid.NewGuid(),
            IsActive = Faker.Boolean.Random(),
            Password = DateTime.Now.ToString(),
            RoleId = Faker.RandomNumber.Next(0, 4),
            Username = Faker.Internet.UserName(),
        };

        // Act
        var userCreated = await userRepository.CreateAsync(user);
        await dbContext.SaveChangesAsync();
        // Assert
        Assert.NotNull(userCreated);
        Assert.IsType<User>(userCreated);
        Assert.Equal(user.Id, userCreated.Id);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnException_WhenEmailExists()
    {
        // Arrange
        var dbContext = AppDbContextFactoryTest.CreateDatabaseInMemory();
        UserRepository userRepository = new(dbContext);
        var testUser = new User()
        {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            Email = "example@email.com",
            Id = Guid.NewGuid(),
            IsActive = Faker.Boolean.Random(),
            Password = DateTime.Now.ToString(),
            RoleId = Faker.RandomNumber.Next(0, 4),
            Username = Faker.Internet.UserName(),
        };

        await dbContext.AddAsync(testUser);
        await dbContext.SaveChangesAsync();

        var user = new User()
        {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            Email = "example@email.com",
            Id = Guid.NewGuid(),
            IsActive = Faker.Boolean.Random(),
            Password = DateTime.Now.ToString(),
            RoleId = Faker.RandomNumber.Next(0, 4),
            Username = Faker.Internet.UserName(),
        };
        // Act
        var result = await userRepository.CreateAsync(user);

        // Assert
        await Assert.ThrowsAsync<Exception>(async () => await dbContext.SaveChangesAsync());
    }

    [Theory, ClassData(typeof(UserDataTest))]
    public async Task GetAllAsync_Should_ReturnAllUsers_WhenUsersExists(List<User> users, List<Role> roles)
    {
        // Arrange
        using var _context = AppDbContextFactoryTest.CreateSQLiteDatabaseInMemory();
        UserRepository userRepository = new(_context);
        await _context.Roles.AddRangeAsync(roles);
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await userRepository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(users.Count, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnEmpty_WhenUsersNotExists()
    {
        // Arrange
        var dbContext = AppDbContextFactoryTest.CreateDatabaseInMemory();
        UserRepository userRepository = new(dbContext);

        // Act
        var result = await userRepository.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    [Theory, ClassData(typeof(UserDataTest))]
    public async Task GetByIdAsync_Should_ReturnUser_WhenUserExists(List<User> users)
    {
        // Arrange
        var dbContext = AppDbContextFactoryTest.CreateDatabaseInMemory();
        UserRepository userRepository = new(dbContext);
        Random rand = new();
        int number = rand.Next(0, users.Count);
        Guid id = users[number].Id;
        await dbContext.Users.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Theory, ClassData(typeof(UserDataTest))]
    public async Task GetByIdAsync_Should_ReturnNull_WhenUserNotExists(List<User> users)
    {
        // Arrange
        var dbContext = AppDbContextFactoryTest.CreateDatabaseInMemory();
        UserRepository userRepository = new(dbContext);
        await dbContext.Users.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
}

public class UserDataTest : TheoryData<List<User>, List<Role>>
{
    public UserDataTest()
    {
        (List<User> Users, List<Role> Roles) = GenerateFakeData(50, 2);
        Add(Users, Roles);
    }
    private static List<User> GenerateUsers(int lenght)
    {
        List<User> data = [];
        for (int i = 0; i < lenght; i++)
        {
            data.Add(new()
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                Email = Faker.Internet.Email(),
                Id = Guid.NewGuid(),
                IsActive = Faker.Boolean.Random(),
                Password = DateTime.Now.ToString(),
                RoleId = Faker.RandomNumber.Next(0, 4),
                Username = Faker.Internet.UserName(),
            });
        }
        return data;
    }
    private static (List<User> Users, List<Role> Roles) GenerateFakeData(int nroUsers, int nroRoles)
    {
        List<User> users = [];
        List<Role> roles = [];
        for (int i = 0; i < nroRoles; i++)
        {
            roles.Add(new()
            {
                Name = "Admin" + i,
                Description = Faker.Lorem.Sentence(1)
            });
        }
        for (int i = 0; i < nroUsers; i++)
        {
            users.Add(new()
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                Email = Faker.Internet.Email(),
                Id = Guid.NewGuid(),
                IsActive = Faker.Boolean.Random(),
                Password = DateTime.Now.ToString(),
                RoleId = Faker.RandomNumber.Next(1, roles.Count),
                Username = Faker.Internet.UserName(),
            });
        }
        return (users, roles);
    }
}
