using Microsoft.AspNetCore.Mvc;

namespace AI_Programming_Assistant.Controllers
{
    public class landing_pageController : Controller
    {
        public IActionResult landing_page()
        {
            return View();
        }

    }
}