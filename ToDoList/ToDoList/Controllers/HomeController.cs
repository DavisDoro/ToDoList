using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using ToDoList.Data;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthRepository _authRepo;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IAuthRepository authRepo, ApplicationDbContext context)
        {
            _logger = logger;
            _authRepo = authRepo;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secured()
        {
            return View();
        }
        [HttpGet("register")]
        public IActionResult Register(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        //[HttpPost("register")]
        //public async Task<IActionResult> SendData(string email, string username, string password, string confirmPassword)
        //{
        //    if (await _authRepo.UserExists(email))
        //    {
        //        TempData["Error"] = "Error. This email address is already in use";
        //        return View("register");
        //    }
        //    if (password != confirmPassword)
        //    {
        //        TempData["Error"] = "Error. Password did not match";
        //        return View("register");
        //    }
        //    var user = new User
        //    {
        //        Username = username,
        //        Email = email,
        //    };
        //    _authRepo.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        //    user.PasswordHash = passwordHash;
        //    user.PasswordSalt = passwordSalt;

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    TempData["Error"] = "Reg complete.";
        //    return View("login");
        //}
        [HttpPost("register")]
        public async Task<IActionResult> SendData(UserRegister incomingUser)
        {
            if (!ModelState.IsValid)
            {
                return View("register");
            }
            if (await _authRepo.UserExists(incomingUser.Email))
            {
                TempData["Error"] = "This email address is already in use";
                return View("register");
            }
            if (incomingUser.Password != incomingUser.ConfirmPassword)
            {
                return View("register");
            }
            var user = new User
            {
                Username = incomingUser.Username,
                Email = incomingUser.Email,
            };
            _authRepo.CreatePasswordHash(incomingUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registration complete";
            return View("login");
        }

    
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"]=returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string username, string password, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                TempData["Error"] = "Error. Username or Password is invalid";
                return View("login");
            }
            else if (!_authRepo.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                TempData["Error"] = "Error. Username or Password is invalid";
                return View("login");
            }
            else // get cookie and claims for logged in user
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", user.Username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Email));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect("/item");
            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        public IActionResult Privacy()
        {
            ViewData["Message"] = "Use this page to detail your site's privacy policy.";
            return View("Privacy");
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
