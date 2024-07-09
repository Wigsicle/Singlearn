using Microsoft.AspNetCore.Mvc;
using SinglearnWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace SinglearnWeb.Controllers
{
    public class HomeworkHubController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeworkHubController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult staff_subject()
        {
            return View();
        }

        public IActionResult staff_class()
        {
            return View();
        }

        public IActionResult staff_homework()
        {
            return View();
        }

        public IActionResult staff_submission()
        {
            return View();
        }

    }
}
