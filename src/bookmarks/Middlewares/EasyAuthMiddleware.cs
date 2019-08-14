using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using bookmarks.Models;
using bookmarks.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace bookmarks.Middlewares
{

    public class EasyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BookmarksDbContext _bookmarkDbContext;
        public EasyAuthMiddleware(RequestDelegate next,
            BookmarksDbContext bookmarkDbContext)
        {
            _next = next;
            _bookmarkDbContext = bookmarkDbContext;
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
                    var httpResponseMessage = await client.GetAsync($"{uriString}/.auth/me");
                    jsonResult = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                var claimsArray = JArray.Parse(jsonResult);
                var user_id = claimsArray[0]["user_id"].Value<string>();
                var claims = new List<Claim>();
                foreach (var claim in claimsArray[0]["user_claims"])
                {
                    claims.Add(new Claim(claim["typ"].ToString(), claim["val"].ToString()));
                }
                var identity = new GenericIdentity(azureAppServicePrincipalNameHeader);
                identity.AddClaims(claims);
                context.User = new GenericPrincipal(identity, null);
                if (!_bookmarkDbContext.Users.Any(user => user.ProviderId == user_id))
                {
                    var user = new User() { Name = context.User.Identity.Name, ProviderId = user_id };
                    _bookmarkDbContext.Users.Add(user);
                    await _bookmarkDbContext.SaveChangesAsync();
                }
            };

            await _next(context);
        }
    }
}