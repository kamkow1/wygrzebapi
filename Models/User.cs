using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace wygrzebapi.Models
{
    [Index(nameof(Login), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [MinLength(1)]
        public string Login { get; set; }

        [MaxLength(32)]
        [MinLength(5)]
        public string Password { get; set; }

        public User(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
