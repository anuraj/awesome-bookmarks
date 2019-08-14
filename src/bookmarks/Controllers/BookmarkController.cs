using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookmarks.Models;
using bookmarks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bookmarks.Controllers
{
    public class BookmarkController : Controller
    {
        private readonly BookmarksDbContext _bookmarksDbContext;
        private readonly IUserService _userService;
        public BookmarkController(BookmarksDbContext bookmarksDbContext, IUserService userService)
        {
            _bookmarksDbContext = bookmarksDbContext;
            _userService = userService;
        }
        public async Task<List<Bookmark>> GetBookmarksAsync()
        {
            return await _bookmarksDbContext.Bookmarks
                .Where(bookmark => bookmark.User == _userService.User).ToListAsync();
        }

        [HttpPost]
        public ActionResult AddBookmark(Bookmark bookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            bookmark.User = _userService.User;
            _bookmarksDbContext.Bookmarks.Add(bookmark);
            return Ok();
        }
    }
}
