using System;
using System.Collections.Generic;
using AutoMapper;
using LibraryAPI.LibraryService;
using LibraryAPI.LibraryService.Models;
using LibraryAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
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
        
        SetupDbContext(services);

        // Register other services
        services.AddScoped<BookRepository>();
        services.AddScoped<ILibraryService, LibraryService.LibraryService>();

        // Add AutoMapper, controllers, and Swagger
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        SetupVersioning(services);
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

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
                options.UseInMemoryDatabase("LibraryDb:"+Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging());
            services.AddScoped<DevAppDbContext>();
            services.AddScoped<IBookRepository, BookRepository>();
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
            
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DevAppDbContext>();
            context.Database.EnsureCreated();
            
            // context.ClearDB();
            
            // Valid automapper?
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            
            var bookRepository = scope.ServiceProvider.GetRequiredService<BookRepository>();
            bookRepository.SeedBooksAsync();
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