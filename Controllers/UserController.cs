using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using wygrzebapi.Context;
using wygrzebapi.Email;
using wygrzebapi.Models;
using System.Collections.Generic;

namespace wygrzebapi.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly IEmailService _emailService;

        public UserController(AppDbContext ctx, IEmailService es)
        {
            _ctx = ctx;
            _emailService = es;
        }

        [HttpPost]
        [Route("/user/register")]
        public IActionResult Register(User user)
        {
            try
            {
                if (_ctx.Users.Where(u => u.Login == user.Login).FirstOrDefault() != null)
                    return StatusCode(409);

                _ctx.Users.Add(new User()
                {
                    Login = user.Login,
                    Password = user.Password,
                    Email = user.Email,
                    RemoteIpAdress = HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreationDate = DateTime.UtcNow
                });

                _ctx.SaveChanges();

                string body = string.Empty;
                using (StreamReader reader = new(Path.Combine("Email/Templates", "WelcomeEmailTemplate.html")))
                {
                    body = reader.ReadToEnd();
                }

                _emailService.Send(_ctx.Users
                                    .Where(u => u.Login == user.Login)
                                    .Select(u => u.Email)
                                    .SingleOrDefault(),
                                    $"Witaj użytkowniku {_ctx.Users.Where(u => u.Login == user.Login).Select(u => u.Login).SingleOrDefault()}!",
                                    "servicewygrzeb@gmail.com",
                                    "wygrzeb2022",
                                    body);

                return StatusCode(200);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("/user/login")]
        public IActionResult Login(User user)
        {
            try
            {

                var userFromDb = _ctx.Users.Where(u => u.Login == user.Login || u.Password == user.Password).FirstOrDefault();

                if (userFromDb == null)
                {
                    return StatusCode(404);
                }

                userFromDb.RemoteIpAdress = HttpContext.Connection.RemoteIpAddress.ToString();
                _ctx.Users.Update(userFromDb);
                _ctx.SaveChanges();

                return Ok(new
                {
                    id = userFromDb.Id,
                    ip = userFromDb.RemoteIpAdress
                });
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/user/getbyid")]
        public IActionResult GetUser(int id)
        {
            try
            {
                return Ok(_ctx.Users
                            .Where(u => u.Id == id)
                            .Select(u => new { 
                                u.Login,
                                u.Password,
                                u.Email,
                                u.Bio,
                                u.CreationDate
                            }));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/user/articles")]
        public IActionResult GetArticlesByUserId(int id)
        {
            try
            {
                return Ok(_ctx.Users
                                .Where(u => u.Id == id)
                                .Select(u => u.Articles)
                                .SingleOrDefault());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/user/history")]
        public IActionResult GetSearchHistory(int id)
        {
            try
            {
                return Ok(new List<Search>(_ctx.Users
                            .Where(u => u.Id == id)
                            .Select(u => u.Searches)
                            .SingleOrDefault()
                            .OrderByDescending(s => s.TimeStamp)));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
