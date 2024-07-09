using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Singlearn.Data;
using Singlearn.Models;
using Singlearn.Models.Entities;
using System.Threading.Tasks;

namespace Singlearn.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AuthController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await dbContext.Users.SingleOrDefaultAsync(u => u.email == model.Email);
                if (user != null && user.password == model.Password)
                {
                    if (user.role == "Student")
                    {
                        var student = await dbContext.Students.FirstOrDefaultAsync(s => s.user_id == user.user_id);
                        if (student != null)
                        {
                            return RedirectToAction("Home", "Student", new { id = student.student_id });
                        }
                    }
                    else if (user.role == "Staff")
                    {
                        var staff = await dbContext.Staff.FirstOrDefaultAsync(s => s.user_id == user.user_id);
                        if (staff != null)
                        {
                            return RedirectToAction("Home", "Staff", new { id = staff.staff_id });
                        }
                    }
                }
            }
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
