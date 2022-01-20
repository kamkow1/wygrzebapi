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
        public IActionResult CreateNew(Search newSearch)
        {
            if (newSearch.Query.Trim().Length == 0 || newSearch.UserId.ToString().Trim().Length == 0)
                return StatusCode(422);

            

            Search search = new()
            {
                Query = newSearch.Query,
                UserId = newSearch.UserId,
                TimeStamp = DateTime.UtcNow
            };

            _ctx.Searches.Add(search);
            _ctx.SaveChanges();

            return Ok(_ctx.Articles.Where(a => a.Title.Contains(newSearch.Query) || a.Content.Contains(newSearch.Query) || a.User.Login.Contains(newSearch.Query)));
        }
    }
}
