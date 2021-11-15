using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModel
{
    public class AddMember
    {
        [Required, EmailAddress(ErrorMessage = "Enter user email address")]
        [DisplayName("User E-Mail address")]
        public string Email { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
    }
}
