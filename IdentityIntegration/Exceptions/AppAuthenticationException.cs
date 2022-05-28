using Microsoft.AspNetCore.Http;

namespace SecurityIntegration.Exceptions;

public class AppAuthenticationException : Exception
{
    public int StatusCode { get; set; }
    public string Message { get; private set; }
    
    public AppAuthenticationException(string message, int statusCode = StatusCodes.Status401Unauthorized)
    {
        Message = message;
        StatusCode = statusCode;
    }
}