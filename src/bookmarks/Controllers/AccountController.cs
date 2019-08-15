using System.Linq;
using System.Threading.Tasks;
using bookmarks.Models;
using bookmarks.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookmarks.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookmarksDbContext _bookmarksDbContext;
        private readonly IUserService _userService;
        public AccountController(BookmarksDbContext bookmarksDbContext, IUserService userService)
        {
            _bookmarksDbContext = bookmarksDbContext;
            _userService = userService;
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Register()
        {
            //var isUserActivated = _bookmarksDbContext.Users.Any(user =>
            //    user.ProviderId == _userService.Id && user.IsActivated);
            //if (isUserActivated)
            //{
            //    return View();
            //}

            //return Redirect("/");
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "Something went wrong. Please retry.");
            //    return View();
            //}

            return View();
        }
    }
}
