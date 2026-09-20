using System.Net;
using System.Text.Json;

namespace ConferenceRooms.Api.Middleware;
/// <summary>
/// Middleware для глобальної обробки винятків та повернення стандартизованих помилок.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Перехоплює виконання HTTP-запиту та обробляє можливі винятки.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Передаємо управління наступному компоненту конвеєра
            await _next(context);
        }
        catch (Exception ex)
        {
            // Логуємо непередбачувану помилку та формуємо стандартизовану відповідь
            _logger.LogError(ex, "Сталася непередбачувана помилка під час обробки запиту.");
            await HandleExceptionAsync(context, ex);
        }
    }

    // Формує та надсилає стандартизовану JSON-відповідь про помилку з відповідним HTTP-статусом.
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Визначаємо HTTP-статус код залежно від типу винятку
        var statusCode = exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            InvalidOperationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.GetTypedHeaders().ContentLength = null;
        context.Response.StatusCode = (int)statusCode;

        // Формуємо об'єкт відповіді з помилкою
        var response = new
        {
            StatusCode = (int)statusCode,
            Error = exception.Message
        };

        // Серіалізуємо у формат JSON та записуємо у тіло відповіді
        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}