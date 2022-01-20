using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using wygrzebapi.Context;
using wygrzebapi.Models;

namespace wygrzebapi.Controllers
{
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public ArticleController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost]
        [Route("/article/create")]
        public IActionResult CreateNewArticle(string title, string content, string thumbnail, int up, int down, int views, int userId)
        {
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

        [HttpGet]
        [Route("/article/recent")]
        public IActionResult GetRecentArticles()
        {
            try
            {
                return Ok(_ctx.Articles.Where(a => a.CreationDate < a.CreationDate.AddDays(7)).OrderByDescending(a => a.CreationDate).ToList());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("/article/upvote")]
        public IActionResult Upvote([FromBody]Article a)
        {
            try
            {
                var article = _ctx.Articles.Find(a.Id);
                article.Upvotes++;
                _ctx.Articles.Update(article);
                _ctx.SaveChanges();                

                return StatusCode(200);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("/article/downvote")]
        public IActionResult Downvote([FromBody] Article a)
        {
            try
            {
                var article = _ctx.Articles.Find(a.Id);
                article.Downvotes++;
                _ctx.Articles.Update(article);
                _ctx.SaveChanges();

                return StatusCode(200);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }
    }
}
