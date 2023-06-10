using System.Net;
using Diploma.Domain.Responses;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Presentation.Middlewares;

/// <summary>
/// Класс Middleware, перехватывающий необработанные исключения
/// </summary>
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

    /// <summary>
    /// Метод вызова следующего метода из цепочки вызовов
    /// </summary>
    /// <param name="httpContext">Контекст запроса</param>
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

    /// <summary>
    /// Метод отправки статуса об ошибке в формате JSON
    /// </summary>
    /// <param name="context">Контекст запроса</param>
    /// <param name="exMsg">Сообщение об ошибке (текст из Exception)</param>
    /// <param name="httpStatusCode">HTTP-код ответа</param>
    /// <param name="message">Ответ сервера</param>
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