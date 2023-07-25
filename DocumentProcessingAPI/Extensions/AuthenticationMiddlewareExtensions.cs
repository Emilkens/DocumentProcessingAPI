using DocumentProcessingAPI.Configuration;
using DocumentProcessingAPI.Middleware;
using Microsoft.Extensions.Options;

namespace DocumentProcessingAPI.Extensions;
public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BasicAuthMiddleware>();
    }

    public static IServiceCollection AddBasicAuth(this IServiceCollection services)
    {
        BasicAuthOptions options = new()
        {
            Password = Environment.GetEnvironmentVariable("DOCUMENT_PROCESSING_API_PASSWORD") ?? throw new Exception("No api password provided."),
            Login = Environment.GetEnvironmentVariable("DOCUMENT_PROCESSING_API_LOGIN") ?? throw new Exception("No api login provided.")
        };

        services.AddSingleton(Options.Create(options));
        return services;
    }
}