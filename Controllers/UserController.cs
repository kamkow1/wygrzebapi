using Microsoft.AspNetCore.Mvc;
using wygrzebapi.Models;
using System.Linq;
using wygrzebapi.Context;

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
        public IActionResult Register(string login, string password, string bio, int age, string country)
        {

            // check if user with provided login already exists
            if (_ctx.Users.Where(u => u.Login == login).FirstOrDefault() != null) return StatusCode(409);

            _ctx.Users.Add(new(login: login,
                               password: password,
                               bio: bio,
                               age: age,
                               country: country));

            _ctx.SaveChanges();

            return StatusCode(200);
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(string login, string password)
        {
            if (login.Trim().Length == 0 || password.Trim().Length == 0)
            {
                return StatusCode(400);
            }

            if (_ctx.Users.Where(x => x.Login == login || x.Password == password) == null)
            {
                return StatusCode(404);
            }

            return StatusCode(200);
        }
    }
}
