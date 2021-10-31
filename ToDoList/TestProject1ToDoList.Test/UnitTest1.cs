using System;
using Microsoft.EntityFrameworkCore;
using ToDoList;
using ToDoList.Data;
using Xunit;

namespace TestProject1ToDoList.Test
{

    public class ItemTest
    {
        private readonly ApplicationDbContext _db;
        public ItemTest(ApplicationDbContext db)
        {
         _db = db;
        }

        [Fact]


        public void GetRecords_WhenDateBAse__ThenReturn_()
        {

            Itemrepository ItemrepositoryNew = new Itemrepository(_db);
           
           


            // Assert.True(IsErrorNull);
        }

    } 
    
}
