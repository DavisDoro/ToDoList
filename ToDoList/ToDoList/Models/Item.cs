using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public enum Priority
    {
        Low = 1,
        Normal = 2,
        High = 3
    }

    public class Item : DateBase
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Task Name")]
        public string ItemName { get; set; }
        [DisplayName("Task Description")]
        public string ItemDescription { get; set; }
        [DisplayName("Resposible User")]
        public string ResponsibleUser { get; set; }

        public DateTime DeadlineDate { get; set; }
        public Priority Priority { get; set; }
    }
}
