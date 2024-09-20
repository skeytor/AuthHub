using AuthHub.Api.Dtos;
using AuthHub.Api.IntegrationTest.Fixtures;
using FluentAssertions;
using System.Net.Http.Json;

namespace AuthHub.Api.IntegrationTest.Systems.Controllers;

[Collection(nameof(WebApplicationCollectionFixture))]
public class RoleController(IntegrationTestWebApplicationFactory<Program> fixuture) 
    : BaseWebApplicationTest(fixuture)
{
    [Fact]
    public async Task GetRoles_Should_ReturnRolesList()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("/api/role");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var data = await response
            .Content
            .ReadFromJsonAsync<IReadOnlyList<RoleResponse>>();
        data.Should().Contain(x => x.Name == "Admin");
    }
}
