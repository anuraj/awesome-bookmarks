using Microsoft.AspNetCore.Builder;

namespace bookmarks.Middlewares
{
    public static class EasyAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseEasyAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EasyAuthMiddleware>();
        }
    }
}