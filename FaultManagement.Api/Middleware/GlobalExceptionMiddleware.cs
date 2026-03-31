using FaultManagement.Api.Middleware;
using FaultManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FaultManagement.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ProblemDetails();

        if (exception is InvalidStatusTransitionException)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            response.Status = 422;
            response.Title = "Invalid Status Transition";
            response.Detail = exception.Message;
        }
        else if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            response.Status = 403;
            response.Title = "Forbidden";
            response.Detail = exception.Message;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            response.Status = 500;
            response.Title = "Internal Server Error";
            response.Detail = "An unexpected error occurred.";
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
