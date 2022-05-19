using Microsoft.AspNetCore.Mvc;

namespace YogaApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
