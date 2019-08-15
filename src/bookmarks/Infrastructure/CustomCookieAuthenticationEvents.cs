using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace bookmarks.Infrastructure
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override async Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Headers.ContainsKey("X-MS-CLIENT-PRINCIPAL-ID"))
            {
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
                var claims = new List<Claim>();
                foreach (var claim in obj[0]["user_claims"])
                {
                    claims.Add(new Claim(claim["typ"].ToString(), claim["val"].ToString()));
                }
                claims.Add(new Claim(ClaimTypes.Role, "TwitterUser"));
                var identity = new GenericIdentity(azureAppServicePrincipalNameHeader);
                identity.AddClaims(claims);
                context.HttpContext.User = new GenericPrincipal(identity, null);
            };

            await base.RedirectToReturnUrl(context);
        }
    }
}
