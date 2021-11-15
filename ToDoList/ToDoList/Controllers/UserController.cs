using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers

{
    public class UserController : Controller
    {

        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext db)
        {
            this.db = db;
        }
       
        [Authorize]
        public IActionResult Index()
        {
            IEnumerable<User> objList = db.Users;
            return View(objList);
        }




        // GET Delete
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = db.Users.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        //POST Delete
        public IActionResult DeletePOST(int? id)
        {
            var obj = db.Users.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            db.Users.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET Edit
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = db.Users.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post-Editmethod
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User obj)
        {
            int userId = obj.Id;
            string userName = obj.Username;


            IEnumerable<Item> itemList = db.Items;

            foreach (var item in itemList)
            {
                if (item.UserId == userId)
                {
                    item.ResponsibleUser = userName;
                }
            }
            db.Users.Update(obj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        


    }
}
