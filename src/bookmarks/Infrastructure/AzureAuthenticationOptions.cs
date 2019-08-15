using Microsoft.AspNetCore.Authentication;

namespace bookmarks.Infrastructure
{
    public class AzureAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string RedirectUri { get; set; }
        public string Provider { get; set; }
    }
}