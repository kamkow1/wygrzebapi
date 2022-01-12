using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [EnableCors("Policy")]
        [HttpPost]
        [Route("/new")]
        public IActionResult CreateNew(string query, int userId)
        {
            try
            {
                if (query.Trim().Length == 0 || userId.ToString().Trim().Length == 0)
                    return StatusCode(422);

                _ctx.Searches.Add(new Search() { 
                    Query = query,
                    UserId = userId,
                    TimeStamp = DateTime.UtcNow
                });
                _ctx.SaveChanges();

                return StatusCode(200);
            }
            catch (NotSupportedException)
            {
                return StatusCode(200);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
