using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class CompletedTaskViewModel
    {
        public List<CompletedTask> Items { get; set; }
        public List<Group> Groups { get; set; }
    }
}
