using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        public string Email { get; set; }

        public DateTime CreationDate { get; set; }

        public string RemoteIpAdress { get; set; }

        public virtual ICollection<Search> Searches { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
