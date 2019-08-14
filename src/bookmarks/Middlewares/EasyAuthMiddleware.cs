using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace bookmarks.Middlewares
{

    public class EasyAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public EasyAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("X-MS-CLIENT-PRINCIPAL-ID"))
            {
                var azureAppServicePrincipalIdHeader = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"][0];
                var azureAppServicePrincipalNameHeader = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"][0];
                var cookieContainer = new CookieContainer();
                var handler = new HttpClientHandler()
                {
                    CookieContainer = cookieContainer
                };
                var uriString = $"{context.Request.Scheme}://{context.Request.Host}";
                foreach (var cookie in context.Request.Cookies)
                {
                    cookieContainer.Add(new Uri(uriString), new Cookie(cookie.Key, cookie.Value));
                }
                var jsonResult = string.Empty;
                using (var client = new HttpClient(handler))
                {
                    var res = await client.GetAsync($"{uriString}/.auth/me");
                    jsonResult = await res.Content.ReadAsStringAsync();
                }
                var obj = JArray.Parse(jsonResult);
                var user_id = obj[0]["user_id"].Value<string>();
                var claims = new List<Claim>();
                foreach (var claim in obj[0]["user_claims"])
                {
                    claims.Add(new Claim(claim["typ"].ToString(), claim["val"].ToString()));
                }
                var identity = new GenericIdentity(azureAppServicePrincipalNameHeader);
                identity.AddClaims(claims);
                context.User = new GenericPrincipal(identity, null);
            };

            await _next(context);
        }
    }
}