using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class GroupAndItemModel
    {
        public List<Item> Items { get; set; }
        public List<Group> Groups { get; set; }
    }
}
