using AuthHub.Domain.Entities;
using AuthHub.Persistence.Abstractions;
using AuthHub.Persistence.Repositories;
using Persistence.UnitTest.Data;
using Persistence.UnitTest.Fixtures;

namespace Persistence.UnitTest.Repositories;

public class UserRespositoryTest(AppDbContextFixture fixture)
    : IClassFixture<AppDbContextFixture>
{
    private readonly IAppDbContext _context = fixture.context;
    private readonly IUnitOfWork _unitOfWork = fixture.unitOfWork;

    [Fact]
    public async Task CreateAsync_Should_ReturnUserEntity_When()
    {
        // Arrange
        Role role = new()
        {
            Name = "Admin",
            Description = "This is an administrator role"
        };
        User user = new()
        {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            Email = Faker.Internet.Email(),
            IsActive = true,
            Password = Faker.Lorem.Sentence(),
            Username = Faker.Name.FullName(),
            Role = role
        };
        await _context.Roles.AddAsync(role);
        UserRepository userRepository = new(_context);

        // Act
        var sut = await userRepository.CreateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        Assert.NotNull(sut);
        Assert.IsType<User>(sut);
        Assert.IsType<Guid>(sut.Id);
        Assert.NotNull(sut.Role);
    }

    [Theory, ClassData(typeof(UserTestData))]
    public async Task GetById_Should_ReturnUser_WhenUserExists(List<User> userExpect, List<Role> roles)
    {
        // Arrange
        // Insert some users with their roles
        List<User> users = userExpect
            .Select(u =>
            {
                u.Role = roles.First();
                return u;
            })
            .ToList();
        await _context.Roles.AddRangeAsync(roles);
        await _context.Users.AddRangeAsync(userExpect);
        await _unitOfWork.SaveChangesAsync();
        Guid id = users[new Random().Next(1, users.Count)].Id;
        UserRepository userRepository = new(_context);

        // Act
        User? sut = await userRepository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(sut);
        Assert.Equal(id, sut.Id);
        Assert.IsType<User>(sut);
    }

    [Theory, ClassData(typeof(UserTestData))]
    public async Task GetAll_Should_ReturnAllUsers_WhenUsersExist(List<User> usersExpect, List<Role> roles)
    {
        // Arrange
        List<User> users = usersExpect
            .Select(u =>
            {
                u.Role = roles.First();
                return u;
            })
            .ToList();
        await _context.Roles.AddRangeAsync(roles);
        await _context.Users.AddRangeAsync(users);
        await _unitOfWork.SaveChangesAsync();
        UserRepository userRepository = new(_context);
        // Act
        IReadOnlyCollection<User> sut = await userRepository.GetAllAsync();

        // Assert
        Assert.NotEmpty(sut);
        Assert.Equal(usersExpect.Count, sut.Count);
        Assert.IsAssignableFrom<IReadOnlyCollection<User>>(sut);
    }
}

public class UserTestData : TheoryData<List<User>, List<Role>>
{
    public UserTestData()
    {
        Add(DataGenerator.GenerateFakeUsers(10), DataGenerator.GenerateFakeRoles(6));
    }
}