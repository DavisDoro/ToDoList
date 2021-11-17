using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        
        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            List<Group> objList = new List<Group>();
            foreach (var group in _context.Groups)
            {
                if (group.Owner == claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    objList.Add(group);
                }
            }
            return View(objList);
        }

        public IActionResult GroupListView()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string userEmail = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Group> userGroup = new List<Group>();
            List<MemberAccess> accessForThisUser = new List<MemberAccess>();
            foreach (var access in _context.Accesses)
            {
                if (access.Email == claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value.ToLower())
                {

                    userGroup.Add(_context.Groups.Find(access.GroupId));

                    accessForThisUser.Add(access);
                }
            }
            ViewGroupsModel viewModel = new ViewGroupsModel()
            {
                Groups = userGroup,
                Accesses = accessForThisUser
            };


            return View(viewModel);
        }

        // [View Group] Get
        public IActionResult ViewGroup(Group obj)
        {
            List<Item> itemList = new List<Item>();
            List<Group> groupList = new List<Group>();

            foreach (var group in _context.Groups)
            {
                if (group.Id == obj.Id)
                {
                    groupList.Add(group);
                }
            }

            foreach (var item in _context.Items)
            {
                if (item.GroupId == obj.Id)
                {
                    itemList.Add(item);
                }
            }
            List<Group> groups = new List<Group>();
            groups.Add(obj);

            GroupAndItemModel viewItem = new GroupAndItemModel()
            {
                Groups = groupList,
                Items = itemList
            };

            return View(viewItem);
        }

        // [CREATE] Get
        public IActionResult Create()
        {

            return View();
        }

        //[DELETE] Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var group = _context.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // [CREATE] Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateGroup obj)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string owner = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            Group addGroup = new Group();
            addGroup.Name = obj.Name;
            addGroup.Description = obj.Description;
            addGroup.Owner = owner;
            _context.Groups.Add(addGroup);
            _context.SaveChanges();

            MemberAccess access = new MemberAccess();
            access.Email = owner;
            access.Role = "Owner";
            access.GroupId = addGroup.Id;
            access.Status = true;
            _context.Accesses.Add(access);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //[ADD MEMBER] Get
        [HttpGet("AddMember/{id}")]
        public IActionResult AddMember(int id)
        {
            Group obj = _context.Groups.Find(id);

            return View(new AddMember { GroupName = obj.Name, GroupId = id });
        }

        //[ADD MEMBER] Post
        [HttpPost("AddMember/{id}")]
        public async Task<IActionResult> AddMember(AddMember obj)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower().Equals(obj.Email.ToLower()));
            if (user == null)
            {
                TempData["Error"] = $"User with email {obj.Email} does not exist.";
                return RedirectToAction("AddMember");
            }

            MemberAccess access = new MemberAccess();
            access.Email = obj.Email.ToLower();
            access.GroupId = obj.GroupId;
            System.Console.WriteLine(obj.GroupId);
            access.Role = "User";
            access.Status = false;

            _context.Accesses.Add(access);
            _context.SaveChanges();
            //Group obj = _context.Groups.Find(id);
            TempData["Success"] = $"User with email {obj.Email} added successfuly.";
            return RedirectToAction("Index");
        }

        //[Join Group] Post
        // User accepts group invite
        public IActionResult JoinGroup(MemberAccess model)
        {
            MemberAccess access = _context.Accesses.Find(model.Id);
            access.Status = true;
            _context.Accesses.Update(access);
            _context.SaveChanges();

            return RedirectToAction("GroupListView");
        }

        // [DELETE] Post
        public IActionResult DeleteGroup(int id)
        {
            List<MemberAccess> accessList = _context.Accesses.ToList();
            foreach (var access in accessList)
            {

                if (access.GroupId == id)
                {
                    _context.Accesses.Remove(access);
                }
            }
            var obj = _context.Groups.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _context.Groups.Remove(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
