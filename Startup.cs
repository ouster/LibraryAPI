using System;
using LibraryAPI.LibraryService;
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
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // Configure services
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure database context based on environment
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        if (environment == "IntegrationTesting" || environment == Environments.Development)
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
        services.AddScoped<ILibraryService, LibraryService.LibraryService>();

        // Add AutoMapper, controllers, and Swagger
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    // Configure HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1"));
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

    }
}