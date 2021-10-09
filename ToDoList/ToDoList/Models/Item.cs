﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Task Name")]
        public string ItemName { get; set; }
        [DisplayName("Task Description")]
        public string ItemDescription { get; set; }
        [DisplayName("Resposible User")]
        public string ResponsibleUser { get; set; }
    }
}