using System.Linq;
using System.Security.Claims;
using bookmarks.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace bookmarks.Services
{
    public class UserService : IUserService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BookmarksDbContext _bookmarksDbContext;
        public UserService(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, BookmarksDbContext bookmarksDbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _bookmarksDbContext = bookmarksDbContext;
        }
        public string Name => _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        public string Id => _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }
}