using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using wygrzebapi.Context;
using wygrzebapi.Models;

namespace wygrzebapi.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public UserController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(string login, string password)
        {
            try
            {
                if (login.Trim().Length == 0 || password.Trim().Length == 0)
                {
                    return StatusCode(400);
                }

                if (_ctx.Users.Where(u => u.Login == login || u.Password == password).FirstOrDefault() != null)
                {
                    return StatusCode(409);
                }

                _ctx.Users.Add(new(login, password));
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
