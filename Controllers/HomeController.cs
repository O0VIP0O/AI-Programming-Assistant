using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AI_Programming_Assistant.Models;

namespace AI_Programming_Assistant.Controllers
{
    public class HomeController : Controller
    {
        private readonly AiProgrammingAssistantContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AiProgrammingAssistantContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId1");

            if (userId != null)
            {
                var userData = _context.Users
                    .FirstOrDefault(u => u.UserId == userId);

                if (userData != null)
                {
                    ViewBag.user_name = userData.UserName;
                    ViewBag.user_id = userData.UserId;
                    ViewBag.user_email = userData.Email;

                    var userProgressList = _context.UsersProgrammingLanguages
                        .Include(u => u.Language) 
                        .Where(u => u.UserId == userId)
                        .ToList();

                    ViewBag.userProgress = userProgressList;
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}