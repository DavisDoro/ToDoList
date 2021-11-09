using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Group : DateBase
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Group Name")]
        public string Name { get; set; }

        [DisplayName("Group Description")]
        public string Description { get; set; }

        [DisplayName("Group Owner")]
        public string Owner { get; set; }
    }
}
