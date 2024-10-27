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
    {    var scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<DevAppDbContext>();
    }

    
    public void Dispose()
    {
        _context.Database.EnsureDeleted(); // Clean up database after tests
        _context.Dispose();
    }
}
