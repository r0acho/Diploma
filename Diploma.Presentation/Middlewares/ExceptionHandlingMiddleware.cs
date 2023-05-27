using System.Net;
using Diploma.Domain.Responses;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Presentation.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(httpContext,
                ex.Message,
                HttpStatusCode.NotFound,
                "Entity not found by requested ID");
        }
        catch (DbUpdateException ex)
        {
            await HandleExceptionAsync(httpContext,
                ex.Message,
                HttpStatusCode.Conflict,
                "Session response already exists for completed session");
        }
        catch (HttpRequestException ex)
        {
            await HandleExceptionAsync(httpContext,
                ex.Message,
                HttpStatusCode.RequestTimeout,
                "Client closed connection");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext,
                ex.Message,
                HttpStatusCode.InternalServerError,
                "Internal server error");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, string exMsg, HttpStatusCode httpStatusCode,
        string message)
    {
        _logger.LogError(exMsg);

        var response = context.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        BaseResponse errorDto = new()
        {
            Description = message,
            StatusCode = httpStatusCode
        };

        await response.WriteAsJsonAsync(errorDto);
    }
}