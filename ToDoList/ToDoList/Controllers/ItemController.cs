using ToDoList.Data;
using ToDoList.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ItemController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Item> objList = _db.Items;
            var sortedObjList = from obj in objList
                                orderby obj.DeadlineDate
                                select obj;
            return View(sortedObjList);

        }

        //GET-create
        public IActionResult Create()
        {
            return View();
        }

        //Post-Create method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item obj)
        {
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


    }
}
