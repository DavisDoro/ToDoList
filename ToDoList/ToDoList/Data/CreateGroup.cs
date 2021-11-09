using System.ComponentModel.DataAnnotations;

namespace ToDoList.Data
{
    public class CreateGroup
    {
        [Required, StringLength(24, ErrorMessage = "Group Name is too long. Max length is 24 characters")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
