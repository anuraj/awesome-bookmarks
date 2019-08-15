using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookmarks.Models;
using bookmarks.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bookmarks.Controllers
{
    [Authorize]
    public class BookmarkController : Controller
    {
        private readonly BookmarksDbContext _bookmarksDbContext;
        private readonly IUserService _userService;
        public BookmarkController(BookmarksDbContext bookmarksDbContext, IUserService userService)
        {
            _bookmarksDbContext = bookmarksDbContext;
            _userService = userService;
        }

        public ActionResult Index()
        {
            return Ok();
        }
    }
}
