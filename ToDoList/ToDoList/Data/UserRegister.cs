using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [StringLength(16, ErrorMessage = "Your username is too long. Max 16 char.")]
        public string Username { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }
    }
}
