using Microsoft.AspNetCore.Mvc;

namespace TalentMatch.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            var (statusCode, title, detail) = ex switch
            {
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "No autorizado", "Credenciales inválidas."),
                ArgumentException => (StatusCodes.Status400BadRequest, "Solicitud inválida", "Los datos enviados no son válidos."),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor", "Ha ocurrido un error inesperado. Por favor, contacte al administrador.")
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(ex, "Ocurrió una excepción no controlada.");
            }
            else
            {
                _logger.LogWarning("Solicitud rechazada con HTTP {StatusCode}.", statusCode);
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Type = "https://datatracker.ietf.org/doc/html/rfc7807"
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
