using Microsoft.AspNetCore.Mvc;
using wygrzebapi.Context;
using wygrzebapi.Models;

namespace wygrzebapi.Controllers
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public SearchController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost]
        [Route("/new")]
        public IActionResult CreateNew(string query, int userId)
        {
            if (query.Trim().Length == 0
                || userId.ToString().Trim().Length == 0)
                return StatusCode(422);
            // Request.HttpContext.Connection.RemoteIpAddress.ToString()
            Search search = new(query: query, 
                                userId: userId);

            _ctx.Searches.Add(search);
            _ctx.SaveChanges();

            return StatusCode(200);
        }
    }
}
