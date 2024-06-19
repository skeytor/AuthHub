using AuthHub.Domain.Entities;
using AuthHub.Persistence.Repositories;
using AuthHub.UnitTests.Helpers;

namespace AuthHub.UnitTests.Systems.AuthHub.Persistence.Repositories;

public class RoleRepositoryTests
{
    [Theory, ClassData(typeof(RoleDataTest))]
    public async Task GetAllAsync_Should_ReturnAllRoles_WhenRolesExists(List<Role> roles)
    {
        // Arrange
        var dbContext = AppDbContextGeneratorTest.Generate();
        RoleRepository roleRepository = new(dbContext);
        await dbContext.Roles.AddRangeAsync(roles);
        await dbContext.SaveChangesAsync();
        
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
                Id = i + ,
                Description = Faker.Lorem.Sentence(6),
                Name = Faker.Lorem.Sentence(1),
            });
        }
        return data;
    }
}