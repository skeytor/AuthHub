namespace AuthHub.Api.IntegrationTest.Fixtures;

/// <summary>
/// Defines a collection fixture for sharing a single instance of
/// <see cref="IntegrationTestWebApplicationFactory{TProgram}"/> across multiple tests.
/// This ensures that web application and its services are initialized once and reused
/// throughout the test collection.
/// </summary>
[CollectionDefinition(nameof(WebApplicationCollectionFixture))]
public class WebApplicationCollectionFixture
    : ICollectionFixture<IntegrationTestWebApplicationFactory<Program>>;
