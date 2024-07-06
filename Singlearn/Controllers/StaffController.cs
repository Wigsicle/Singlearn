using Microsoft.AspNetCore.Mvc;
using SinglearnWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace SinglearnWeb.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StaffController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IActionResult home()
        {
            return View();
        }

        public IActionResult profile()
        {
            return View();
        }

        public IActionResult homeworkhub_subject()
        {
            return View();
        }

        public IActionResult homeworkhub_class()
        {
            return View();
        }

        public IActionResult homeworkhub_homework()
        {
            return View();
        }

        public IActionResult homeworkhub_submission()
        {
            return View();
        }

    }
}
