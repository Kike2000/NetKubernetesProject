using System;
using System.Net;
using Newtonsoft.Json;

namespace NetKubernetes.Middleware;

public class ManagerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManagerMiddleware> _logger;
    public ManagerMiddleware(RequestDelegate next, ILogger<ManagerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await ManagerExceptionAsync(httpContext, ex, _logger);
        }
    }
    private async Task ManagerExceptionAsync(HttpContext httpContext, Exception ex, ILogger<ManagerMiddleware> logger)
    {
        object? errors = null;
        switch (ex)
        {
            case MiddlewareException me:
                logger.LogError(ex, "Middleware Error");
                errors = me.Errors;
                httpContext.Response.StatusCode = (int)me.StatusCode;
                break;
            case Exception exception:
                logger.LogError(ex, "Error de servidor");
                errors = string.IsNullOrWhiteSpace(exception.Message) ? "Error" : exception.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }
        httpContext.Response.ContentType = "application/json";
        var resultados = string.Empty;
        if (errors != null)
        {
            resultados = JsonConvert.SerializeObject(new { errors });
        }
        await httpContext.Response.WriteAsync(resultados);
    }
}