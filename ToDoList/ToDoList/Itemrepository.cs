using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList
{
    public class Itemrepository
    {
        private ApplicationDbContext db;

        public Itemrepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public DbSet<Item> Items { get; set; }

        public List<Item> GetItems()
        {
            IEnumerable<Item> objList = db.Items;
            var sortedObjList = from obj in objList
                                orderby obj.DeadlineDate
                                select obj;

            return sortedObjList.ToList();
        }
    }
}
