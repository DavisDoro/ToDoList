using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class GroupContent : DateBase
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int ItemId { get; set; }
    }
}
