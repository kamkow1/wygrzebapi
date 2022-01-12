using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using wygrzebapi.Context;
using wygrzebapi.Models;

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

        [HttpPost]
        [Route("/create")]
        public IActionResult CreateNewArticle(string title, string content,
#nullable enable 
        Uri? thumbnail,
#nullable disable 
        int up, int down, int views, int userId) {
            try
            {
                /*_ctx.Articles.Add(new Article(title: title, 
                                              content: content, 
                                              thumbnail: thumbnail, 
                                              up: up, 
                                              down: down,  
                                              views: views, 
                                              userId: userId,
                                              creationDate: DateTime.UtcNow));*/
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
            catch (NotSupportedException)
            {
                return StatusCode(200);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    
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
