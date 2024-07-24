using Microsoft.AspNetCore.Mvc;

namespace Singlearn.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
