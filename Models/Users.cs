using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace belt2.Models
{
    public abstract class BaseEntity{}
    public class User : BaseEntity
    {   
         public User() {
             ideas = new List<Idea>();
         }
        [Key]
        public int id {get; set;}
        
        [Required]
        [MinLength(2)]
        [RegularExpression((@"^[a-zA-Z]+$"), ErrorMessage="first name must be letters only. No numbers.")]
        public string first_name { get; set; }
        [Required]
        [MinLength(2)]
        [RegularExpression((@"^[a-zA-Z]+$"), ErrorMessage="last name must be letters only. No numbers.")]
        public string last_name { get; set; }
        [Required]
        [EmailAddress]        
        public string email { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [RegularExpression((@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,10}"), ErrorMessage = "password must contain a lowercase letter, uppercase letter, number and special character")]
        public string password { get; set; }
        [Required]
        [Compare(nameof(password), ErrorMessage = "password does not  match")]
        public string confirmPassword { get ; set; }

        public int user_id {get; set;}
        public int num_likes {get; set;}
        public int num_posts {get; set;}
        public ICollection<Idea> ideas {get; set;}
    }
}
   