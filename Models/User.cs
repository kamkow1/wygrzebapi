using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace wygrzebapi.Models
{
    [Index(nameof(Login), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20, ErrorMessage = "max is 20 characters!")]
        [MinLength(1, ErrorMessage = "min is 1 characters!")]
        public string Login { get; set; }

        [MaxLength(32, ErrorMessage = "max is 32 characters!")]
        [MinLength(5, ErrorMessage = "min is 5 characters!")]
        public string Password { get; set; }

        public DateTime CreationDate { get; } = DateTime.UtcNow;

        public int Age { get; set; }

        public string Country { get; set; } = "Polska";

        public string Bio { get; set; }

        public User(string login, string password, string bio)
        {
            this.Login = login;
            this.Password = password;

            if (bio.Trim().Length == 0)
            {
                this.Bio = $"Hej, jestem {login}";
            }
            else
            {
                this.Bio = bio;
            }
        }
    }
}
