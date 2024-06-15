using AuthHub.Domain.Entities;
using System.Data;

namespace AuthHub.UnitTests.Systems.AuthHub.Domain.Entities
{
    public class UserTests
    {
        // [ThingUnderTest]_Should_[ExpectedResult]_[Conditions]
        [Fact]
        public void Create_Should_ReturnSucces_WhenValueIsNotNull()
        {
            var user = new User()
            {
                FirstName = "Rober",
                LastName = "Guerrero",
                Email = "example@email.com",
                IsActive = true,
                Id = new Guid(),
                RoleId = 1,
                Username = "example"
            };
            Assert.True(user.IsActive);
            Assert.Equal("example", user.Username);
            Assert.NotNull(user);
        }

        [Fact]
        public void AddRole_Should_ReturnSucces_WhenRoleValueIsNotNull()
        {
            var role = new Role()
            {
                Id = 1,
                Name = "Admin",
                Description = "This is admin user",
            };
            var user = new User()
            {
                FirstName = "Rober",
                LastName = "Guerrero",
                Email = "example@email.com",
                IsActive = true,
                Id = new Guid(),
                Role = role,
                Username = "example"
            };
            Assert.NotNull(user.Role);
        }
    }
}
