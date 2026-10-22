using Microsoft.AspNetCore.Mvc;
using AI_Programming_Assistant.Models;

namespace AI_Programming_Assistant.Controllers
{
    public class login_create_accountController : Controller
    {
        private readonly AiProgrammingAssistantContext _context;

        public login_create_accountController(AiProgrammingAssistantContext context)
        {
            _context = context;
        }

        // =======================
        // عرض صفحة تسجيل الدخول
        // =======================
        public IActionResult login_create_account()
        {
            return View();
        }

     
        [HttpPost]
        public IActionResult login_create_account(string Username, string Password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == Username && u.Password == Password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId1", user.UserId);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.UserName ?? "");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login credentials";
            return View();
        }

        [HttpPost]
        public IActionResult Register(string new_user, string new_email, string new_pass1, string new_pass2)
        {
            if (new_pass1 != new_pass2)
            {
                ViewBag.Error = "Passwords do not match";
                return View("login_create_account");
            }

            var user = new User
            {
                UserName = new_user,
                Email = new_email,
                Password = new_pass1
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // 🔥 حفظ المستخدم مباشرة بعد التسجيل
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.UserName ?? "");

            return RedirectToAction("Index", "Home");
        }

     
    }
}