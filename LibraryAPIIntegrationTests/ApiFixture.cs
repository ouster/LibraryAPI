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
        var scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<DevAppDbContext>(); // Get the DbContext

        var factoryWithInMemoryDb = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureServices(services =>
            {
                // Replace the DB context with an in-memory database
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<DevAppDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<DevAppDbContext>(options =>
                    options.UseInMemoryDatabase("IntegrationTestDb:" + Guid.NewGuid().ToString())
                        .EnableSensitiveDataLogging());
            });
        });
        Client = factoryWithInMemoryDb.CreateClient();
        
        _context.SeedData();
    }

    // protected ApiTestFixture(WebApplicationFactory<Program> factory)
    // {
    //     var webHostBuilder = factory.WithWebHostBuilder(builder =>
    //     {
    //         builder.ConfigureServices(services =>
    //         {
    //             // Remove the existing DbContext registration
    //             var descriptor = services.SingleOrDefault(d =>
    //                 d.ServiceType == typeof(DbContextOptions<DevAppDbContext>));
    //             if (descriptor != null)
    //             {
    //                 services.Remove(descriptor);
    //             }
    //
    //             // Add a new DbContext with an in-memory database
    //             services.AddDbContext<DevAppDbContext>(options =>
    //                 options.UseInMemoryDatabase("IntegrationTestDb:" + Guid.NewGuid().ToString())
    //                     .EnableSensitiveDataLogging());
    //         });
    //     });
    //
    //     Client = webHostBuilder.CreateClient();
    //
    //     var scope = Client.Services.CreateScope();
    //     _context = scope.ServiceProvider.GetRequiredService<DevAppDbContext>(); // Get the DbContext
    //
    //     // Seed the database with initial test data
    //     SeedDatabase();
    // }

    private void SeedDatabase()
    {
        // Check if data already exists to avoid duplicates
        if (!_context.Books.Any())
        {
            // Add initial data directly to the DbSet
            _context.Books.AddRange(
                new BookModel(id: 1, title: "1984", author: "George Orwell", isbn: "9781234567897",
                    publishedDate: new DateTime(1949, 6, 8)),
                new BookModel(id: 2, title: "To Kill a Mockingbird", author: "Harper Lee", isbn: "9783127323207",
                    publishedDate: new DateTime(1960, 7, 11)),
                new BookModel(id: 3, title: "The Great Gatsby", author: "F. Scott Fitzgerald", isbn: "4855186747734",
                    publishedDate: new DateTime(1925, 4, 10)),
                new BookModel(id: 4, title: "Brave New World", author: "Aldous Huxley", isbn: "4501169518",
                    publishedDate: new DateTime(1932, 1, 1))
            );

            _context.SaveChanges(); // Persist changes to the in-memory database
        }
    }

    
    public void Dispose()
    {
        _context.Database.EnsureDeleted(); // Clean up database after tests
        _context.Dispose();
    }
}

// public void Dispose()
// {
//     _context.Database.EnsureDeleted(); // Deletes the existing database
//     _context.Dispose();
// }

// protected ApiTestFixture(WebApplicationFactory<Program> factory)
// {
//     _factory = factory.WithWebHostBuilder(builder =>
//     {
//         builder.UseEnvironment("Development"); // TODO review this later
//         builder.ConfigureServices(services =>
//         {
//             // Replace the DB context with an in-memory database
//             var descriptor = services.SingleOrDefault(d =>
//                 d.ServiceType == typeof(DbContextOptions<DevAppDbContext>));
//             if (descriptor != null)
//                 services.Remove(descriptor);
//             
//             services.AddDbContext<DevAppDbContext>(options =>
//                 options.UseInMemoryDatabase("IntegrationTestDb:"+Guid.NewGuid().ToString())
//                     .EnableSensitiveDataLogging());
//         });
//     });
//     Client = _factory.CreateClient();
// }
