using AuthHub.Domain.Entities;
using AuthHub.Persistence;
using AuthHub.Persistence.Repositories;
using AuthHub.UnitTests.Context;

namespace AuthHub.UnitTests.Systems.AuthHub.Persistence.Repositories;

public class RoleRepositoryTests
{
    [Theory, ClassData(typeof(RoleDataTest))]
    public async Task GetAllAsync_Should_ReturnAllRoles_WhenRolesExists(List<Role> roles)
    {
        // Arrange
        using AppDbContext _context = AppDbContextFactoryTest
            .CreateSQLiteDatabaseInMemory();
        RoleRepository roleRepository = new(_context);
        await _context.Roles.AddRangeAsync(roles);
        await _context.SaveChangesAsync();

        // Act
        var result = await roleRepository.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.IsAssignableFrom<IReadOnlyCollection<Role>>(result);
        Assert.Equal(roles.Count, result.Count);
    }
}

public class RoleDataTest : TheoryData<List<Role>>
{
    public RoleDataTest()
    {
        Add(GenerateData(10));
    }
    private static List<Role> GenerateData(int lenght)
    {
        List<Role> data = [];
        for (int i = 0; i < lenght; i++)
        {
            data.Add(new()
            {
                Name = Faker.Name.First(),
                Description = Faker.Lorem.Sentence(3)
            });
        }
        return data;
    }
}