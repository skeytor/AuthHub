namespace AuthHub.Api.IntegrationTest;

public class UserControllerTests
{
    [Fact]
    public async Task GetAllUsersRequest_Should_ReturnSucces()
    {
        // Arrange
        var application = new IntegrationTestWebApplicationFactory();
        HttpClient client = application.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/user");

        // Assertions
        response.EnsureSuccessStatusCode();
    }
}
