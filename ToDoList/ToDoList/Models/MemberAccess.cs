using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class MemberAccess :DateBase
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int GroupId { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
    }
}
