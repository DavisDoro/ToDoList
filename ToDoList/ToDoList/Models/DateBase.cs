using System;

namespace ToDoList.Models
{
    public abstract class DateBase
    {
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public DateTime LastEdited { get; set; }
    }
}
