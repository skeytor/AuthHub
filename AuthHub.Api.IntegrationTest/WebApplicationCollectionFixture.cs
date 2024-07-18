namespace AuthHub.Api.IntegrationTest;


[CollectionDefinition(nameof(WebApplicationCollectionFixture))]
public class WebApplicationCollectionFixture 
    : ICollectionFixture<IntegrationTestWebApplicationFactory<Program>>;
