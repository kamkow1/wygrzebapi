using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wygrzebapi.Models
{
    [Serializable]
    public class Article
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }


        public string Thumbail { get; set; }

        public int Upvotes { get; set; }
        
        public int Downvotes { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
