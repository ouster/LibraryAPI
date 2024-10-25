using AutoMapper;
using Microsoft.EntityFrameworkCore;

using LibraryAPI.LibraryService;
using LibraryAPI.LibraryService.Models;
using LibraryAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//logging
builder.Logging.AddConsole(); 

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var environment = builder.Environment.EnvironmentName;

// Setup DB
if (environment == "Development")
{
    builder.Services.AddDbContext<DevAppDbContext>(options =>
        options.UseInMemoryDatabase("LibraryDb"));
}
else
{
    // Use real SQL Server (or other) database for Production, UAT, etc.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<DevAppDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// DI Services
builder.Services.AddScoped<ILibraryService, LibraryService>();

// Automapper
var mapper = builder.Services.AddAutoMapper(typeof(BookMapperProfile));

var app = builder.Build();

// Automapper Valid?
var map = app.Services.GetRequiredService<IMapper>();
map.ConfigurationProvider.AssertConfigurationIsValid(); // Throws exception if mappings are invalid

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    
    // Ensure database is created
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DevAppDbContext>();
    context.Database.EnsureCreated();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
