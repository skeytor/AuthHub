using AuthHub.Api.IntegrationTest.Fixtures;
using Xunit.Abstractions;

namespace AuthHub.Api.IntegrationTest.Systems.Controllers;

[Collection(nameof(WebApplicationCollectionFixture))]
public class WeatherForecastTestController(
    IntegrationTestWebApplicationFactory<Program> fixture,
    ITestOutputHelper testOutputHelper) : BaseWebApplicationTest(fixture, testOutputHelper)
{
    [Theory]
    [InlineData("/WeatherForecast")]
    public async Task Get_Forecast(string path)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(path);
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(message);
        response.EnsureSuccessStatusCode();
    }
}
