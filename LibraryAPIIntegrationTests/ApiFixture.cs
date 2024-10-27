using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using LibraryAPI;
using LibraryAPI.LibraryService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibraryAPI.LibraryService.Models;
using Xunit;

namespace LibraryAPIIntegrationTests;

public abstract class ApiTestFixture : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly DevAppDbContext _context;
    public HttpClient Client { get; }
    protected string BaseUrl { get; } = "/api/v0.1/library";

    protected ApiTestFixture(WebApplicationFactory<Program> factory)
    {        
        factory = factory.WithWebHostBuilder(builder =>
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
        var scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<DevAppDbContext>();
        Client = factory.CreateClient();
    }

    
    public void Dispose()
    {
        // _context.Database.EnsureDeleted(); // Clean up database after tests
        _context.Dispose();
    }
}
