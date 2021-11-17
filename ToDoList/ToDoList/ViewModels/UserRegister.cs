using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(24, ErrorMessage = "Your username is too long. Max 16 char.")]
        public string Username { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Minimum password length is 5 characters long")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }
    }
}