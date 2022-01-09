using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using wygrzebapi.Context;
using wygrzebapi.Email;

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

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(string login, string password, string bio, int age, string country, string email)
        {
            try
            {
                if (_ctx.Users.Where(u => u.Login == login).FirstOrDefault() != null) return StatusCode(409);

                _ctx.Users.Add(new(login: login,
                                   password: password,
                                   bio: bio,
                                   age: age,
                                   country: country,
                                   email: email,
                                   remoteIp: Request.HttpContext.Connection.RemoteIpAddress.ToString()));
                // Request.HttpContext.Connection.RemoteIpAddress.ToString()
                _ctx.SaveChanges();

                return StatusCode(200);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

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

                if (_ctx.Users.Where(u => u.Login == login || u.Password == password) == null)
                {
                    return StatusCode(404);
                }

                if (Request.HttpContext.Connection.RemoteIpAddress.ToString() != _ctx.Users
                                                                                    .Where(u => u.Login == login)
                                                                                    .Select(u => u.CurrentRemoteIpAdress)
                                                                                    .SingleOrDefault())
                {
                    string body = string.Empty;
                    using (StreamReader reader = new(Path.Combine("Email/Templates", "emailTemplate.html")))
                    {
                        body = reader.ReadToEnd();
                    }

                    _emailService.Send(_ctx.Users
                                        .Where(u => u.Login == login)
                                        .Select(u => u.Email)
                                        .SingleOrDefault(),
                                        "Ktoś próbował wejść na twoje konto!",
                                        "servicewygrzeb@gmail.com",
                                        "wygrzeb2022",
                                        body);

                    _ctx.Users.Where(u => u.Login == login).SingleOrDefault().CurrentRemoteIpAdress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    _ctx.Users.Update(_ctx.Users.Where(u => u.Login == login).SingleOrDefault());
                    _ctx.SaveChanges();
                }

                return StatusCode(200);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/history")]
        public IActionResult GetSearchHistory(int id)
        {
            try
            {
                return Ok(_ctx.Users
                            .Where(u => u.Id == id)
                            .Select(u => u.Searches)
                            .SingleOrDefault()
                            .OrderByDescending(s => s.TimeStamp));
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
