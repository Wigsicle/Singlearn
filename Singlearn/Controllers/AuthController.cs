using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SinglearnWeb.Data;
using SinglearnWeb.Models;
using SinglearnWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace SinglearnWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AuthController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]

        public async Task<IActionResult> Login()
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
                        return RedirectToAction("Home", "Student");
                    }
                    else if (user.role == "Staff")
                    {
                        return RedirectToAction("Home", "Staff");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Login");
        }


    }
}
