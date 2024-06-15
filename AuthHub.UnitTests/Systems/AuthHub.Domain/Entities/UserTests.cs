using AuthHub.Domain.Entities;

namespace AuthHub.UnitTests.Systems.AuthHub.Domain.Entities
{
    public class UserTests
    {
        // [ThingUnderTest]_Should_[ExpectedResult]_[Conditions]
        [Fact]
        public void Create_Should_ReturnSuccess_WhenValueIsNotNull()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "Your First Name",
                LastName = "Guerrero",
                Email = "example@email.com",
                IsActive = true,
                Id = new Guid(),
                RoleId = 1,
                Username = "example"
            };
            // Assert
            Assert.True(user.IsActive);
            Assert.Equal("example", user.Username);
            Assert.NotNull(user);
        }

        [Fact]
        public void AddRole_Should_ReturnSuccess_WhenRoleValueIsNotNull()
        {
            // Arrange
            var role = new Role()
            {
                Id = 1,
                Name = "Admin",
                Description = "This is admin user",
            };
            var user = new User()
            {
                FirstName = "Your First Name",
                LastName = "Your Last Name",
                Email = "example@email.com",
                IsActive = true,
                Id = new Guid(),
                Role = role,
                Username = "example"
            };
            // Assert
            Assert.NotNull(user.Role);
        }
    }

    public class UserCreateTestData : TheoryData<User>
    {
        public UserCreateTestData()
        {
            Add(new User
            {

                FirstName = "Your First Name",
                LastName = "Guerrero",
                Email = "example@email.com",
                IsActive = true,
                Id = new Guid(),
                RoleId = 1,
                Username = "example"

            });

        }
    }
}
