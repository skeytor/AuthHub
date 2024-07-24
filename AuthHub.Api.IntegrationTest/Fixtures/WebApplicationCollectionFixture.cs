namespace AuthHub.Api.IntegrationTest.Fixtures;


[CollectionDefinition(nameof(WebApplicationCollectionFixture))]
public class WebApplicationCollectionFixture
    : ICollectionFixture<IntegrationTestWebApplicationFactory<Program>>;
