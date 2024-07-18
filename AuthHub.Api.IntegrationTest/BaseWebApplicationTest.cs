﻿using AuthHub.Persistence.Abstractions;

namespace AuthHub.Api.IntegrationTest;

public abstract class BaseWebApplicationTest(IntegrationTestWebApplicationFactory<Program> factory)
{
    protected readonly HttpClient _httpClient = factory.CreateClient();
    protected readonly IAppDbContext _context = factory.Context!;
}