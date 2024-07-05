using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinglearnWeb.Data;

namespace SinglearnWeb.Controllers
{
    public class StudentController : Controller
    {

        private readonly ApplicationDbContext dbContext;

        public StudentController(ApplicationDbContext dbContext)
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

        [HttpGet]
        public async Task<IActionResult> profile(string id)
        {
            var student = await dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View("Profile", student);
        }
    }
}
