using System.ComponentModel.DataAnnotations;

namespace wygrzebapi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }        

        public string Login { get; set; }

        public string Password { get; set; }

        public User(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
