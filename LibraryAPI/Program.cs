using System;
using System.IO;
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
using Microsoft.Extensions.Logging;


namespace LibraryAPI;

public class Program
{
    private readonly StartupHelper _startupHelper;
    
    public static void Main(string[] args)
    {
        var builder = CreateBuilder(args);

        var startup = new StartupHelper(builder.Configuration, builder.Environment);
        startup.ConfigureServices(builder.Services);
        
        var app = builder.Build();
        
        startup.Configure(app, app.Environment);

        app.Run();
    }
    
    private static WebApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();


        return builder;
    }
}