using System.Text.Json;
using SecurityIntegration.Exceptions;

namespace IdentityDemoApi.Middlewares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext httpContext, ILoggerFactory loggerFactory)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ExceptionHandler>();

            if (ex is AppAuthenticationException authenticationException)
            {
                await WriteResponse(authenticationException.StatusCode, authenticationException.Message, httpContext);
            }
            else
            {
                const string message = "Unexpected exception";
                logger.LogError(ex, message);
                await WriteResponse(500, message, httpContext);
            }
        }
    }
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static async Task WriteResponse(int status, string message, HttpContext httpContext)
    {
        if (!httpContext.Response.HasStarted)
        {
            httpContext.Response.Clear();
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = status;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                Success = false, 
                Message = message
            }, JsonSerializerOptions));
        }
        else
            throw new Exception("Response stream has already started");
    }
}