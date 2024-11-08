using System.Data.Common;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryAPI.Middleware;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); // Call the next middleware in the pipeline
        }
        catch (AutoMapperMappingException ex)
        {
            // Log the exception message and the errors
            logger.LogError($"Mapping Failed: {ex.Message}"); // Log the message
            await HandleExceptionAsync(context, ex);
        }
        catch (DbException dbEx)
        {
            // Log the database-related exception
            logger.LogError($"Database error occurred: {dbEx.Message}");
            throw; // TODO or handle as needed
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);

        // Create a response object
        var response = new
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "An unexpected error occurred. Please try again later.",
            Detail = ex.Message // Optionally include the exception message (for development only)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsJsonAsync(response);
    }
}
