using AutoMapper;
using LibraryAPI.LibraryService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



namespace LibraryAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateBuilder(args);
        var app = builder.Build();
        
        EnsureDbIsCreated(app);

        var mapper = app.Services.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid(); 
        
        app.Run();
    }

    private static void EnsureDbIsCreated(IHost app) //TODO review this as add real db
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DevAppDbContext>();
        context.Database.EnsureCreated();
    }

    private static IHostBuilder CreateBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}