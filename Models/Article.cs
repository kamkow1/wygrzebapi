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


#nullable enable
        public Uri? Thumbail { get; set; }
#nullable restore

        public int Upvotes { get; set; }
        
        public int Downvotes { get; set; }

        public int ViewCount { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
/*
        [JsonConstructor]
        public Article(string title, 
                       string content, 
#nullable enable
                       Uri? thumbnail, 
#nullable restore
                       int up, 
                       int down, 
                       int views, 
                       int userId,
                       DateTime creationDate) {
            this.Title = title;
            this.Content = content;
            this.Thumbail = thumbnail;
            this.Upvotes = up;
            this.Downvotes = down;
            this.ViewCount = views;
            this.UserId = userId;
            this.CreationDate = creationDate;
        }

        public Article()
        {

        }*/
    }
}
