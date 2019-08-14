using System.Linq;
using bookmarks.Models;
using bookmarks.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookmarks.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly BookmarksDbContext _bookmarksDbContext;
        private readonly IUserService _userService;
        public AccountController(BookmarksDbContext bookmarksDbContext, IUserService userService)
        {
            _bookmarksDbContext = bookmarksDbContext;
            _userService = userService;
        }
        public IActionResult Register()
        {
            var isUserActivated = _bookmarksDbContext.Users.Any(user =>
                user.ProviderId == _userService.Id && user.IsActivated);
            if (isUserActivated)
            {
                return View();
            }
            
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong. Please retry.");
                return View();
            }

            return View();
        }
    }
}
