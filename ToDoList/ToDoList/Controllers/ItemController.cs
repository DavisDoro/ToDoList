
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoList.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ToDoList.Models;


namespace ToDoList.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ItemController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Authorize]
        public IActionResult Index()
        {


            Itemrepository ItemrepositoryNew = new Itemrepository(_db);
            ;

            return View(ItemrepositoryNew.GetItems());

        }

        //GET-create
        [Authorize]
        public IActionResult Create()
        {
            IEnumerable<User> userview = _db.Users;
            List<string> userlist = new List<string> { };

            foreach (var username in userview)
            {
                userlist.Add(username.Username);
            }
            ViewBag.Users = new SelectList(userlist);
            return View();
        }
        [HttpPost]
        public ActionResult Drop(FormCollection form)
        {
            var optionsValue = form["Options"];

            return RedirectToAction("Drop");
        }


        //Post-Create method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item obj)
        {
            IEnumerable<User> objList = _db.Users;
            if(obj.DeadlineDate < DateTime.Now)
            {
                return RedirectToAction("ErrorDate");
            }  

            obj.UserId = (from user in objList
                          where obj.ResponsibleUser == user.Username
                          select user.Id).FirstOrDefault();


            _db.Items.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }


        // GET Delete
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Items.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        //POST Delete
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Items.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Items.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET Edit
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Items.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            IEnumerable<User> userview = _db.Users;
            List<string> userlist = new List<string> { };

            foreach (var username in userview)
            {
                userlist.Add(username.Username);
            }
            ViewBag.Users = new SelectList(userlist);
            return View(obj);
        }

        //Post-Create method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item obj)
        {
            IEnumerable<User> objList = _db.Users;
            if (obj.DeadlineDate < DateTime.Now)
            {
                return RedirectToAction("ErrorDate");
            }
            obj.UserId = (from user in objList
                          where obj.ResponsibleUser == user.Username
                          select user.Id).FirstOrDefault();
            _db.Items.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ErrorDate()
        {
            return View();

        }
    }
}
