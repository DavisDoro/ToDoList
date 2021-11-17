using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class CompletedTask : DateBase
    {

        [Key]
        public int Id { get; set; }
        [DisplayName("Task Name")]
        public string ItemName { get; set; }
        [DisplayName("Task Description")]
        public string ItemDescription { get; set; }
        [DisplayName("Resposible User")]
        public string ResponsibleUser { get; set; }
        public int GroupId { get; set; }
        public DateTime DeadlineDate { get; set; }
        public Priority Priority { get; set; }
        public int? UserId { get; set; }
        public DateTime Finished { get; set; }

    }
}
