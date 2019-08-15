using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace bookmarks.Infrastructure
{
    public class AzureAuthenticationHandler : AuthenticationHandler<AzureAuthenticationOptions>
    {
        public AzureAuthenticationHandler(IOptionsMonitor<AzureAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var uriString = $"{Context.Request.Scheme}://{Context.Request.Host}/.auth/login/twitter?post_login_redirect_url=/";
            Context.Response.Redirect(uriString);
            await Task.CompletedTask;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var easyAuthEnabled = bool.Parse(Environment.GetEnvironmentVariable("EASY_AUTH_ENABLED") ?? "False");
                if (!easyAuthEnabled)
                {
                    return AuthenticateResult.NoResult();
                }

                var azureAppServicePrincipalIdHeader = Context.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"][0];
                var azureAppServicePrincipalNameHeader = Context.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"][0];
                if (String.IsNullOrWhiteSpace(azureAppServicePrincipalIdHeader))
                {
                    return AuthenticateResult.NoResult();
                }

                var uriString = $"{Context.Request.Scheme}://{Context.Request.Host}";
                var jsonResult = string.Empty;
                var handler = new HttpClientHandler();
                using (var client = new HttpClient(handler))
                {
                    var httpResponseMessage = await client.GetAsync($"{uriString}/.auth/me");
                    jsonResult = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                var claimsArray = JArray.Parse(jsonResult);
                var claims = new List<Claim>();
                foreach (var claim in claimsArray[0]["user_claims"])
                {
                    claims.Add(new Claim(claim["typ"].ToString(), claim["val"].ToString()));
                }
                var identity = new GenericIdentity(azureAppServicePrincipalNameHeader);
                identity.AddClaims(claims);
                var principal = new ClaimsPrincipal();
                principal.AddIdentity(identity);
                Context.User = new GenericPrincipal(identity, null);
                var authenticationTicket = new AuthenticationTicket(principal, "AzureEasyAuthentication");
                return AuthenticateResult.Success(authenticationTicket);
            }
            catch (Exception exception)
            {
                return AuthenticateResult.Fail(exception);
            }
        }
    }
}