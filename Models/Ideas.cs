using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace belt2.Models
{
    public class Idea : BaseEntity
    {
        public Idea(){
            users = new List<User>();
            num_likes = 0;
            likers = "";
        }

        [Key]
        public int id {get; set;}
        [Required]
        [MinLength(5)]
        public string description {get; set;}
        [Required]
        public string poster {get; set;}
        public int poster_id {get; set;}
        public int num_likes {get; set;}
        public string likers {get; set;}
        public ICollection<User> users {get; set;}

    }
}