using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.ViewModel;

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

        [HttpGet]
        public IActionResult UserView()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            List<Group> userGroup = new List<Group>();

            foreach (var access in _context.Accesses) // TODO: Accesses to list first
            {
                if (access.Email == claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    int groupId = access.GroupId;
                    userGroup.Add(_context.Groups.Find(groupId));
                }
            }
            return View(userGroup);
        }
        [HttpPost]
        public IActionResult ViewGroupPost(Group group)
        {
            return RedirectToAction("item/Create");
        }

        // [Group Content] Get
        //[HttpGet("UserView/{id}")]
        public IActionResult ViewGroup(Group group)
        {
            var Id = group.Id;
            //TODO Get item list for this group from GroupContent table

            return View("ViewGroup", group);
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
                return NotFound(); //not found exception
            }
            return View(group);
        }

        // [CREATE] Get
        public IActionResult Create()
        {
            return View();
        }

        // [CREATE] Post
        [HttpPost]
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
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == obj.Email.ToLower());
            if (user == null)
            {
                TempData["Error"] = $"User with email {obj.Email} does not exist.";
                return RedirectToAction("AddMember");
            }

            MemberAccess access = new MemberAccess();
            access.Email = obj.Email;
            access.GroupId = obj.GroupId;
            //System.Console.WriteLine(obj.GroupId);
            access.Role = "User";
            access.Status = true;

            _context.Accesses.Add(access);
            _context.SaveChanges();
            //Group obj = _context.Groups.Find(id);
            TempData["Success"] = $"User with email {obj.Email} added successfuly.";
            return RedirectToAction("Index");
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

