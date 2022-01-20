using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wygrzebapi.Models
{
    [Serializable]
    public class Search
    {
        [Key]
        public int Id { get; set; }

        public string Query { get; set; }

        public DateTime TimeStamp { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
