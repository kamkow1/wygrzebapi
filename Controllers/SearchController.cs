using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using wygrzebapi.Context;
using wygrzebapi.Models;

namespace wygrzebapi.Controllers
{
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public SearchController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost]
        [Route("/search/new")]
        public IActionResult CreateNew(string query, int userId)
        {
            if (query.Trim().Length == 0 || userId.ToString().Trim().Length == 0)
                return StatusCode(422);

            Search search = new()
            {
                Query = query,
                UserId = userId,
                TimeStamp = DateTime.UtcNow
            };

            _ctx.Searches.Add(search);
            _ctx.SaveChanges();

            return Ok(_ctx.Articles.Where(a => a.Title.Contains(query) || a.Content.Contains(query) || a.User.Login.Contains(query)));
        }
    }
}
