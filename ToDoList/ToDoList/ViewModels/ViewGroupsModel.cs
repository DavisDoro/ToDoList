using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class ViewGroupsModel
    {
        public List<MemberAccess> Accesses { get; set; }
        public List<Group> Groups { get; set; }

    }
}
