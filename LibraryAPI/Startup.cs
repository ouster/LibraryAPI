using System;
using AutoMapper;
using LibraryAPI.LibraryService;
using LibraryAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Your service and model namespaces

namespace LibraryAPI;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    // Configure services
    public void ConfigureServices(IServiceCollection services)
    {
        
        services.AddLogging();
        
        // Configure database context based on environment
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        if (environment == Environments.Development)
        {
            // Use an in-memory database for integration tests
            services.AddDbContext<DevAppDbContext>(options =>
                options.UseInMemoryDatabase("LibraryDb"));
        }
        else
        {
            // Use a different production-ready configuration
            // services.AddDbContext<DevAppDbContext>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection")));
        }

        // Register other services
        services.AddScoped<ILibraryService, LibraryService.LibraryDbService>();

        // Add AutoMapper, controllers, and Swagger
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1"));
            app.UseDeveloperExceptionPage();
            
            // Ensure database is created
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DevAppDbContext>();
            context.Database.EnsureCreated();
            
            // Valid automapper?
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid(); //
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        
        app.UseMiddleware<GlobalExceptionHandler>();
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

    }
}