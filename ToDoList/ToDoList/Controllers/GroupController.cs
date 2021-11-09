using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
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

        public IActionResult UserView()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            List<Group> userGroup = new List<Group>();

            foreach (var access in _context.Accesses)
            {
                if (access.Email == claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    int groupId = access.GroupId;
                    userGroup.Add(_context.Groups.Find(groupId));
                }
            }
            return View(userGroup);
        }

        // [CREATE] Get
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //[DELETE] Get
        [Authorize]
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
        [Authorize]
        [HttpGet("AddMember/{id}")]
        public IActionResult AddMember(int id)
        {
            Group obj = _context.Groups.Find(id);

            return View(new AddMember { GroupName = obj.Name, GroupId = id });
        }
        //[ADD MEMBER] Post
        [Authorize]
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
            access.Email = obj.Email;
            access.GroupId = obj.GroupId;
            System.Console.WriteLine(obj.GroupId);
            access.Role = "User";
            access.Status = true;

            _context.Accesses.Add(access);
            _context.SaveChanges();
            //Group obj = _context.Groups.Find(id);
            TempData["Success"] = $"User with email {obj.Email} added successfuly.";
            return RedirectToAction("Index");
        }

        // [DELETE] Post
        [Authorize]
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

