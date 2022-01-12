using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using wygrzebapi.Context;
using wygrzebapi.Email;
using wygrzebapi.Models;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;

namespace wygrzebapi.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly IEmailService _emailService;

        public UserController(AppDbContext ctx, IEmailService es)
        {
            _ctx = ctx;
            _emailService = es;
        }

        [EnableCors("Policy")]
        [HttpPost]
        [Route("/register")]
        public IActionResult Register(string login, string password, string bio, int age, string country, string email)
        {
            try
            {
                if (_ctx.Users.Where(u => u.Login == login).FirstOrDefault() != null)
                    return StatusCode(409);

                _ctx.Users.Add(new User()
                {
                    Login = login,
                    Password = password,
                    Bio = bio,
                    Age = age,
                    Country = country,
                    Email = email,
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
                                    .Where(u => u.Login == login)
                                    .Select(u => u.Email)
                                    .SingleOrDefault(),
                                    $"Witaj użytkowniku {_ctx.Users.Where(u => u.Login == login).Select(u => u.Login).SingleOrDefault()}!",
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

        [EnableCors("Policy")]
        [HttpPost]
        [Route("/login")]
        public IActionResult Login(string login, string password)
        {
            try
            {
                if (login.Trim().Length == 0 || password.Trim().Length == 0)
                {
                    return StatusCode(400);
                }

                var user = _ctx.Users.Where(u => u.Login == login || u.Password == password).FirstOrDefault();

                if (user == null)
                {
                    return StatusCode(404);
                }

                user.RemoteIpAdress = HttpContext.Connection.RemoteIpAddress.ToString();
                _ctx.Users.Update(user);
                _ctx.SaveChanges();

                return Ok(new
                {
                    id = user.Id,
                    ip = user.RemoteIpAdress
                });
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [EnableCors("Policy")]
        [HttpGet]
        [Route("/getbyid")]
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
                                u.Country,
                                u.Age,
                                u.CreationDate
                            }));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [EnableCors("Policy")]
        [HttpGet]
        [Route("/articles")]
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

        [EnableCors("Policy")]
        [HttpGet]
        [Route("/history")]
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
