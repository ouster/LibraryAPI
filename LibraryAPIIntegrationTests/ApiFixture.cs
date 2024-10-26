using System.Linq;
using System.Net.Http;
using LibraryAPI;
using LibraryAPI.LibraryService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LibraryAPIIntegrationTests;

public abstract class ApiTestFixture : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public HttpClient Client { get; }
    protected string BaseUrl { get; } = "/api/v0.1/library";

    protected ApiTestFixture(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development"); // TODO review this later
            builder.ConfigureServices(services =>
            {
                // Replace the DB context with an in-memory database
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<DevAppDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                
                services.AddDbContext<DevAppDbContext>(options =>
                    options.UseInMemoryDatabase("IntegrationTestDb"));
            });
        });
        Client = _factory.CreateClient();
    }
}
