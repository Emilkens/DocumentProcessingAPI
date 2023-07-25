using DocumentProcessingAPI.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace DocumentProcessingAPI.Middleware;

/// <summary>
/// Provides Basic Auth capabilities to the request pipeline
/// </summary>
public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BasicAuthMiddleware> _logger;
    private readonly BasicAuthOptions _authenticationOptions;

    public BasicAuthMiddleware(RequestDelegate next, ILogger<BasicAuthMiddleware> logger, IOptions<BasicAuthOptions> options)
    {
        _next = next;
        _logger = logger;
        _authenticationOptions = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string authHeader = context.Request.Headers["Authorization"];

        if (authHeader == null || !authHeader.StartsWith("Basic "))
        {
            FailAuthentication(context);
            return;
        }

        string username, password;
        try
        {
            string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            byte[] decodedBytes = Convert.FromBase64String(encodedCredentials);
            string credentials = Encoding.UTF8.GetString(decodedBytes);

            string[] parts = credentials.Split(':', 2);
            username = parts[0];
            password = parts[1];
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception thrown while decoding credentials\nException Message: {ex.Message} ");
            context.Response.StatusCode = 401;

            SetBodyMessage(context, "Credentials could not be read. Verify if the base64 value is valid.");
            return;
        }

        if (!IsValidUser(username, password))
        {
            FailAuthentication(context);
            return;
        }

        await _next(context);
    }

    private bool IsValidUser(string username, string password)
    {
        return username == _authenticationOptions.Login && password == _authenticationOptions.Password;
    }

    private void FailAuthentication(HttpContext context)
    {
        context.Response.Headers["WWW-Authenticate"] = "Basic";
        context.Response.StatusCode = 401;
    }

    private async void SetBodyMessage(HttpContext context, string message)
    {
        byte[] responseBodyBytes = Encoding.UTF8.GetBytes(message);

        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.ContentLength = responseBodyBytes.Length;

        await context.Response.Body.WriteAsync(responseBodyBytes, 0, responseBodyBytes.Length);
    }
}
