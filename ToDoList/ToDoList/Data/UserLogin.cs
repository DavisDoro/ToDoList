using System.ComponentModel.DataAnnotations;

namespace ToDoList.Data
{
    public class UserLogin
    {
        [Required(ErrorMessage= "Please enter your email address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
