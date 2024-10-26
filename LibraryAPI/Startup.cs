using System;
using AutoMapper;
using LibraryAPI.LibraryService;
using LibraryAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
        
        SetupDbContext(services);

        // Register other services
        services.AddScoped<ILibraryService, LibraryService.LibraryDbService>();

        // Add AutoMapper, controllers, and Swagger
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        SetupVersioning(services);
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(/*c =>
        {
            c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Library API", Version = "v1.0" });
            c.SwaggerDoc("v2.0", new OpenApiInfo { Title = "Library API", Version = "v2.0" });
        }*/);

    }

    private static void SetupVersioning(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true; // Optional, helps to inform clients about versions
            options.AssumeDefaultVersionWhenUnspecified = true; // Set a default version
            options.DefaultApiVersion = new ApiVersion(0, 1); // Set default version
            options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Read version from URL
        });
    }

    private static void SetupDbContext(IServiceCollection services)
    {
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
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseApiVersioning();
        
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API"));
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