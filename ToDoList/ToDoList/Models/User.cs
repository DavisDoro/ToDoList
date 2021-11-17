using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Data;

namespace ToDoList.Models
{
    public class User : DateBase
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public virtual ICollection<Item> Items { get; set; }

    }

 
}
