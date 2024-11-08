using System.Text;
using AutoMapper;
using LibraryAPI;
using LibraryAPI.LibraryService;
using LibraryAPI.LibraryService.Entities.Models;
using LibraryAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// Start Builder
var builder = WebApplication.CreateBuilder(args);

// Configure logging and configuration sources
builder.Logging.AddConsole();

// Configure configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Configure services
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddLogging();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<JwtSettings>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();

configureDBContext(builder);

//auth
configureAuth(builder);

// Set up API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(0, 1);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

AddSwagger(builder);

// Build!!!!!
var app = builder.Build();

middleWareConfiguration(app);

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();

void configureAuth(WebApplicationBuilder webApplicationBuilder)
{
    // Configure JWT Authentication
    var jwtSettings = webApplicationBuilder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    Console.WriteLine($"Issuer: {jwtSettings?.Issuer}");

    webApplicationBuilder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            if (jwtSettings != null)
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey ??
                        throw new InvalidOperationException("Secret Key is empty")))
                };
            }
        });
    webApplicationBuilder.Services.AddAuthorization();
}

void AddSwagger(WebApplicationBuilder builder1)
{
    builder1.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });

        // Define the JWT Bearer authentication scheme
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
        });

        // Set the security requirement
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                []
            }
        });
    });
}

void middleWareConfiguration(WebApplication webApplication)
{
    // Middleware configuration
    if (webApplication.Environment.IsDevelopment())
    {
        webApplication.UseDeveloperExceptionPage();
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API"));

        using var scope = webApplication.Services.CreateScope();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        var bookRepository = scope.ServiceProvider.GetRequiredService<BookRepository>();
        bookRepository.SeedBooksAsync();
    
        webApplication.UseMiddleware<FakeJwtAuthorizationMiddleware>();
    }
    else
    {
        webApplication.UseExceptionHandler("/Home/Error");
        webApplication.UseHsts();
    }

    webApplication.UseMiddleware<GlobalExceptionHandler>();
}

void configureDBContext(WebApplicationBuilder webApplicationBuilder1)
{
    // Set up database context based on environment
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    Console.WriteLine($"environment: {environment}");
    if (environment == Environments.Development)
    {
        webApplicationBuilder1.Services.AddDbContext<DevAppDbContext>(options =>
            options.UseInMemoryDatabase("LibraryDb")
                .EnableSensitiveDataLogging());
    }
    else
    {
        // Configure production database (uncomment and configure as needed)
        // builder.Services.AddDbContext<DevAppDbContext>(options =>
        //     options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionConnection")));
    }
}


public partial class Program
{
} // allow integration test project to access Program