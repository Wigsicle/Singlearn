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
                            HttpContext.Session.SetString("student_id", student.student_id.ToString());
                            HttpContext.Session.SetString("role", "Student");
                            HttpContext.Session.SetString("class_id", student.class_id);
                            return RedirectToAction("Home", "Student");
                        }
                    }
                    else if (user.role == "Staff")
                    {
                        var staff = await dbContext.Staff.FirstOrDefaultAsync(s => s.user_id == user.user_id);
                        if (staff != null)
                        {
                            HttpContext.Session.SetString("staff_id", staff.staff_id.ToString());
                            HttpContext.Session.SetString("role", "Staff");
                            return RedirectToAction("Home", "Staff");
                        }
                    }
                    else if (user.role == "Admin")
                    {
                        var staff = await dbContext.Staff.FirstOrDefaultAsync(s => s.user_id == user.user_id);
                        if (staff != null)
                        {
                            HttpContext.Session.SetString("staff_id", staff.staff_id.ToString());
                            HttpContext.Session.SetString("role", "Admin");
                            return RedirectToAction("Index", "Admin");
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
            // Clear all the session data
            HttpContext.Session.Clear();

            // Optionally clear authentication cookies if using cookie authentication
            Response.Cookies.Delete(".AspNetCore.Cookies");

            // Redirect to the login page or home page
            return RedirectToAction("Login");
        }
    }
}
