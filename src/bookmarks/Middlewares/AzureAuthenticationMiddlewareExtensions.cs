using Microsoft.AspNetCore.Builder;

namespace bookmarks.Middlewares
{
    public static class AzureAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseEasyAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AzureAuthenticationMiddleware>();
        }
    }
}