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
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ItemController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            Itemrepository ItemrepositoryNew = new Itemrepository(_db);
            return View(ItemrepositoryNew.GetItems());
        }

        //GET-create
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
        [HttpGet]
        public IActionResult ViewHistory(GroupAndItemModel model)
        {
            int groupId = model.Groups[0].Id;

            List<CompletedTask> completedItems = _db.CompletedTasks.Where(t => t.GroupId == groupId).ToList();
            List<Group> thisGroup = _db.Groups.Where(g => g.Id == groupId).ToList();

            CompletedTaskViewModel viewModel = new CompletedTaskViewModel()
            {
                Groups = thisGroup,
                Items = completedItems
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateGroupItem(GroupAndItemModel group)
        {
            int groupId = group.Groups[0].Id;
            List<string> groupUserEmail = new List<string> { };
            List<string> userList = new List<string> { };

            List<MemberAccess> memberAccess = _db.Accesses.Where(u => u.GroupId == groupId).ToList();

            foreach (var user in memberAccess)
            {
                    groupUserEmail.Add(user.Email);
            }
            foreach (var email in groupUserEmail)
            {
                List<User> users = _db.Users.Where(u => u.Email == email).ToList();
                userList.Add(users[0].Username);
            }

            Item viewItem = new Item();
            viewItem.GroupId = groupId;

            ViewBag.Users = new SelectList(userList);

            return View(viewItem);
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

            int groupId = obj.GroupId;

            return RedirectToAction("ViewGroup", "Group", new { id = groupId });
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
            int groupId = obj.GroupId;

            var completedTask = new CompletedTask()
            {
                ItemName = obj.ItemName,
                ItemDescription = obj.ItemDescription,
                ResponsibleUser = obj.ResponsibleUser,
                GroupId = obj.GroupId,
                DeadlineDate = obj.DeadlineDate,
                Finished = DateTime.Now
            };
            _db.CompletedTasks.Add(completedTask);
            _db.Items.Remove(obj);
            _db.SaveChanges();

            if (!string.IsNullOrEmpty(groupId.ToString()))
            {
                return RedirectToAction("ViewGroup", "Group", new { id = groupId });
            }
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