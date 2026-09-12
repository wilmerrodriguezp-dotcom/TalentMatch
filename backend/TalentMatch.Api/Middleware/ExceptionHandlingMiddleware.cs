using System.Net;
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
            _logger.LogError(ex, "Ocurrió una excepción no controlada.");

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor",
                Detail = "Ha ocurrido un error inesperado. Por favor, contacte al administrador.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7807"
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
