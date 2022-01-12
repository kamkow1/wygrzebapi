using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using wygrzebapi.Context;
using wygrzebapi.Models;
using Microsoft.AspNetCore.Cors;

namespace wygrzebapi.Controllers
{
    [ApiController]
    [Route("/api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public ArticleController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [EnableCors("Policy")]
        [HttpPost]
        [Route("/create")]
        public IActionResult CreateNewArticle(string title, string content, string thumbnail, int up, int down, int views, int userId) {
            try
            {
                _ctx.Articles.Add(new Article()
                {
                    Title = title,
                    Content = content,
                    Thumbail = thumbnail,
                    Upvotes = up,
                    Downvotes = down,
                    ViewCount = views,
                    UserId = userId,
                    CreationDate = DateTime.UtcNow
                });
                _ctx.SaveChanges();

                return StatusCode(201);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [EnableCors("Policy")]
        [HttpGet]
        [Route("/recent")]
        public IActionResult GetTenRecentArticles()
        {
            try
            {
                return Ok(new List<Article>(_ctx.Articles.Where(a => a.CreationDate < a.CreationDate.AddDays(-7))));
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
